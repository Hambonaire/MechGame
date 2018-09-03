﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechController_Player : MonoBehaviour
{

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
    public GameObject torsoRoot;
    public GameObject legsRoot;
    public CharacterController characterController;

    private Vector3 inputDirection;

    public Cockpit cockpitItem;
    public Weapon rightWeaponItem;
    public Weapon leftWeaponItem;

    GameObject rightArmWeapon;
    GameObject leftArmWeapon;
    GameObject cockpit;

    Transform torsoConnection;
    Transform rightArmBarrel;
    Transform leftArmBarrel;

    [SerializeField]
    int ammoSecondary;
    [SerializeField]
    int ammoPrimary;

    #region WeaponStats
    GameObject bulletSecondary;
    float rateOfFireSecondary;
    float nextFireSecondary;
    int maxAmmoSecondary;
    float reloadTimeSecondary;
    float nextReloadSecondaryEnd;
    bool reloadingSecondary = false;
    [Range(0f, 100f)]
    float spraySecondary;
    float bulletSpeedSecondary;
    float bulletLifeSecondary;

    GameObject bulletPrimary;
    float rateOfFirePrimary;
    float nextFirePrimary;
    int maxAmmoPrimary;
    float reloadTimePrimary;
    float nextReloadPrimaryEnd;
    bool reloadingPrimary = false;
    [Range(0f, 100f)]
    float sprayPrimary;
    float bulletSpeedPrimary;
    float bulletLifePrimary;
    #endregion

    private void Start()
    {
        cockpit = Instantiate(cockpitItem.prefab, torsoRoot.transform.position, Quaternion.identity);
        cockpit.transform.parent = torsoRoot.transform;

        bulletPrimary= rightWeaponItem.bullet;
        rateOfFirePrimary = rightWeaponItem.rateOfFire;
        maxAmmoPrimary = rightWeaponItem.maxAmmo;
        reloadTimePrimary = rightWeaponItem.reloadTime;
        sprayPrimary = rightWeaponItem.bulletSpary;
        bulletSpeedPrimary = rightWeaponItem.bulletSpeed;
        bulletLifePrimary = rightWeaponItem.bulletLife;      

        bulletSecondary = leftWeaponItem.bullet;
        rateOfFireSecondary = leftWeaponItem.rateOfFire;
        maxAmmoSecondary = leftWeaponItem.maxAmmo;
        reloadTimeSecondary = leftWeaponItem.reloadTime;
        spraySecondary = leftWeaponItem.bulletSpary;
        bulletSpeedSecondary = leftWeaponItem.bulletSpeed;
        bulletLifeSecondary = leftWeaponItem.bulletLife;

        ammoSecondary = maxAmmoSecondary;
        ammoPrimary = maxAmmoPrimary;

        rightArmWeapon = Instantiate(rightWeaponItem.prefab, cockpitItem.rightArmConnection.position, Quaternion.identity) as GameObject;
        rightArmWeapon.transform.parent = torsoRoot.transform;

        leftArmWeapon = Instantiate(leftWeaponItem.prefab, cockpitItem.leftArmConnection.position, Quaternion.identity) as GameObject;
        leftArmWeapon.transform.parent = torsoRoot.transform;

        rightArmBarrel = rightArmWeapon.transform.Find("Barrel");
        leftArmBarrel = leftArmWeapon.transform.Find("Barrel");
    }

    private void Update()
    {
        #region Input

        // Get input direction
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetMouseButton(1))
        {
            FireSecondary(rateOfFireSecondary);
        }
        if (reloadingSecondary && Time.time > nextReloadSecondaryEnd)
        {
            ammoSecondary = maxAmmoSecondary;
            reloadingSecondary = false;
        }

        if (Input.GetMouseButton(0))
        {
            FirePrimary(rateOfFirePrimary);
        }
        if (reloadingPrimary && Time.time > nextReloadPrimaryEnd)
        {
            ammoPrimary = maxAmmoPrimary;
            reloadingPrimary = false;
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
        Vector3 eulerRotation = new Vector3(cam.transform.transform.eulerAngles.x - 5, cam.transform.eulerAngles.y, torsoRoot.transform.eulerAngles.z);
        torsoRoot.transform.rotation = Quaternion.Euler(eulerRotation);

        #endregion

        Vector3 forwardRight = torsoRoot.transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(rightArmBarrel.position, forwardRight, Color.red);

        Vector3 forwardLeft = torsoRoot.transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(leftArmBarrel.position, forwardLeft, Color.green);
    }

    private void FireSecondary(float RoFSecondary)
    {
        if (Time.time > nextFireSecondary && ammoSecondary > 0)
        {
            nextFireSecondary = Time.time + RoFSecondary;
            ammoSecondary--;

            // Random bullet spray
            float randX = Random.Range(-spraySecondary / 50, spraySecondary / 50);
            float randZ = Random.Range(-spraySecondary / 50, spraySecondary / 50);

            // Create the Bullet from the Bullet Prefab
            var bullet = (GameObject)Instantiate(bulletSecondary, leftArmBarrel.position, torsoRoot.transform.rotation);
            bullet.transform.rotation *= Quaternion.Euler(90 + randX, 0, randZ);

            // Add velocity to the bullet
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * bulletSpeedSecondary;

            // Destroy the bullet after _ seconds
            Destroy(bullet, bulletLifeSecondary);
        }
        if (ammoSecondary == 0 && !reloadingSecondary)
        {
            nextReloadSecondaryEnd = Time.time + reloadTimeSecondary;
            reloadingSecondary = true;
        }
    }

    private void FirePrimary(float RoFPrimary)
    {
        if (Time.time > nextFirePrimary && ammoPrimary > 0)
        {
            nextFirePrimary = Time.time + RoFPrimary;
            ammoPrimary--;

            // Random bullet spray
            float randX = Random.Range(-sprayPrimary / 50, sprayPrimary / 50);
            float randZ = Random.Range(-sprayPrimary / 50, sprayPrimary / 50);

            // Create the Bullet from the Bullet Prefab
            var bullet = (GameObject)Instantiate(bulletPrimary, rightArmBarrel.position, torsoRoot.transform.rotation);
            bullet.transform.rotation *= Quaternion.Euler(90 + randX, 0, randZ);

            // Add velocity to the bullet
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * bulletSpeedPrimary;
            Debug.Log(bullet.GetComponent<Rigidbody>().velocity);

            // Destroy the bullet after _ seconds
            Destroy(bullet, bulletLifePrimary);
        }
        if (ammoPrimary == 0 && !reloadingPrimary)
        {
            nextReloadPrimaryEnd = Time.time + reloadTimePrimary;
            reloadingPrimary = true;
        }
    }
}