using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MechController
{
    CharacterController characterController;

    private Vector3 inputDirection;

    [HideInInspector]
    public Vector3 enemyCenterMass;
    float targetDistance;
    float firingAngle;

    public Camera cam;

    Animator legsAnimator;
	//Animator rightWeaponAnimator;
    //Animator leftWeaponAnimator;

    new void Start()
    {
        base.Start();

        characterController = GetComponent<CharacterController>();

        cam = FindObjectOfType<Camera>();
        cam.gameObject.GetComponent<MechCamera_Player>().target = armRotAxis;
    }

     void Update()
    {
        // For Raycasting
        forward = armRotAxis.TransformDirection(Vector3.forward) * 20;

        /**
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
        */
		
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

    public void AttachCamera(Camera cam)
    {

    }

    public void GetInput()
    {
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKeyDown(KeyCode.R))
        {
			Reload();
        }

        if (Input.GetMouseButton(0))
        {

            Fire();
        }
        else
        {
            //OnCooldown();
        }

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
        Vector3 eulerRotationTorso = new Vector3(torsoRotAxis.transform.eulerAngles.x, cam.transform.eulerAngles.y, torsoRotAxis.transform.eulerAngles.z);
        torsoRotAxis.transform.rotation = Quaternion.Euler(eulerRotationTorso);

        // Arms turning (x only) for arms, connected to RotationAxis
        Vector3 eulerRotationArms = new Vector3(cam.transform.eulerAngles.x - 3, armRotAxis.eulerAngles.y, armRotAxis.eulerAngles.z);
        armRotAxis.rotation = Quaternion.Euler(eulerRotationArms);
    }

    
}