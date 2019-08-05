using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Player : MonoBehaviour
{
    public Stats playerStats;

    public GameObject mechManager;

    public ScriptableFloat playerRightAmmoCurrent;
    public ScriptableFloat playerRightAmmoMax;
    public ScriptableFloat playerLeftAmmoCurrent;
    public ScriptableFloat playerLeftAmmoMax;
    public ScriptableFloat playerRightFillBar;
    public ScriptableFloat playerLeftFillBar;
    public ScriptableFloat playerHealthCurrent;
    public ScriptableFloat playerHealthMax;
    public ScriptableBool playerRightReloading;
    public ScriptableBool playerLeftReloading;
    public ScriptableGameObject playerTarget;

    float gravity = -9.8f;
    float walkSpeed;
    float runSpeed;

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

    private Vector3 inputDirection;

    private CapsuleCollider hitBox;
    private float baseHitBoxRadius = 1f;
    private float baseHitBoxHeight = 3f;
    private float baseHitBoxCenter = 1.55f;

    private Vector3 forward;

    private float firingAngle;

    public GameObject cam;
    public GameObject torsoRoot;
    public GameObject legsRoot;
    public CharacterController characterController;

    float overallScaleFactor = 1;

    Animator legsAnimator;
    Animator rightWeaponAnimator;
    Animator leftWeaponAnimator;

    public Legs legsItem;
    public Cockpit cockpitItem;
    public Weapon rightWeaponItem;
    public Weapon leftWeaponItem;

    public GameObject legs;
    public List<GameObject> rightArmWeapons;
    public List<GameObject> leftArmWeapons;
    public GameObject cockpit;

    public Transform legsBase;
    public Transform cockpitRotationCenter;
    public Transform torsoConnection;
    public Transform rightArmBarrel;
    public Transform leftArmBarrel;

    [SerializeField]
    int left_ammoCurr;
    [SerializeField]
    int right_ammoCurr;

    #region WeaponStats
    GameObject right_bullet;
    float right_ROF;
    float right_nextFire;
    int right_maxAmmo;
    float right_reloadTime;
    float right_nextReloadEnd;
    bool right_isReloading = false;
    [Range(0f, 100f)]
    float right_spread;
    float right_bulletSpeed;
    float right_bulletLife;
    FireType right_fireType;
    float right_chargeTime;
    float right_chargeCurr;
    float right_beamTime;
    float right_beamCurr;
    int right_projectileCount;
    bool right_autoTrack;

    GameObject left_bullet;
    float left_ROF;
    float left_nextFire;
    int left_maxAmmo;
    float left_reloadTime;
    float left_nextReloadEnd;
    bool left_isReloading = false;
    [Range(0f, 100f)]
    float left_Spread;
    float left_bulletSpeed;
    float left_bulletLife;
    FireType left_FireType;
    float left_chargeTime;
    float left_chargeCurr;
    float left_beamTime;
    float left_beamCurr;
    int left_projectileCount;
    bool left_autoTrack;
    #endregion

    public Vector3 enemyCenterMass;
    public float targetDistance;

    private void Start()
    {
        Initialize();

        playerStats = GetComponent<Stats>();
        //Variables.PlayerHealth_Max = playerStats.GetMaxHealth();
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

        //Handled by Stats script
        //Variables.PlayerHealth_Curr = playerStats.GetCurrentHealth();
        //playerHealthCurrent.value = playerStats.GetCurrentHealth();
    }

    public void GetInput()
    {
        // Get input direction
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!right_isReloading)
            {
                ReloadPrimary();
            }
            if (!left_isReloading)
            {
                ReloadSecondary();
            }
        }

        if (Input.GetMouseButton(0) && rightArmWeapon != null && !right_isReloading)
        {
            FirePrimary();
        }
        else
        {
            if (rightWeaponAnimator != null)
            {
                rightWeaponAnimator.SetBool("firing", false);
            }

            if (right_fireType == FireType.Charge)
            {
                right_chargeCurr -= Time.deltaTime;
                right_chargeCurr = Mathf.Clamp(right_chargeCurr, 0, Mathf.Infinity);
            }
            else if (right_fireType == FireType.Beam)
            {
                right_beamCurr -= Time.deltaTime;
                right_beamCurr = Mathf.Clamp(right_beamCurr, 0, right_beamTime);
            }
        }

        if (Input.GetMouseButton(1) && leftArmWeapon != null && !left_isReloading)
        {
            FireSecondary();
        }
        else
        {
            if (leftWeaponAnimator != null)
            {
                leftWeaponAnimator.SetBool("firing", false);
            }

            if (left_FireType == FireType.Charge)
            {
                left_chargeCurr -= Time.deltaTime;
                left_chargeCurr = Mathf.Clamp(left_chargeCurr, 0, Mathf.Infinity);
            }
            else if (right_fireType == FireType.Beam)
            {
                left_beamCurr -= Time.deltaTime;
                left_beamCurr = Mathf.Clamp(left_beamCurr, 0, left_beamTime);
            }
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
        Vector3 eulerRotationTorso = new Vector3(torsoRoot.transform.eulerAngles.x, cam.transform.eulerAngles.y, torsoRoot.transform.eulerAngles.z);
        torsoRoot.transform.rotation = Quaternion.Euler(eulerRotationTorso);

        // Arms turning (x only) for arms, connected to RotationAxis
        Vector3 eulerRotationArms = new Vector3(cam.transform.eulerAngles.x - 3, cockpitRotationCenter.eulerAngles.y, cockpitRotationCenter.eulerAngles.z);
        cockpitRotationCenter.rotation = Quaternion.Euler(eulerRotationArms);
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
        if (rightArmWeapon != null)
        {
            Destroy(rightArmWeapon);
        }
        if (leftArmWeapon != null)
        {
            Destroy(leftArmWeapon);
        }
        #endregion

        #region Part Variables
        legs = Instantiate(legsItem.prefab, legsRoot.transform.position, transform.rotation);
        legs.transform.parent = legsRoot.transform;
        // Scaling
        legs.transform.localScale = new Vector3(overallScaleFactor, overallScaleFactor, overallScaleFactor);

        torsoRoot.transform.position = legs.transform.Find("TorsoConnection").position;

        cockpit = Instantiate(cockpitItem.prefab, torsoRoot.transform.position, torsoRoot.transform.rotation);
        cockpit.transform.parent = torsoRoot.transform;
        // Scaling
        //cockpit.transform.localScale = new Vector3(torsoScaleFactor, torsoScaleFactor, torsoScaleFactor);

        cockpitRotationCenter = cockpit.transform.Find("RotationAxis");
        #endregion

        #region Weapon Variables
        if (rightWeaponItem.rightPrefab != null)
        {
            right_bullet = rightWeaponItem.bullet;
            right_ROF = rightWeaponItem.rateOfFire;
            right_maxAmmo = rightWeaponItem.maxAmmo;
            right_reloadTime = rightWeaponItem.reloadTime;
            right_spread = rightWeaponItem.bulletSpray;
            right_bulletSpeed = rightWeaponItem.bulletSpeed;
            right_bulletLife = rightWeaponItem.bulletLife;

            right_autoTrack = rightWeaponItem.autoTarget;

            right_fireType = rightWeaponItem.fireMode;

            if (right_fireType == FireType.Charge)
            {
                right_chargeTime = rightWeaponItem.chargeTimeIfChargeType;
            }
            else if (right_fireType == FireType.Multi)
            {
                right_projectileCount = rightWeaponItem.projectilesCountIfMultiType;
            }
            else if (right_fireType == FireType.Beam)
            {
                right_beamTime = rightWeaponItem.beamTimeIfBeamType;
            }
            else
            {
                right_beamTime = 0;
                left_chargeTime = 0;
            }

            rightArmWeapon = Instantiate(rightWeaponItem.rightPrefab, cockpit.transform.Find("RightArmConnection").position, cockpit.transform.rotation) as GameObject;
            rightArmWeapon.transform.parent = cockpitRotationCenter.transform;

            rightArmBarrel = rightArmWeapon.transform.Find("Barrel");
        }

        if (leftWeaponItem.leftPrefab != null)
        {
            left_bullet = leftWeaponItem.bullet;
            left_ROF = leftWeaponItem.rateOfFire;
            left_maxAmmo = leftWeaponItem.maxAmmo;
            left_reloadTime = leftWeaponItem.reloadTime;
            left_Spread = leftWeaponItem.bulletSpray;
            left_bulletSpeed = leftWeaponItem.bulletSpeed;
            left_bulletLife = leftWeaponItem.bulletLife;

            left_FireType = leftWeaponItem.fireMode;

            left_autoTrack = leftWeaponItem.autoTarget;

            if (left_FireType == FireType.Charge)
            {
                left_chargeTime = leftWeaponItem.chargeTimeIfChargeType;
            }
            else if (left_FireType == FireType.Multi)
            {
                left_projectileCount = leftWeaponItem.projectilesCountIfMultiType;
            }
            else if (left_FireType == FireType.Beam)
            {
                left_beamTime = leftWeaponItem.beamTimeIfBeamType;
            }
            else
            {
                left_beamTime = 0;
                left_chargeTime = 0;
            }

            leftArmWeapon = Instantiate(leftWeaponItem.leftPrefab, cockpit.transform.Find("LeftArmConnection").position, cockpit.transform.rotation) as GameObject;
            leftArmWeapon.transform.parent = cockpitRotationCenter.transform;

            leftArmBarrel = leftArmWeapon.transform.Find("Barrel");
        }

        //Variables.PlayerPrimaryAmmo_Max = right_maxAmmo;
        //Variables.PlayerSecondaryAmmo_Max = left_maxAmmo;      
        //Variables.PlayerSecondaryAmmo_Curr = Variables.PlayerSecondaryAmmo_Max;
        //Variables.PlayerPrimaryAmmo_Curr = Variables.PlayerPrimaryAmmo_Max;

        playerRightAmmoCurrent.value = right_ammoCurr;
        playerRightAmmoMax.value = right_maxAmmo;
        playerLeftAmmoCurrent.value = left_ammoCurr;
        playerLeftAmmoMax.value = left_maxAmmo;

        //Variables.PrimaryReloading = false;
        //Variables.SecondaryReloading = false;

        playerRightReloading.value = false;
        playerLeftReloading.value = false;

        right_isReloading = false;
        left_isReloading = false;
        #endregion

        #region Animators
        if (legs.transform.Find("AnimatorHolder") != null)
        {
            legsAnimator = legs.transform.Find("AnimatorHolder").GetComponent<Animator>();
        }

        if (rightArmWeapon.transform.Find("AnimatorHolder") != null && rightArmWeapon != null)
        {
            rightWeaponAnimator = rightArmWeapon.transform.Find("AnimatorHolder").GetComponent<Animator>();
        }

        if (leftArmWeapon.transform.Find("AnimatorHolder") != null && leftArmWeapon != null)
        {
            leftWeaponAnimator = leftArmWeapon.transform.Find("AnimatorHolder").GetComponent<Animator>();
        }
        #endregion

        /// NOW HANDLED IN EQUIPMENT MANAGER
        #region Stats
        //playerStats.SetBallisticArmor(cockpitItem.ballisticArmor + legsItem.ballisticArmor);
        //playerStats.SetEnergyArmor(cockpitItem.energyArmor + legsItem.energyArmor);
        //playerStats.SetHealthMax(cockpitItem.health + legsItem.health);

        //Variables.PlayerHealth_Max = playerStats.GetMaxHealth();
        //Variables.PlayerHealth_Curr = playerStats.GetCurrentHealth();
        //Variables.PlayerBallisticArmor = playerStats.GetBallisticArmor();
        //Variables.PlayerEnergyArmor = playerStats.GetEnergyArmor();

        //playerHealthMax.value = playerStats.GetMaxHealth();
        //playerHealthCurrent.value = playerStats.GetCurrentHealth();
        #endregion

        #region Misc.
        walkSpeed = legsItem.walkSpeed * overallScaleFactor;
        runSpeed = legsItem.runSpeed * overallScaleFactor;
        hitBox.radius = baseHitBoxRadius * overallScaleFactor;
        hitBox.height = baseHitBoxHeight * overallScaleFactor;
        hitBox.center = new Vector3(0, hitBox.height / 2 + .05f, hitBox.center.z);
        #endregion

        cockpit.transform.localScale = new Vector3(overallScaleFactor, overallScaleFactor, overallScaleFactor);
    }

    private void FirePrimary()
    {
        if (!playerRightReloading.value)
        {
            if (Time.time > right_nextFire && playerRightAmmoCurrent.value > 0)
            {
                if (right_fireType == FireType.Regular)
                {
                    right_nextFire = Time.time + right_ROF;
                    playerRightAmmoCurrent.value--;

                    // Random bullet spray
                    float randX = Random.Range(-right_spread / 50, right_spread / 50);
                    float randY = Random.Range(-right_spread / 50, right_spread / 50);
                    float randZ = Random.Range(-right_spread / 50, right_spread / 50);

                    // Create the Bullet from the Bullet Prefab
                    var bullet = (GameObject)Instantiate(right_bullet, rightArmBarrel.position, rightArmBarrel.rotation);

                    if (playerTarget.value != null && right_autoTrack)
                    {
                        bullet.transform.LookAt(playerTarget.value.transform);
                        bullet.transform.rotation *= Quaternion.Euler(92 + randX, -.01f * targetDistance + randY, randZ);
                        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * right_bulletSpeed;
                    }
                    else
                    {
                        bullet.transform.rotation *= Quaternion.Euler(92 + randX, (-firingAngle * 0.7f) + randY, randZ);
                        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * right_bulletSpeed;
                    }

                    // Put stats on the bullet
                    bullet.GetComponent<Bullet>().SetBallisticDamage(rightWeaponItem.ballisticDamage);
                    bullet.GetComponent<Bullet>().SetEnergyDamage(rightWeaponItem.energyDamage);
                    bullet.GetComponent<Bullet>().SetFaction(0);

                    // Add velocity to the bullet
                    //bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * right_bulletSpeed;

                    // Destroy the bullet after _ seconds
                    Destroy(bullet, right_bulletLife);
                }
                else if (right_fireType == FireType.Charge)
                {
                    if (right_chargeCurr < right_chargeTime)
                    {
                        right_chargeCurr += Time.deltaTime;

                        //Debug.Log("Primary Charging: " + right_chargeCurr + " out of: " + right_chargeTime);
                    }
                    else if (right_chargeCurr >= right_chargeTime)
                    {
                        right_nextFire = Time.time + right_ROF;
                        playerRightAmmoCurrent.value--;

                        // Random bullet spray
                        float randX = Random.Range(-right_spread / 50, right_spread / 50);
                        float randY = Random.Range(-right_spread / 50, right_spread / 50);
                        float randZ = Random.Range(-right_spread / 50, right_spread / 50);

                        // Create the Bullet from the Bullet Prefab
                        var bullet = (GameObject)Instantiate(right_bullet, rightArmBarrel.position, rightArmBarrel.rotation);

                        if (playerTarget.value != null && right_autoTrack)
                        {
                            bullet.transform.LookAt(playerTarget.value.transform);
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, -.01f * targetDistance + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * right_bulletSpeed;
                        }
                        else
                        {
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, (-firingAngle * 0.7f) + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * right_bulletSpeed;
                        }

                        // Put stats on the bullet
                        bullet.GetComponent<Bullet>().SetBallisticDamage(rightWeaponItem.ballisticDamage);
                        bullet.GetComponent<Bullet>().SetEnergyDamage(rightWeaponItem.energyDamage);
                        bullet.GetComponent<Bullet>().SetFaction(0);

                        // Add velocity to the bullet
                        //bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * right_bulletSpeed;

                        // Destroy the bullet after _ seconds
                        Destroy(bullet, right_bulletLife);

                        right_chargeCurr = 0;
                    }
                }
                else if (right_fireType == FireType.Multi)
                {
                    right_nextFire = Time.time + right_ROF;
                    playerRightAmmoCurrent.value--;

                    for (int i = 0; i < right_projectileCount; i++)
                    {
                        // Random bullet spray
                        float randX = Random.Range(-right_spread / 50, right_spread / 50);
                        float randY = Random.Range(-right_spread / 50, right_spread / 50);
                        float randZ = Random.Range(-right_spread / 50, right_spread / 50);

                        // Create the Bullet from the Bullet Prefab
                        var bullet = (GameObject)Instantiate(right_bullet, rightArmBarrel.position, rightArmBarrel.rotation);

                        if (playerTarget.value != null && right_autoTrack)
                        {
                            bullet.transform.LookAt(playerTarget.value.transform);
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, -.01f * targetDistance + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * right_bulletSpeed;
                        }
                        else
                        {
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, (-firingAngle * 0.7f) + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * right_bulletSpeed;
                        }

                        // Put stats on the bullet
                        bullet.GetComponent<Bullet>().SetBallisticDamage(rightWeaponItem.ballisticDamage);
                        bullet.GetComponent<Bullet>().SetEnergyDamage(rightWeaponItem.energyDamage);
                        bullet.GetComponent<Bullet>().SetFaction(0);

                        // Add velocity to the bullet
                        //bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * right_bulletSpeed;

                        // Destroy the bullet after _ seconds
                        Destroy(bullet, right_bulletLife);
                    }
                }
                else if (right_fireType == FireType.Beam)
                {
                    if (right_beamCurr >= right_beamTime)
                    {
                        right_beamCurr = Mathf.Clamp(right_beamCurr, 0, right_beamTime);
                        ReloadPrimary();
                    }
                    else if (right_beamCurr < right_beamTime)
                    {
                        right_beamCurr += Time.deltaTime;

                        right_nextFire = Time.time + right_ROF;
                        playerRightAmmoCurrent.value--;

                        // Random bullet spray
                        float randX = Random.Range(-right_spread / 50, right_spread / 50);
                        float randY = Random.Range(-right_spread / 50, right_spread / 50);
                        float randZ = Random.Range(-right_spread / 50, right_spread / 50);

                        // Create the Bullet from the Bullet Prefab
                        var bullet = (GameObject)Instantiate(right_bullet, rightArmBarrel.position, rightArmBarrel.rotation);

                        if (playerTarget.value != null && right_autoTrack)
                        {
                            bullet.transform.LookAt(playerTarget.value.transform);
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, -.01f * targetDistance + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * right_bulletSpeed;
                        }
                        else
                        {
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, (-firingAngle * 0.7f) + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * right_bulletSpeed;
                        }

                        // Put stats on the bullet
                        bullet.GetComponent<Bullet>().SetBallisticDamage(rightWeaponItem.ballisticDamage);
                        bullet.GetComponent<Bullet>().SetEnergyDamage(rightWeaponItem.energyDamage);
                        bullet.GetComponent<Bullet>().SetFaction(0);

                        // Add velocity to the bullet
                        //bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * right_bulletSpeed;

                        // Destroy the bullet after _ seconds
                        Destroy(bullet, right_bulletLife);
                    }
                }
            }
            else if (playerRightAmmoCurrent.value == 0 && !playerRightReloading.value && !right_isReloading)
            {
                ReloadPrimary();
            }
            if (rightWeaponAnimator != null)
            {
                rightWeaponAnimator.SetBool("firing", true);
            }
        }
        else
        {
            if (rightWeaponAnimator != null)
            {
                rightWeaponAnimator.SetBool("firing", false);
            }
        }
    }

    private void FireSecondary()
    {
        if (!playerLeftReloading.value)
        {
            if (Time.time > left_nextFire && playerLeftAmmoCurrent.value > 0)
            {
                if (left_FireType == FireType.Regular)
                {
                    left_nextFire = Time.time + left_ROF;
                    playerLeftAmmoCurrent.value--;

                    // Random bullet spray
                    float randX = Random.Range(-left_Spread / 50, left_Spread / 50);
                    float randY = Random.Range(-right_spread / 50, right_spread / 50);
                    float randZ = Random.Range(-left_Spread / 50, left_Spread / 50);

                    // Create the Bullet from the Bullet Prefab
                    var bullet = (GameObject)Instantiate(left_bullet, leftArmBarrel.position, leftArmBarrel.rotation);

                    if (playerTarget.value != null && left_autoTrack)
                    {
                        bullet.transform.LookAt(playerTarget.value.transform);
                        bullet.transform.rotation *= Quaternion.Euler(92 + randX, .01f * targetDistance + randY, randZ);
                        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * left_bulletSpeed;
                    }
                    else
                    {
                        bullet.transform.rotation *= Quaternion.Euler(92 + randX, (firingAngle * .7f) + randY, randZ);
                        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * left_bulletSpeed;
                    }

                    // Put stats on the bullet
                    bullet.GetComponent<Bullet>().SetBallisticDamage(leftWeaponItem.ballisticDamage);
                    bullet.GetComponent<Bullet>().SetEnergyDamage(leftWeaponItem.energyDamage);
                    bullet.GetComponent<Bullet>().SetFaction(0);

                    // Add velocity to the bullet
                    //bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * left_bulletSpeed;

                    // Destroy the bullet after _ seconds
                    Destroy(bullet, left_bulletLife);

                    left_chargeCurr = 0;
                }
                else if (left_FireType == FireType.Charge)
                {
                    if (left_chargeCurr < left_chargeTime)
                    {
                        left_chargeCurr += Time.deltaTime;
                    }
                    else if (left_chargeCurr >= left_chargeTime)
                    {
                        left_nextFire = Time.time + left_ROF;
                        playerLeftAmmoCurrent.value--;

                        // Random bullet spray
                        float randX = Random.Range(-left_Spread / 50, left_Spread / 50);
                        float randY = Random.Range(-left_Spread / 50, left_Spread / 50);
                        float randZ = Random.Range(-left_Spread / 50, left_Spread / 50);

                        // Create the Bullet from the Bullet Prefab
                        var bullet = (GameObject)Instantiate(left_bullet, leftArmBarrel.position, leftArmBarrel.rotation);

                        if (playerTarget.value != null && left_autoTrack)
                        {
                            bullet.transform.LookAt(playerTarget.value.transform);
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, .01f * targetDistance + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * left_bulletSpeed;
                        }
                        else
                        {
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, (firingAngle * .7f) + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * left_bulletSpeed;
                        }

                        // Put stats on the bullet
                        bullet.GetComponent<Bullet>().SetBallisticDamage(leftWeaponItem.ballisticDamage);
                        bullet.GetComponent<Bullet>().SetEnergyDamage(leftWeaponItem.energyDamage);
                        bullet.GetComponent<Bullet>().SetFaction(0);

                        // Add velocity to the bullet
                        //bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * left_bulletSpeed;

                        // Destroy the bullet after _ seconds
                        Destroy(bullet, left_bulletLife);

                        left_chargeCurr = 0;
                    }

                }
                else if (left_FireType == FireType.Multi)
                {
                    left_nextFire = Time.time + left_ROF;
                    playerLeftAmmoCurrent.value--;

                    for (int i = 0; i < right_projectileCount; i++)
                    {
                        // Random bullet spray
                        float randX = Random.Range(-left_Spread / 50, left_Spread / 50);
                        float randY = Random.Range(-left_Spread / 50, left_Spread / 50);
                        float randZ = Random.Range(-left_Spread / 50, left_Spread / 50);

                        // Create the Bullet from the Bullet Prefab
                        var bullet = (GameObject)Instantiate(left_bullet, leftArmBarrel.position, leftArmBarrel.rotation);

                        if (playerTarget.value != null && left_autoTrack)
                        {
                            bullet.transform.LookAt(playerTarget.value.transform);
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, .01f * targetDistance + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * left_bulletSpeed;
                        }
                        else
                        {
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, (firingAngle * .7f) + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * left_bulletSpeed;
                        }

                        // Put stats on the bullet
                        bullet.GetComponent<Bullet>().SetBallisticDamage(leftWeaponItem.ballisticDamage);
                        bullet.GetComponent<Bullet>().SetEnergyDamage(leftWeaponItem.energyDamage);
                        bullet.GetComponent<Bullet>().SetFaction(0);

                        // Add velocity to the bullet
                        //bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * left_bulletSpeed;

                        // Destroy the bullet after _ seconds
                        Destroy(bullet, left_bulletLife);
                    }
                }
                else if (left_FireType == FireType.Beam)
                {
                    if (left_beamCurr >= left_beamTime)
                    {
                        left_beamCurr = Mathf.Clamp(left_beamCurr, 0, left_beamTime);
                        ReloadSecondary();
                    }
                    else if (left_beamCurr < left_beamTime)
                    {
                        left_beamCurr += Time.deltaTime;

                        left_nextFire = Time.time + left_ROF;
                        playerLeftAmmoCurrent.value--;

                        // Random bullet spray
                        float randX = Random.Range(-left_Spread / 50, left_Spread / 50);
                        float randY = Random.Range(-left_Spread / 50, left_Spread / 50);
                        float randZ = Random.Range(-left_Spread / 50, left_Spread / 50);

                        // Create the Bullet from the Bullet Prefab
                        var bullet = (GameObject)Instantiate(left_bullet, leftArmBarrel.position, leftArmBarrel.rotation);

                        if (playerTarget.value != null && left_autoTrack)
                        {
                            bullet.transform.LookAt(playerTarget.value.transform);
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, .01f * targetDistance + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * left_bulletSpeed;
                        }
                        else
                        {
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, (firingAngle * .7f) + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * left_bulletSpeed;
                        }

                        // Put stats on the bullet
                        bullet.GetComponent<Bullet>().SetBallisticDamage(leftWeaponItem.ballisticDamage);
                        bullet.GetComponent<Bullet>().SetEnergyDamage(leftWeaponItem.energyDamage);
                        bullet.GetComponent<Bullet>().SetFaction(0);

                        // Add velocity to the bullet
                        //bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * left_bulletSpeed;

                        // Destroy the bullet after _ seconds
                        Destroy(bullet, left_bulletLife);

                        left_chargeCurr = 0;
                    }
                }
            }
            else if (playerLeftAmmoCurrent.value == 0 && !playerLeftReloading.value && !left_isReloading)
            {
                ReloadSecondary();
            }
            if (leftWeaponAnimator != null)
            {
                leftWeaponAnimator.SetBool("firing", true);
            }
        }
        else
        {
            if (leftWeaponAnimator != null)
            {
                leftWeaponAnimator.SetBool("firing", false);
            }
        }
    }

    private void ReloadPrimary()
    {
        //Debug.Log("ATTEMPTING RELOAD");
        if (playerRightAmmoCurrent.value < playerRightAmmoMax.value && !playerRightReloading.value)
        {
            playerRightReloading.value = true;
            right_isReloading = true;
            right_nextReloadEnd = Time.time + right_reloadTime;
            if (rightWeaponAnimator != null) rightWeaponAnimator.SetBool("firing", false);
            Invoke("ReloadPrimary", right_reloadTime);
            //Debug.Log("Reload end at " + right_nextReloadEnd);
        }
        else if (playerRightReloading.value || right_isReloading)
        {
            //Debug.Log("Reload done at " + Time.time);
            playerRightAmmoCurrent.value = playerRightAmmoMax.value;
            right_isReloading = false;
            playerRightReloading.value = false;
        }
    }

    private void ReloadSecondary()
    {
        if (playerLeftAmmoCurrent.value < playerLeftAmmoMax.value && !playerLeftReloading.value)
        {
            playerLeftReloading.value = true;
            left_isReloading = true;
            left_nextReloadEnd = Time.time + left_reloadTime;
            if (leftWeaponAnimator != null) leftWeaponAnimator.SetBool("firing", false);
            Invoke("ReloadSecondary", left_reloadTime);
            //Debug.Log("Reload end at " + left_nextReloadEnd);
        }
        else if (playerLeftReloading.value || left_isReloading)
        {
            //Debug.Log("Reload done at " + Time.time);
            playerLeftAmmoCurrent.value = playerLeftAmmoMax.value;
            left_isReloading = false;
            playerLeftReloading.value = false;
        }
    }

    public void SetNewWeapons(Weapon newLeft, Weapon newRight)
    {
        this.leftWeaponItem = newLeft;
        this.rightWeaponItem = newRight;

        Initialize();
    }

    public void SetNewCockpit(Cockpit newCockpit)
    {
        this.cockpitItem = newCockpit;

        Initialize();
    }

    public void SetNewLegs(Legs newLegs)
    {
        this.legsItem = newLegs;

        Initialize();
    }

    public float GetScaleFactor()
    {
        return overallScaleFactor;
    }

}
