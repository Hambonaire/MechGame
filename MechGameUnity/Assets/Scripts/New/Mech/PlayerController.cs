using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MechController
{
    CharacterController characterController;

    private Vector3 inputDirection;

    public Camera cam;

    new void Start()
    {
        base.Start();

        characterController = GetComponent<CharacterController>();

		AttachCamera(LevelManager._instance.playerCam);
    }

     void Update()
    {
        // For Raycasting
        forward = armRotAxis.TransformDirection(Vector3.forward) * 20;

        if (cam != null)
            GetFiringSolutionPoint();

        Move();

        GetInput();
    }

    public void AttachCamera(Camera camera)
    {
		cam = camera;

        camera.GetComponent<CamPlayerMech>().baseTarget = transform;

        camera.GetComponent<CamPlayerMech>().target = armRotAxis;
        
        camera.GetComponent<CamPlayerMech>().camOffset = sectionManager.cameraOffset;
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
            OnCooldown();
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
        //float goalSpeed = (inputDirection.magnitude == 0 /*|| !movementEnabled*/) ? 0 : Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        float goalSpeed = (inputDirection.magnitude == 0 /*|| !movementEnabled*/) ? 0 : walkSpeed;

        // Runnning?
        //running = Input.GetKey(KeyCode.LeftShift);
        // Target Speed
        //targetSpeed = ((false) ? runSpeed : walkSpeed) * inputDirection.magnitude;
        targetSpeed = walkSpeed * inputDirection.magnitude;

        // Movement
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref movementSmoothVelocity, movementSmoothTime);
        characterController.Move((velocity + transform.forward * currentSpeed) * Time.deltaTime);

        // Leg Animation
        if (legsAnimator != null)
        {
            //legsAnimator.SetFloat("moveBlend", currentSpeed);
            legsAnimator.SetFloat("moveBlend", ((true) ? .75f : .7f) * inputDirection.magnitude, movementSmoothTime, Time.deltaTime);
        }

        // Torso Turning (y only) for cockpit object itself
        Vector3 eulerRotationTorso = new Vector3(torsoRotAxis.transform.eulerAngles.x, cam.transform.eulerAngles.y + 1, torsoRotAxis.transform.eulerAngles.z);
        torsoRotAxis.transform.rotation = Quaternion.Euler(eulerRotationTorso);

        // Arms turning (x only) for arms, connected to RotationAxis
        Vector3 eulerRotationArms = new Vector3(cam.transform.eulerAngles.x, armRotAxis.eulerAngles.y, armRotAxis.eulerAngles.z);
        armRotAxis.rotation = Quaternion.Euler(eulerRotationArms);
    }

    public override Vector3 GetFiringSolutionPoint()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Physics.Raycast(ray, out RaycastHit hit);

        return hit.point;
    }
}