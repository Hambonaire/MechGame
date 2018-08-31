using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechController_Player : MonoBehaviour {

    public int walkSpeed = 4;
    public int runSpeed = 8;

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
    private float turnSmoothTime_Legs = .12f;

    public GameObject cam;
    public GameObject torso;
    public GameObject legs;
    public CharacterController characterController;

    private Vector3 inputDirection;

    public Transform rightArmBarrel;
    public Transform leftArmBarrel;

    [SerializeField]
    int ammoRight;
    [SerializeField]
    int ammoLeft;

    public GameObject bulletRight;
    public float rateOfFireRight;
    float nextFireRight;
    public int maxAmmoRight;
    public float reloadTimeRight;
    float nextReloadRightEnd;
    bool reloadingRight = false;

    public GameObject bulletLeft;  
    public float rateOfFireLeft;  
    float nextFireLeft;  
    public int maxAmmoLeft;
    public float reloadTimeLeft;    
    float nextReloadLeftEnd;   
    bool reloadingLeft = false;

    private void Start()
    {
        ammoRight = maxAmmoRight;
        ammoLeft = maxAmmoLeft;
    }

    private void Update()
    {
        #region Input

        // Get input direction
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetMouseButton(0))
        {
            FireRight(rateOfFireRight);
        }
        if (reloadingRight && Time.time > nextReloadRightEnd)
        {
            ammoRight = maxAmmoRight;
            reloadingRight = false;
        }

        if (Input.GetMouseButton(1))
        {
            FireLeft(rateOfFireLeft);
        }
        if (reloadingLeft && Time.time > nextReloadLeftEnd)
        {
            ammoLeft = maxAmmoLeft;
            reloadingLeft = false;
        }

        #endregion



        #region Turning & Movement

        //Leg Turning
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

        //Torso Turning
        Vector3 eulerRotation = new Vector3(cam.transform.transform.eulerAngles.x - 5, cam.transform.eulerAngles.y, torso.transform.eulerAngles.z);
        torso.transform.rotation = Quaternion.Euler(eulerRotation);

        #endregion

        Vector3 forwardRight = rightArmBarrel.transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(rightArmBarrel.position, forwardRight, Color.red);

        Vector3 forwardLeft = leftArmBarrel.transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(leftArmBarrel.position, forwardLeft, Color.green);
    }

    private void FireRight(float RoFRight)
    {
        if (Time.time > nextFireRight && ammoRight > 0)
        {
            nextFireRight = Time.time + RoFRight;
            ammoRight--;

            // Create the Bullet from the Bullet Prefab
            var bullet = (GameObject)Instantiate(bulletRight, rightArmBarrel.position, torso.transform.rotation);
            bullet.transform.rotation *= Quaternion.Euler(90, 0, 0);

            // Add velocity to the bullet
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 150;

            // Destroy the bullet after 2 seconds
            Destroy(bullet, 2.0f);
        }
        if (ammoRight == 0 && !reloadingRight)
        {
            nextReloadRightEnd = Time.time + reloadTimeRight;
            reloadingRight = true;
        }

    }

    private void FireLeft(float RoFLeft)
    {
        if (Time.time > nextFireLeft && ammoLeft > 0)
        {
            nextFireLeft = Time.time + RoFLeft;
            ammoLeft--;

            // Create the Bullet from the Bullet Prefab
            var bullet = (GameObject)Instantiate(bulletLeft, leftArmBarrel.position, torso.transform.rotation);
            bullet.transform.rotation *= Quaternion.Euler(90, 0, 0);

            // Add velocity to the bullet
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 80;

            // Destroy the bullet after 2 seconds
            Destroy(bullet, 5.0f);
        }
        if (ammoLeft == 0 && !reloadingLeft)
        {
            nextReloadLeftEnd = Time.time + reloadTimeLeft;
            reloadingLeft = true;
        }

    }

}
