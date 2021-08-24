using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MechController
{
    EquipmentManager equipRef;
    public CharacterController characterController;

    private Vector3 inputDirection;

    public ScriptableFloat playerHealthCurrent;
    public ScriptableFloat playerHealthMax;
    public ScriptableFloat playerShieldCurrent;
    public ScriptableFloat playerShieldMax;
    public ScriptableFloat playerTargetDistance;
    public ScriptableFloat playerTargetMaxDistance;
    public ScriptableBool autoTarget;

    [HideInInspector]
    public Vector3 enemyCenterMass;
    float targetDistance;
    float firingAngle;

    public GameObject cam;

    Animator legsAnimator;              // TODO: Look into Cockpit bobbing thru transform movement, not animations? Think u can "animate" movement in Unity... check into that
    //Animator rightWeaponAnimator;
    //Animator leftWeaponAnimator;

    //public List<WeaponMapStruct> reloadStructs = new List<WeaponMapStruct>();

    public bool[] reloadStruct = { false, false, false, false };
    //public List<WeaponMapStruct> cooldownStructs = new List<WeaponMapStruct>();

    List<List<List<int>>> testmap0 = new List<List<List<int>>>() {
        new List<List<int>> () { new List<int> { 1 }, new List<int> { 1 }, new List<int> { 1 } },
        new List<List<int>> () { new List<int> { 0 }, new List<int> { 0 }, new List<int> { 0 } }
    };
    List<List<List<int>>> testmap1 = new List<List<List<int>>>() {
        new List<List<int>> () { new List<int> { 0 }, new List<int> { 0 }, new List<int> { 0 } },
        new List<List<int>> () { new List<int> { 1 }, new List<int> { 1 }, new List<int> { 1 } }
    };

    void Start()
    {
        hitBox = GetComponent<CapsuleCollider>();
        equipRef = EquipmentManager.Instance;
        myStats = GetComponent<Stats>();
        //Variables.PlayerHealth_Max = playerStats.GetMaxHealth();
        Rebuild();

        InvokeRepeating("ReloadRepeating", 0, .25f);
    }

    void Update()
    {
        // For Raycasting
        forward = cockpitRotationCenter.TransformDirection(Vector3.forward) * 20;

        /*
            // For auto-correcting firing
            RaycastHit hit;
            Physics.SphereCast(torsoRoot.transform.position, 5f, forward, out hit, 150f);
            if(hit.transform != null)
            {

                // CHANGE .25 TO GET FROM PREFAB (ARM DISTANCE)

                firingAngle = 90 - (Mathf.Atan((hit.point - this.transform.position).magnitude / (cockpitItem.armDistanceX * overallScaleFactor)) * Mathf.Rad2Deg);

                if (hit.transform.gameObject.tag == "Enemy")
                {
                    Variables.HasTarget = true;
                    Variables.PlayerTargetTransform = hit.transform.gameObject.GetComponent<EnemyController>().getCenterMass();
                    targetDistance = Vector3.Distance(this.transform.position, hit.transform.position);
                    Debug.Log(hit.transform.gameObject.name);
                }
                else
                {
                    Variables.HasTarget = false;
                }
            }
            else
            {
                Variables.HasTarget = false;
                firingAngle = 0;
            }
        */

        //if (Variables.HasTarget)
        if (myTarget != null)
        {
            //targetDistance = Vector3.Distance(this.transform.position, Variables.PlayerTargetTransform);
            targetDistance = Vector3.Distance(this.transform.position, myTarget.transform.position);
        }
        else
        {
            firingAngle = 0;
        }

        firingAngle = Mathf.Clamp(firingAngle, 0f, 5f);

        GetInput();

        Move();

        // TODO: Weapon UI stuff, move to UI_inGame...
        /**
        if (right_fireType == FireType.Charge)
        {
            //Variables.PlayerPrimaryChargeVal
            playerRightFillBar.value = (right_chargeCurr / right_chargeTime);
        }
        else if (right_fireType == FireType.Beam)
        {
            //Variables.PlayerPrimaryChargeVal 
            playerRightFillBar.value = right_beamCurr / right_beamTime;
        }
        else
        {
            playerRightFillBar.value = 0;
        }

        if (left_FireType == FireType.Charge)
        {
            //Variables.PlayerSecondaryChargeVal 
            playerLeftFillBar.value = Mathf.Clamp01(left_chargeCurr / left_chargeTime);
        }
        else if (left_FireType == FireType.Beam)
        {
            //Variables.PlayerSecondaryChargeVal 
            playerLeftFillBar.value = Mathf.Clamp01(left_beamCurr / left_beamTime);
        }
        else
        {
            playerLeftFillBar.value = 0;
        }
        **/

        //Handled by Stats script
        //Variables.PlayerHealth_Curr = playerStats.GetCurrentHealth();
        //playerHealthCurrent.value = playerStats.GetCurrentHealth();
    }

    public void GetInput()
    {
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int i = 0; i < weapon_data.Length; i++)
            {
                weapon_data[i].executable.Reload();

                /*
                for (int j = 0; j < weapon_data[i].Count; j++)
                {
                    for (int k = 0; k < weapon_data[i][k].Count; k++)
                    {
                        weapon_data[i][j][k].executable.Reload();
                        if(weapon_data[i][j][k].executable.isReloading)
                            reloadStructs.Add(new WeaponMapStruct { a=i, b=j, c=k });
                    }
                }
                */
            }
        }

        if (Input.GetMouseButton(0))
        {
            Fire();
        }
        else
        {
            OnCooldown();
        }

        if (Input.GetMouseButton(1))
        {
            Fire();
        }
        else
        {
            OnCooldown();
        }
		
		// TODO: Make real input system using Unity inputs
		// 		 Should become a foreach Input Axii if: Fire() -> else: OnCooldown()
    }

    public void Move()
    {
        // Falling
        if (!characterController.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0;
        }

        // Leg Turning (turns the whole object)
        if (inputDirection != Vector3.zero)
        {
            float targetAngle_Legs = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle_Legs, ref turnSmoothVelocity_Legs, turnSmoothTime_Legs);
        }

        // Set the goal speed:
        //   0 if no movement buttons are pressed
        //   runSpeed if the player is running
        //   walkSpeed if the player is not running
        float goalSpeed = (inputDirection.magnitude == 0 || !movementEnabled) ? 0 : Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // Runnning?
        running = Input.GetKey(KeyCode.LeftShift);
        // Target Speed
        targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDirection.magnitude;

        // Movement
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref movementSmoothVelocity, movementSmoothTime);
        characterController.Move((velocity + transform.forward * currentSpeed) * Time.deltaTime);

        // Leg Animation
        if (legsAnimator != null)
        {
            //legsAnimator.SetFloat("moveBlend", currentSpeed);
            legsAnimator.SetFloat("moveBlend", ((running) ? 1 : .7f) * inputDirection.magnitude, movementSmoothTime, Time.deltaTime);
        }

        // Torso Turning (y only) for cockpit object itself
        Vector3 eulerRotationTorso = new Vector3(torsoRoot.transform.eulerAngles.x, cam.transform.eulerAngles.y, torsoRoot.transform.eulerAngles.z);
        torsoRoot.transform.rotation = Quaternion.Euler(eulerRotationTorso);

        // Arms turning (x only) for arms, connected to RotationAxis
        Vector3 eulerRotationArms = new Vector3(cam.transform.eulerAngles.x - 3, cockpitRotationCenter.eulerAngles.y, cockpitRotationCenter.eulerAngles.z);
        cockpitRotationCenter.rotation = Quaternion.Euler(eulerRotationArms);
    }

    /*
    void Fire(List<List<List<int>>> fireMap)
    {
        for (int i = 0; i < fireMap.Count; i++)
        {
            for (int j = 0; j < fireMap[i].Count; j++)
            {
                for (int k = 0; k < fireMap[i][j].Count && k < weapon_data[i][j].Count; k++)
                {
                    if (fireMap[i][j][k] == 1 && weapon_data[i][j][k] != null)
                    {
                        weapon_data[i][j][k].executable.Fire();
                        if (weapon_data[i][j][k].executable.isReloading)
                            SmartAddReload(new WeaponMapStruct { a = i, b = j, c = k });
                    }
                }
            }
        }
    }
    */

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

    /*
    void OnCooldown(List<List<List<int>>> fireMap)
    {
        for (int i = 0; i < fireMap.Count; i++)
        {
            for (int j = 0; j < fireMap[i].Count; j++)
            {
                for (int k = 0; k < fireMap[i][j].Count && k < weapon_data[i][j].Count; k++)
                {
                    if (fireMap[i][j][k] == 1 && weapon_data[i][j][k] != null)
                    {
                        weapon_data[i][j][k].executable.OnCooldown();
                    }
                }
            }
        }
    }
    */

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

    /*
    void ReloadRepeating()
    {
        for (int i = 0; i < reloadStructs.Count; i++)
        {
            weapon_data[reloadStructs[i].a][reloadStructs[i].b][reloadStructs[i].c].executable.Reload();
            if (!weapon_data[reloadStructs[i].a][reloadStructs[i].b][reloadStructs[i].c].executable.isReloading)
            {
                reloadStructs.Remove(reloadStructs[i]);
            }
        }
    }
    */

    void ReloadRepeating()
    {
        for(int i = 0; i < reloadStruct.Length; i++)
        {
            if (reloadStruct[i])
            {
                weapon_data[i].executable.Reload();
                if (!weapon_data[i].executable.isReloading)
                    reloadStruct[i] = false;
            }
        }
    }

    /*
    public void SmartAddReload(WeaponMapStruct str)
    {
        if (reloadStructs.Count == 0)
            reloadStructs.Add(str);
        else
        {
            bool isNew = true;

            for (int i = 0; i < reloadStructs.Count; i++)
            {
                if (reloadStructs[i].a == str.a &&
                    reloadStructs[i].b == str.b &&
                    reloadStructs[i].c == str.c)
                    isNew = false;
            }

            if (isNew) reloadStructs.Add(str);
        }
    }
    */

    public void SmartAddReload(int pos)
    {
        reloadStruct[pos] = true;
    }

    public void Rebuild()
    {
        autoTarget.value = false;

        if (equipRef.currentCockpit != null && equipRef.currentLegs != null)
        {
            #region Remove Current Items
            if (cockpit != null)
            {
                Destroy(cockpit);
            }
            if (legs != null)
            {
                Destroy(legs);
            }
            for (int i = 0; i < weapon_data.Length; i++)
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
            if (equipRef.currentLegs != null)
            {
                legs = Instantiate(equipRef.currentLegs.prefab, legsRoot.transform.position, transform.rotation);
                legs.transform.parent = legsRoot.transform;
                if (equipRef.currentCockpit != null) legs.transform.localScale = Vector3.one * equipRef.currentCockpit.scaleFactor; // new Vector3(cockpit.scaleFactor, cockpit.scaleFactor, cockpit.scaleFactor);
                else legs.transform.localScale = Vector3.one * equipRef.currentCockpit.scaleFactor;

                torsoRoot.transform.position = legs.transform.Find("TorsoConnection").position;
                myStats.OnEquipmentChanged(equipRef.currentLegs, null);
            }

            if (equipRef.currentCockpit != null)
            {
                cockpit = Instantiate(equipRef.currentCockpit.prefab, torsoRoot.transform.position, torsoRoot.transform.rotation);
                cockpit.transform.parent = torsoRoot.transform;

                cockpitRotationCenter = cockpit.transform.Find("RotationAxis");

                for (int i = 0; i < equipRef.currentWeapons.Length; i++)
                {
                    if (equipRef.currentWeapons[i] != null)
                    {
                        Datatype_Weapon data = new Datatype_Weapon();
                        GameObject wepTemp = Instantiate(equipRef.currentWeapons[i].prefab, this.cockpit.transform.Find("Connection_" + i).position, this.cockpit.transform.rotation) as GameObject;
                        wepTemp.transform.parent = cockpitRotationCenter.transform;

                        data.weapon_object = wepTemp;
                        data.executable = new WeaponExecutable(equipRef.currentWeapons[i], data.weapon_object.transform.Find("Barrel"), null, null, null);
                        weapon_data[i] = data;
                    }

                    /*
                    for (int j = 0; j < equipRef.currentWeapons[i].Count; j++)
                    {
                        while (weapon_data[i][j].Count < equipRef.currentWeapons[i][j].Count)
                        {
                            weapon_data[i][j].Add(null);
                        }

                        for (int k = 0; k < equipRef.currentWeapons[i][j].Count; k++)
                        {
                            if (equipRef.currentWeapons[i][j][k] != null)
                            {
                                Datatype_Weapon data = new Datatype_Weapon();
                                GameObject wepTemp = Instantiate(equipRef.currentWeapons[i][j][k].prefab, cockpit.transform.Find("Connection_" + i + j + k).position, cockpit.transform.rotation) as GameObject;
                                wepTemp.transform.parent = cockpitRotationCenter.transform;

                                data.weapon_object = wepTemp;
                                data.executable = new WeaponExecutable(equipRef.currentWeapons[i][j][k], data.weapon_object.transform.Find("Barrel"), wepTemp.transform.Find("AnimatorHolder").GetComponent<Animator>(), this, playerTargetDistance);
                                weapon_data[i][j][k] = data;

                                if (equipRef.currentWeapons[i][j][k].autoTarget)
                                {
                                    autoTarget.value = true;

                                    if (equipRef.currentWeapons[i][j][k].maxTrackDistance > playerTargetMaxDistance.value)
                                        playerTargetMaxDistance.value = equipRef.currentWeapons[i][j][k].maxTrackDistance;
                                }
                            }
                        }

                    }
                    */
                }
            }

            // TODO: Do UI passing stuff here? (ammo, reloading, etc.) Loop thru Executables or just plug in the existing loop^^^
            // TODO: Reminder: make sure stats references scriptableFloats, etc. for UI passing
            #endregion

            // TODO: All the animation stuff here, prob need to do loop stuff, Find(), etc. Calling the animators takes place in Fire & OnCooldown
            #region Animators
            
            if (legs.transform.Find("AnimatorHolder") != null)
            {
                legsAnimator = legs.transform.Find("AnimatorHolder").GetComponent<Animator>();
            }
            /*
            if (rightArmWeapon.transform.Find("AnimatorHolder") != null && rightArmWeapon != null)
            {
                rightWeaponAnimator = rightArmWeapon.transform.Find("AnimatorHolder").GetComponent<Animator>();
            }

            if (leftArmWeapon.transform.Find("AnimatorHolder") != null && leftArmWeapon != null)
            {
                leftWeaponAnimator = leftArmWeapon.transform.Find("AnimatorHolder").GetComponent<Animator>();
            }
            */
            #endregion

            #region Misc.
            walkSpeed = equipRef.currentLegs.walkSpeed * equipRef.currentLegs.scaleFactor;
            runSpeed = equipRef.currentLegs.runSpeed * equipRef.currentCockpit.scaleFactor;
            hitBox.radius = baseHitBoxRadius * equipRef.currentCockpit.scaleFactor;
            hitBox.height = baseHitBoxHeight * equipRef.currentCockpit.scaleFactor;
            hitBox.center = new Vector3(0, hitBox.height / 2 + .05f, hitBox.center.z);
            #endregion

            cockpit.transform.localScale = Vector3.one * equipRef.currentCockpit.scaleFactor; // new Vector3(currentCockpit.scaleFactor, currentCockpit.scaleFactor, currentCockpit.scaleFactor);
        }
    }
}
