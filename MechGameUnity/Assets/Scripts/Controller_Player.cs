using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Player : MonoBehaviour
{
    public struct WeaponMapStruct {
        public int a;
        public int b;
        public int c;
    }

    public CharacterController characterController;

    public Stats playerStats;

    public GameObject mechManager;

	private Vector3 inputDirection;

    public ScriptableFloat playerHealthCurrent;
    public ScriptableFloat playerHealthMax;
    public ScriptableGameObject playerTarget;
    [HideInInspector]
    public Vector3 enemyCenterMass;
    float targetDistance;
	float firingAngle;
	
    private Vector3 forward;

    [HideInInspector]
    public float gravity = -9.8f;
    [HideInInspector]
    public float walkSpeed;
    [HideInInspector]
    public float runSpeed;

    private Vector3 velocity = Vector3.zero;
    private float currentSpeed = 0;
    private float targetSpeed = 0;
    private float movementSmoothVelocity = 0;
    private float movementSmoothTime = 0.1f;
    private bool movementEnabled = true;
    private bool running;

    private float turnSmoothVelocity_Torso = 0;
    private float turnSmoothTime_Torso = .025f;
    private float turnSmoothVelocity_Legs = 0;
    private float turnSmoothTime_Legs = .2f;

    [HideInInspector]
    public CapsuleCollider hitBox;
    [HideInInspector]
    public float baseHitBoxRadius = 1f;
    [HideInInspector]
    public float baseHitBoxHeight = 3f;
    [HideInInspector]
    public float baseHitBoxCenter = 1.55f;

    public GameObject cam;
    [HideInInspector]
    public GameObject torsoRoot;
    [HideInInspector]
    public GameObject legsRoot;

    //public Transform legsBase;
    public Transform cockpitRotationCenter;
    public Transform torsoConnection;

    [HideInInspector]
    public float overallScaleFactor = 1;

    Animator legsAnimator;              // TODO: Look into Cockpit bobbing thru transform movement, not animations? Think u can "animate" movement in Unity... check into that
    //Animator rightWeaponAnimator;
    //Animator leftWeaponAnimator;

    public List<List<List<Datatype_Weapon>>> weapon_data = new List<List<List<Datatype_Weapon>>>(){
        new List<List<Datatype_Weapon>>() {
            new List<Datatype_Weapon>(),
            new List<Datatype_Weapon>(),
            new List<Datatype_Weapon>()
        },
        new List<List<Datatype_Weapon>>() {
            new List<Datatype_Weapon>(),
            new List<Datatype_Weapon>(),
            new List<Datatype_Weapon>()
        }
    };
    [HideInInspector]
    public GameObject cockpit;
    [HideInInspector]
    public GameObject legs;

    public List<WeaponMapStruct> reloadStructs = new List<WeaponMapStruct>();
    //public List<WeaponMapStruct> cooldownStructs = new List<WeaponMapStruct>();

    List<List<List<int>>> testmap0 = new List<List<List<int>>>() {
        new List<List<int>> { new List<int> { 1 } }, new List<List<int>> { new List<int> { 1 } }, new List<List<int>> { new List<int> { 1 } },
        new List<List<int>> { new List<int> { 0 } }, new List<List<int>> { new List<int> { 0 } }, new List<List<int>> { new List<int> { 0 } }
    };
    List<List<List<int>>> testmap1 = new List<List<List<int>>>() {
        new List<List<int>> { new List<int> { 0 } }, new List<List<int>> { new List<int> { 0 } }, new List<List<int>> { new List<int> { 0 } },
        new List<List<int>> { new List<int> { 1 } }, new List<List<int>> { new List<int> { 1 } }, new List<List<int>> { new List<int> { 1 } }
    };

    private void Start()
    {
        playerStats = GetComponent<Stats>();
        //Variables.PlayerHealth_Max = playerStats.GetMaxHealth();

        InvokeRepeating("ReloadRepeating", 0, .2f);
    }

    private void Update()
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
        if (playerTarget.value != null)
        {
            //targetDistance = Vector3.Distance(this.transform.position, Variables.PlayerTargetTransform);
            targetDistance = Vector3.Distance(this.transform.position, playerTarget.value.transform.position);
        }
        else
        {
            firingAngle = 0;
        }

        firingAngle = Mathf.Clamp(firingAngle, 0f, 5f);

        GetInput();

        Move();

        // TODO: Weapon UI stuff, move to UI_inGame...
        /*
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
        */

        //Handled by Stats script
        //Variables.PlayerHealth_Curr = playerStats.GetCurrentHealth();
        //playerHealthCurrent.value = playerStats.GetCurrentHealth();
    }

    public void GetInput()
    {
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int i = 0; i < weapon_data.Count; i++)
            {
                for (int j = 0; j < weapon_data[i].Count; j++)
                {
                    for (int k = 0; k < weapon_data[i][k].Count; k++)
                    {
                        weapon_data[i][j][k].executable.Reload();
                        reloadStructs.Add(new WeaponMapStruct { a=i, b=j, c=k });
                    }
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            Fire(testmap0);
        }
        else
        {
            OnCooldown(testmap0);
        }

        if (Input.GetMouseButton(1))
        {
            Fire(testmap1);
        }
        else
        {
            OnCooldown(testmap1);
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

    void Fire(List<List<List<int>>> fireMap)
    {
        for (int i = 0; i < fireMap.Count; i++)
        {
            for (int j = 0; j < fireMap[i].Count; j++)
            {
                for (int k = 0; k < fireMap[i][k].Count; k++)
                {
                    if (fireMap[i][j][k] == 1) weapon_data[i][j][k].executable.Fire();
                }
            }
        }
    }

    void OnCooldown(List<List<List<int>>> fireMap)
    {
        for (int i = 0; i < fireMap.Count; i++)
        {
            for (int j = 0; j < fireMap[i].Count; j++)
            {
                for (int k = 0; k < fireMap[i][k].Count; k++)
                {
                    if (fireMap[i][j][k] == 1) weapon_data[i][j][k].executable.OnCooldown();
                }
            }
        }
    }

    void ReloadRepeating()
    {
        foreach(WeaponMapStruct str in reloadStructs)
        {
            weapon_data[str.a][str.b][str.c].executable.Reload();
            if (weapon_data[str.a][str.b][str.c].executable.isReloading)
            {
                // Maybe do someting here?
            }
            else
            {
                reloadStructs.Remove(str);
            }
        }
    }

    public void Rebuild(Cockpit cockpit, Legs legs, List<List<List<Weapon>>> weps, List<Accessory> accs)
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
		for (int i = 0; i < weapon_data.Count; i++) 
		{
			for (int j = 0; j < weapon_data[i].Count; j++)
			{
				for (int k = 0; k < weapon_data[i][j].Count; k++)
				{
					weapon_data[i][j][k].Delete();
					weapon_data[i][i].RemoveAt(k);
				}
			}
		}
		
        #endregion
		
		#region Create New Items
		
		this.legs = Instantiate(legs.prefab, legsRoot.transform.position, transform.rotation);
        this.legs.transform.parent = legsRoot.transform;
        this.legs.transform.localScale = Vector3.one * cockpit.scaleFactor; // new Vector3(cockpit.scaleFactor, cockpit.scaleFactor, cockpit.scaleFactor);

        torsoRoot.transform.position = this.legs.transform.Find("TorsoConnection").position;

        this.cockpit = Instantiate(cockpit.prefab, torsoRoot.transform.position, torsoRoot.transform.rotation);
        this.cockpit.transform.parent = torsoRoot.transform;

        cockpitRotationCenter = this.cockpit.transform.Find("RotationAxis");	
			
		for (int i = 0; i < weps.Count; i++) 
		{
			for (int j = 0; j < weps[i].Count; j++)
			{
				for (int k = 0; k < weps[i][j].Count; k++)
				{
                    Datatype_Weapon data = new Datatype_Weapon();
					data.weapon_object = Instantiate(weps[i][j][k].prefab, this.cockpit.transform.Find("Connection_" + i + j + k).position, this.cockpit.transform.rotation) as GameObject;
					data.weapon_object.transform.parent = cockpitRotationCenter.transform;
					data.executable = new WeaponExecutable(weps[i][j][k], data.weapon_object.transform.Find("Barrel"));
					weapon_data[i][j].Add(data); 				
				}
			}
		}
		
		// TODO: Do UI passing stuff here? (ammo, reloading, etc.) Loop thru Executables or just plug in the existing loop^^^
		// TODO: Reminder: make sure stats references scriptableFloats, etc. for UI passing
	    #endregion

		// TODO: All the animation stuff here, prob need to do loop stuff, Find(), etc. Calling the animators takes place in Fire & OnCooldown
        #region Animators
		/*
        if (this.legs.transform.Find("AnimatorHolder") != null)
        {
            legsAnimator = this.legs.transform.Find("AnimatorHolder").GetComponent<Animator>();
        }

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
        walkSpeed = legs.walkSpeed * legs.scaleFactor;
        runSpeed = legs.runSpeed * cockpit.scaleFactor;
        hitBox.radius = baseHitBoxRadius * cockpit.scaleFactor;
        hitBox.height = baseHitBoxHeight * cockpit.scaleFactor;
        hitBox.center = new Vector3(0, hitBox.height / 2 + .05f, hitBox.center.z);
        #endregion

        this.cockpit.transform.localScale = Vector3.one * cockpit.scaleFactor; // new Vector3(currentCockpit.scaleFactor, currentCockpit.scaleFactor, currentCockpit.scaleFactor);
        
    }
}
