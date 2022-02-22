using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MechController
{
    CharacterController characterController;

    //HUDManager hudManager;

    #region Inputs
    private Vector3 inputDirection;
    private float mouseX;
    private float mouseY;
    #endregion

    #region Camera
    public float mouseSensitivity = 8;
    public float zoomedMouseSensitivity = 3;
    Vector2 pitchMinMax = new Vector2(-30, 45);

    Vector3 currentRotation;
    float rotationSmoothTime = .1f;
    Vector3 rotationSmoothVelocity;

    float pitch;
    float yaw;

    bool zoomed = false;
    float zoom = 60;
    #endregion

    MechManager lastTarget;

    public new void Initialize()
    {
        base.Initialize();
    }

    new void Start()
    {
        base.Start();

        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        weaponSystem.FindTargetInView();

        if (weaponSystem.target != null)
        {
            if (lastTarget == null)
            {
                lastTarget = weaponSystem.target;
                lastTarget.outlineHandler.EnableOutlines(true);
            }
            else if (weaponSystem.target != lastTarget)
            {
                lastTarget.outlineHandler.EnableOutlines(false);
                lastTarget = weaponSystem.target;
                lastTarget.outlineHandler.EnableOutlines(true);
            }
        }
        else if (lastTarget != null)
        {
            lastTarget.outlineHandler.EnableOutlines(false);
        }

        GetInput();

        Move();

        MoveCamera();
    }

    public void AttachCamera(Camera camera, CinemachineVirtualCamera virtualCam, HUDManager myHUD)
    {
        mechCamera = camera;
        virtualCamera = virtualCam;

        cameraPlaceholder = new GameObject();

        virtualCamera.Follow = cameraPlaceholder.transform;
        virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset = new Vector3(sectionManager.cameraOffset.x, sectionManager.cameraOffset.y, 0);
        virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = sectionManager.cameraOffset.z;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (mechManager == null)
            mechManager = GetComponent<MechManager>();

        mechManager.controllerHUD = myHUD;

        myHUD.connectedPlayer = mechManager;
        myHUD.connectedSystem = weaponSystem;
    }

    void GetInput()
    {
        // Mouse input for cam movement
        mouseX = InputManager._instance.MouseX; //Input.GetAxis("Mouse X");
        mouseY = -InputManager._instance.MouseY; //-Input.GetAxis("Mouse Y");

        //inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        inputDirection = new Vector3(InputManager._instance.MoveAxisX, 0, InputManager._instance.MoveAxisY).normalized;

        /*
        if (Input.GetKeyDown(KeyCode.R))
        {
			Reload();
        }
        */

        //if (Input.GetMouseButton(0))
        if (InputManager._instance.LeftHeld)
        {
            weaponSystem.FireByGroup(0, GetFiringSolutionPoint());
        }

        if (InputManager._instance.RightClick)
        {
            // already zoom, zoom out
            if (zoomed)
                mechManager.controllerHUD.OnZoom(30f, 60f);
            else
                mechManager.controllerHUD.OnZoom(60f, 30f);
            zoomed = !zoomed;
        }

        //if (Input.mouseScrollDelta.y != 0)
        if (InputManager._instance.ScrollDelta != 0)
        {
            weaponSystem.selectedGroup -= (InputManager._instance.ScrollDelta > 0 ? 1 : -1);
        }

        weaponSystem.selectedGroup = Mathf.Clamp(weaponSystem.selectedGroup, 0, 2);
    }

    void MoveCamera()
    {
        // Rotate
        yaw += mouseX * (zoomed ? zoomedMouseSensitivity: mouseSensitivity);
        pitch += mouseY * (zoomed ? zoomedMouseSensitivity : mouseSensitivity);
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        cameraPlaceholder.transform.localEulerAngles = currentRotation;

        // Move
        /*
        cameraPlaceholder.transform.position =
            //For height
            new Vector3(armRotAxis.position.x, transform.position.y + sectionManager.cameraOffset.y, armRotAxis.position.z)//
            +
            cameraPlaceholder.transform.right * sectionManager.cameraOffset.x
            -
            // For distance behind
            (cameraPlaceholder.transform.forward * sectionManager.cameraOffset.z);
        */
        cameraPlaceholder.transform.position = new Vector3(armRotAxis.position.x, transform.position.y, armRotAxis.position.z);
    }

    protected override void Move()
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
            float targetAngle_Legs = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraPlaceholder.transform.eulerAngles.y;
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


        // Arms turning (x only) for arms, connected to RotationAxis
        var targetArmRot = Quaternion.Euler(cameraPlaceholder.transform.eulerAngles.x, armRotAxis.eulerAngles.y, armRotAxis.eulerAngles.z);
        armRotAxis.rotation = Quaternion.Slerp(armRotAxis.rotation, targetArmRot, 5 * Time.deltaTime);

        /*
        // Torso Turning (y only) for cockpit object itself
        // Torso rotation -> Encapsulatin object rotation offset
        var curFwd = transform.forward;
        // measure the angle rotated since last frame:
        angDiff = Vector3.Angle(curFwd, lastFwd);
        if (angDiff > 0.01)
        {
            // if rotated a significant angle...
            // fix angle sign...
            if (Vector3.Cross(curFwd, lastFwd).y < 0) angDiff = -angDiff;
            lastFwd = curFwd; // and update lastFwd
        }
        torsoRotAxis.transform.eulerAngles = new Vector3(torsoRotAxis.transform.eulerAngles.x, torsoRotAxis.transform.eulerAngles.y + angDiff, torsoRotAxis.transform.eulerAngles.z);
        */

        OffsetTorsoLegTurn(ref lastForward);

        // Smooth follow camera
        Vector3 camLookAngle = Quaternion.LookRotation(cameraPlaceholder.transform.forward).eulerAngles;
        var targetTorsoAngle = Quaternion.Euler(Vector3.Scale(camLookAngle, Vector3.up));
        torsoRotAxis.rotation = Quaternion.Slerp(torsoRotAxis.rotation, targetTorsoAngle, 5 * Time.deltaTime);
    }

    public override Vector3 GetFiringSolutionPoint()
    {
        Ray ray = mechCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Physics.Raycast(ray, out RaycastHit hit);

        return hit.point;
    }

    public override void OnWeaponFire(float intensity)
    {
        mechManager.controllerHUD.OnFire(intensity);
    }

    protected override void SectionTookDamage(MechManager source, float damageTaken)
    {
        mechManager.controllerHUD.GotHit(damageTaken);

    }
}