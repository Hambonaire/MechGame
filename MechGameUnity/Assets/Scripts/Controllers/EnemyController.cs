using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyController : MechController
{
    public NavMeshAgent navAgent;
    //public CharacterController characterController;

    //public GameObject mechManager;

    //private Vector3 inputDirection;

    //public GameObject playerObject;
    public ScriptableFloat playerObjectDistance;
    Vector3 targetCenterMass;
    Transform targetTransform;
    float torsoTrackSpeed = .8f;

    Animator legsAnimator;
    //Animator rightWeaponAnimator;
    //Animator leftWeaponAnimator;

    //Weapons to attempt to emplace 
    public Cockpit cockpitItem;
    public Legs legsItem;

    public Weapon LeftHandWeapon;
    public Weapon RightHandWeapon;
    public Weapon LeftShoulderWeapon;
    public Weapon RightShoulderWeapon;

    public List<Weapon> LeftRegWeapons;
    public List<Weapon> RightRegWeapons;
    public List<Weapon> LeftShoulderWeapons;
    public List<Weapon> RightShoulderWeapons;

    //public List<WeaponMapStruct> reloadStructs = new List<WeaponMapStruct>();

    public bool[] reloadStruct = { false, false, false, false };

    List<List<List<int>>> testmap = new List<List<List<int>>>() {
        new List<List<int>> () { new List<int> { 1 }, new List<int> { 1 }, new List<int> { 1 } },
        new List<List<int>> () { new List<int> { 1 }, new List<int> { 1 }, new List<int> { 1 } }
    };


    private void Awake()
    {
        playerObjectDistance = ScriptableObject.CreateInstance("ScriptableFloat") as ScriptableFloat;
    }

    private void Start()
    {
        hitBox = GetComponent<CapsuleCollider>();

        myStats = GetComponent<Stats>();

        //navAgent = this.GetComponent<NavMeshAgent>();

        Initialize();

        navAgent.SetDestination(new Vector3(Random.Range(-45f, 45f), 0, Random.Range(-45f, 45f)));

        //InvokeRepeating("ReloadRepeating", 0, 1);
    }

    private void Update()
    {
        //TODO: If player is in range then fire
        Fire();

        if (!navAgent.pathPending)
        {
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)
                {
                    //navAgent.isStopped = true;
                    navAgent.SetDestination(new Vector3(Random.Range(-45f, 45f), 0, Random.Range(-45f, 45f)));
                }
            }
        }
        
        #region Turning & Movement
        /**
        // Falling
        if (!characterController.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0;
        }
        */

        /**
        if ((targetTransform.position - this.transform.position).magnitude > 14)
        {
            navAgent.isStopped = false;
            //navAgent.SetDestination(targetTransform.position);
            navAgent.SetDestination(new Vector3(Random.Range(-50f, 50f), 0, Random.Range(-50f, 50f)));
        }
        else
        {
            navAgent.isStopped = true;
        }
        */

        // Leg Turning       
        //float targetAngle_Legs = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
        //transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle_Legs, ref turnSmoothVelocity_Legs, turnSmoothTime_Legs);


        /**
        // Set the goal speed:
        //   0 if no movement buttons are pressed
        //   runSpeed if the player is running
        //   walkSpeed if the player is not running
        float goalSpeed = (inputDirection.magnitude == 0 || !movementEnabled) ? 0 : Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        */

        /**  
        // Runnning?
        //running = Input.GetKey(KeyCode.LeftShift);
        // Target Speed
        //targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDirection.magnitude;

        // Movement
        //currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref movementSmoothVelocity, movementSmoothTime);
        //characterController.Move((velocity + transform.forward * currentSpeed) * Time.deltaTime);
         */

        // Leg Animation
        if (legsAnimator != null) legsAnimator.SetFloat("moveBlend", ((navAgent.isStopped) ? 0f : .7f), movementSmoothTime, Time.deltaTime);

        // Torso Turning (y only) for cockpit object itself
        //Vector3 eulerRotationTorso = new Vector3(torsoRoot.transform.eulerAngles.x, cam.transform.eulerAngles.y, torsoRoot.transform.eulerAngles.z);
        //torsoRoot.transform.rotation = Quaternion.Euler(eulerRotationTorso);

        // Torso turning towards a target
        Quaternion OriginalRot = torsoRoot.transform.rotation;
        torsoRoot.transform.LookAt(myTarget.GetComponent<MechController>().TorsoRoot.transform);
        Quaternion NewRot = torsoRoot.transform.rotation;
        torsoRoot.transform.rotation = OriginalRot;
        torsoRoot.transform.rotation = Quaternion.Lerp(OriginalRot, NewRot, torsoTrackSpeed);

        //Debug.Log(torsoTrackSpeed * Time.deltaTime);
        // Arms turning (x) for arms, connected to RotationAxis
        //Vector3 eulerRotationArms = new Vector3(cam.transform.eulerAngles.x - 3, cockpitRotationCenter.eulerAngles.y, cockpitRotationCenter.eulerAngles.z);
        //cockpitRotationCenter.rotation = Quaternion.Euler(eulerRotationArms);
        #endregion

        Vector3 forward = cockpitRotationCenter.TransformDirection(Vector3.forward) * 10;
        //Debug.DrawRay(rightArmBarrel.position, forward, Color.red);

        //Debug.DrawRay(leftArmBarrel.position, forward, Color.green);

        //Debug.DrawRay(cockpitRotationCenter.position, forward * 5, Color.blue);
    }

    void Fire()
    {
        for (int i = 0; i < weapon_data.Length; i++)
        {
            if (weapon_data[i] != null)
            {
                weapon_data[i].executable.Fire();
                if (weapon_data[i].executable.isReloading)
                    SmartAddReload(i);
            }
        }
    }

    void OnCooldown()
    {
        for (int i = 0; i < weapon_data.Length; i++)
        {
            if (weapon_data[i] != null)
            {
                weapon_data[i].executable.OnCooldown();
            }
        }
    }

    void ReloadRepeating()
    {
        for (int i = 0; i < reloadStruct.Length; i++)
        {
            if (reloadStruct[i])
            {
                weapon_data[i].executable.Reload();
                if (!weapon_data[i].executable.isReloading)
                    reloadStruct[i] = false;
            }
        }
    }

    public void SmartAddReload(int pos)
    {
        reloadStruct[pos] = true;
    }

    public void Initialize()
    {
        #region Beforehand Calculations
        overallScaleFactor = cockpitItem.scaleFactor;
        #endregion

        #region Remove Current Items
        if (cockpit != null)
        {
            Destroy(cockpit);
        }
        if (legs != null)
        {
            Destroy(legs);
        }
        for(int i = 0; i < weapon_data.Length; i++)
        {
            if (weapon_data[i] != null)
            {
                weapon_data[i].Delete();
                weapon_data[i] = null;
            }

            /*
            for (int j = 0; j < weapon_data[i].Count; j++)
            {
                for (int k = 0; k < weapon_data[i][j].Count; k++)
                {
                    if (weapon_data[i][j][k] != null)
                    {
                        weapon_data[i][j][k].Delete();
                        weapon_data[i][j].RemoveAt(k);
                    }
                }

                weapon_data[i][j].Clear();
            }
            */
        }
        #endregion

        #region Create New Items
        legs = Instantiate(legsItem.prefab, legsRoot.transform.position, transform.rotation);
        legs.transform.parent = legsRoot.transform;
        legs.transform.localScale = cockpitItem.scaleFactor * Vector3.one;// new Vector3(overallScaleFactor, overallScaleFactor, overallScaleFactor);
        torsoRoot.transform.position = this.legs.transform.Find("TorsoConnection").position;
        myStats.OnEquipmentChanged(legsItem, null);

        cockpit = Instantiate(cockpitItem.prefab, torsoRoot.transform.position, torsoRoot.transform.rotation);
        cockpit.transform.parent = torsoRoot.transform;
        cockpitRotationCenter = this.cockpit.transform.Find("RotationAxis");
        myStats.OnEquipmentChanged(cockpitItem, null);

        #region Instantiate Weapons

        if (LeftHandWeapon != null)
        {
            Datatype_Weapon data = new Datatype_Weapon();
            GameObject wepTemp = Instantiate(LeftHandWeapon.prefab, this.cockpit.transform.Find("Connection_" + 0).position, this.cockpit.transform.rotation) as GameObject;
            wepTemp.transform.parent = cockpitRotationCenter.transform;

            data.weapon_object = wepTemp;
            data.executable = new WeaponExecutable(LeftHandWeapon, data.weapon_object.transform.Find("Barrel"), null, this, playerObjectDistance);

            weapon_data[0] = data;
            myStats.OnEquipmentChanged(LeftHandWeapon, null);
        }
        if (RightHandWeapon != null)
        {
            Datatype_Weapon data = new Datatype_Weapon();
            GameObject wepTemp = Instantiate(RightHandWeapon.prefab, this.cockpit.transform.Find("Connection_" + 1).position, this.cockpit.transform.rotation) as GameObject;
            wepTemp.transform.parent = cockpitRotationCenter.transform;

            data.weapon_object = wepTemp;
            data.executable = new WeaponExecutable(RightHandWeapon, data.weapon_object.transform.Find("Barrel"), null, this, playerObjectDistance);

            weapon_data[0] = data;
            myStats.OnEquipmentChanged(RightHandWeapon, null);
        }
        if (LeftShoulderWeapon != null)
        {
            Datatype_Weapon data = new Datatype_Weapon();
            GameObject wepTemp = Instantiate(LeftShoulderWeapon.prefab, this.cockpit.transform.Find("Connection_" + 0).position, this.cockpit.transform.rotation) as GameObject;
            wepTemp.transform.parent = cockpitRotationCenter.transform;

            data.weapon_object = wepTemp;
            data.executable = new WeaponExecutable(LeftShoulderWeapon, data.weapon_object.transform.Find("Barrel"), null, this, playerObjectDistance);

            weapon_data[0] = data;
            myStats.OnEquipmentChanged(LeftShoulderWeapon, null);
        }
        if (RightShoulderWeapon != null)
        {
            Datatype_Weapon data = new Datatype_Weapon();
            GameObject wepTemp = Instantiate(RightHandWeapon.prefab, this.cockpit.transform.Find("Connection_" + 1).position, this.cockpit.transform.rotation) as GameObject;
            wepTemp.transform.parent = cockpitRotationCenter.transform;

            data.weapon_object = wepTemp;
            data.executable = new WeaponExecutable(RightHandWeapon, data.weapon_object.transform.Find("Barrel"), null, this, playerObjectDistance);

            weapon_data[0] = data;
            myStats.OnEquipmentChanged(RightHandWeapon, null);
        }
        #endregion

        /*
        while (weapon_data[0][0].Count < LeftRegWeapons.Count)
        {
            weapon_data[0][0].Add(null);
        }
        for (int i = 0; i < LeftRegWeapons.Count; i++)
        {
            if (i < cockpitItem.leftRegularCount && LeftRegWeapons[i] != null)
            {
                Datatype_Weapon data = new Datatype_Weapon();
                GameObject wepTemp = Instantiate(LeftRegWeapons[i].prefab, this.cockpit.transform.Find("Connection_" + 0 + 0 + i).position, this.cockpit.transform.rotation) as GameObject;
                wepTemp.transform.parent = cockpitRotationCenter.transform;

                data.weapon_object = wepTemp;
                data.executable = new WeaponExecutable(LeftRegWeapons[i], data.weapon_object.transform.Find("Barrel"), null, this, playerObjectDistance);

                weapon_data[0][0][i] = data;
                myStats.OnEquipmentChanged(LeftRegWeapons[i], null);
            }
        }
        while (weapon_data[1][0].Count < RightRegWeapons.Count)
        {
            weapon_data[1][0].Add(null);
        }
        for (int i = 0; i < RightRegWeapons.Count; i++)
        {
            if (i < cockpitItem.rightRegularCount && RightRegWeapons[i] != null)
            {
                Datatype_Weapon data = new Datatype_Weapon();
                GameObject wepTemp = Instantiate(RightRegWeapons[i].prefab, this.cockpit.transform.Find("Connection_" + 1 + 0 + i).position, this.cockpit.transform.rotation) as GameObject;
                wepTemp.transform.parent = cockpitRotationCenter.transform;

                data.weapon_object = wepTemp;
                data.executable = new WeaponExecutable(RightRegWeapons[i], data.weapon_object.transform.Find("Barrel"), null, this, playerObjectDistance);

                weapon_data[1][0][i] = data;
            }
        }
        while (weapon_data[0][2].Count < LeftShoulderWeapons.Count)
        {
            weapon_data[0][2].Add(null);
        }
        for (int i = 0; i < LeftShoulderWeapons.Count; i++)
        {
            if (i < cockpitItem.leftShoulderCount && LeftShoulderWeapons[i] != null)
            {
                Datatype_Weapon data = new Datatype_Weapon();
                GameObject wepTemp = Instantiate(LeftShoulderWeapons[i].prefab, this.cockpit.transform.Find("Connection_" + 0 + 2 + i).position, this.cockpit.transform.rotation) as GameObject;
                wepTemp.transform.parent = cockpitRotationCenter.transform;

                data.weapon_object = wepTemp;
                data.executable = new WeaponExecutable(LeftShoulderWeapons[i], data.weapon_object.transform.Find("Barrel"), null, this, playerObjectDistance);

                weapon_data[0][2][i] = data;
            }
        }
        while (weapon_data[1][2].Count < RightShoulderWeapons.Count)
        {
            weapon_data[1][2].Add(null);
        }
        for (int i = 0; i < RightShoulderWeapons.Count; i++)
        {
            if (i < cockpitItem.rightRegularCount && RightShoulderWeapons[i] != null)
            {
                Datatype_Weapon data = new Datatype_Weapon();
                GameObject wepTemp = Instantiate(RightShoulderWeapons[i].prefab, this.cockpit.transform.Find("Connection_" + 1 + 2 + i).position, this.cockpit.transform.rotation) as GameObject;
                wepTemp.transform.parent = cockpitRotationCenter.transform;

                data.weapon_object = wepTemp;
                data.executable = new WeaponExecutable(RightShoulderWeapons[i], data.weapon_object.transform.Find("Barrel"), null, this, playerObjectDistance);

                weapon_data[1][2][i] = data;
            }
        }
        */
        #endregion

        #region Animators
        if (legs.transform.Find("AnimatorHolder") != null)
        {
            legsAnimator = legs.transform.Find("AnimatorHolder").GetComponent<Animator>();
        }
        #endregion

        #region Misc.
        hitBox.radius = baseHitBoxRadius * cockpitItem.scaleFactor;
        hitBox.height = baseHitBoxHeight * cockpitItem.scaleFactor;
        hitBox.center = new Vector3(0, hitBox.height / 2 + .05f, hitBox.center.z);
        #endregion

        cockpit.transform.localScale = Vector3.one * cockpitItem.scaleFactor;
    }

}
