using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    float gravity = -9.8f;

    int walkSpeed = 5;
    int runSpeed = 8;

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

    public NavMeshAgent navAgent;
    //public CharacterController characterController;

    public CapsuleCollider hitBox;
    float baseHitBoxRadius = 1f;
    float baseHitBoxHeight = 3f;

    public Transform targetTransform;
    public float torsoTrackSpeed = .8f;

    public GameObject torsoRoot;
    public GameObject legsRoot;

    //public float torsoScaleFactor = 0;
    //public float legsScaleFactor = 0;
    float overallScaleFactor = 1;
    //float weaponScaleFactor = 1;

    Animator legsAnimator;
    Animator rightWeaponAnimator;
    Animator leftWeaponAnimator;

    public Legs legsItem;
    public Cockpit cockpitItem;
    public Weapon rightWeaponItem;
    public Weapon leftWeaponItem;

    GameObject legs;
    GameObject rightArmWeapon;
    GameObject leftArmWeapon;
    GameObject cockpit;

    Transform legsBase;
    Transform cockpitRotationCenter;
    Transform torsoConnection;
    Transform rightArmBarrel;
    Transform leftArmBarrel;

    [SerializeField]
    int secondary_ammoCurr;
    [SerializeField]
    int primary_ammoCurr;

    #region WeaponStats
    GameObject secondary_bullet;
    float secondary_ROF;
    float secondary_nextFire;
    int secondary_maxAmmo;
    float secondary_reloadTime;
    float secondary_nextReloadEnd;
    bool secondary_isReloading = false;
    [Range(0f, 100f)]
    float secondary_Spread;
    float secondary_bulletSpeed;
    float secondary_bulletLife;

    GameObject primary_bullet;
    float primary_ROF;
    float primary_nextFire;
    int primary_maxAmmo;
    float primary_reloadTime;
    float primary_nextReloadEnd;
    bool primary_isReloading = false;
    [Range(0f, 100f)]
    float primary_spread;
    float primary_bulletSpeed;
    float primary_bulletLife;
    #endregion

    #region EnemySpecific
    [Range(0f, 100f)]
    public float primarySpreadIncrease;
    [Range(0f, 100f)]
    public float secondarySpreadIncrease;
    [Range(0f, 100f)]
    public float primaryDamageReduction;
    [Range(0f, 100f)]
    public float secondaryDamageReduction;
    #endregion

    bool firePrimary;
    bool fireSecondary;

    private void Start()
    {
        //navAgent = this.GetComponent<NavMeshAgent>();

        Initialize();

        navAgent.SetDestination(new Vector3(Random.Range(-45f, 45f), 0, Random.Range(-45f, 45f)));
    }

    private void Update()
    {
        if (Random.Range(0, 5) == 1)
        {
            firePrimary = true;
            fireSecondary = true;
        }
        else if (Random.Range(0, 5) == 0)
        {
            firePrimary = false;
            fireSecondary = false;
        }

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

        #region Firing
        if (firePrimary)
        {
            FirePrimary();
        }

        if (fireSecondary)
        {
            FireSecondary();
        }
        #endregion     
        
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
        legsAnimator.SetFloat("moveBlend", ((navAgent.isStopped) ? 0f : .7f), movementSmoothTime, Time.deltaTime);

        // Torso Turning (y only) for cockpit object itself
        //Vector3 eulerRotationTorso = new Vector3(torsoRoot.transform.eulerAngles.x, cam.transform.eulerAngles.y, torsoRoot.transform.eulerAngles.z);
        //torsoRoot.transform.rotation = Quaternion.Euler(eulerRotationTorso);

        // Torso turning towards a target
        Quaternion OriginalRot = torsoRoot.transform.rotation;

        torsoRoot.transform.LookAt(targetTransform);
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
        if (rightWeaponItem.prefab != null)
        {
            primary_bullet = rightWeaponItem.bullet;
            primary_ROF = rightWeaponItem.rateOfFire;
            primary_maxAmmo = rightWeaponItem.maxAmmo;
            primary_reloadTime = rightWeaponItem.reloadTime;
            primary_spread = rightWeaponItem.bulletSpread;
            primary_bulletSpeed = rightWeaponItem.bulletSpeed;
            primary_bulletLife = rightWeaponItem.bulletLife;

            rightArmWeapon = Instantiate(rightWeaponItem.prefab, cockpit.transform.Find("RightArmConnection").position, cockpit.transform.rotation) as GameObject;
            rightArmWeapon.transform.parent = cockpitRotationCenter.transform;
            // Scaling
           // rightArmWeapon.transform.localScale = new Vector3(torsoScaleFactor, torsoScaleFactor, torsoScaleFactor);

            rightArmBarrel = rightArmWeapon.transform.Find("Barrel");
        }

        if (leftWeaponItem.prefab != null)
        {
            secondary_bullet = leftWeaponItem.bullet;
            secondary_ROF = leftWeaponItem.rateOfFire;
            secondary_maxAmmo = leftWeaponItem.maxAmmo;
            secondary_reloadTime = leftWeaponItem.reloadTime;
            secondary_Spread = leftWeaponItem.bulletSpread;
            secondary_bulletSpeed = leftWeaponItem.bulletSpeed;
            secondary_bulletLife = leftWeaponItem.bulletLife;

            leftArmWeapon = Instantiate(leftWeaponItem.prefab, cockpit.transform.Find("LeftArmConnection").position, cockpit.transform.rotation) as GameObject;
            leftArmWeapon.transform.parent = cockpitRotationCenter.transform;
            // Scaling
            //leftArmWeapon.transform.localScale = new Vector3(torsoScaleFactor, torsoScaleFactor, torsoScaleFactor);

            leftArmBarrel = leftArmWeapon.transform.Find("Barrel");
        }

        secondary_ammoCurr = secondary_maxAmmo;
        primary_ammoCurr = primary_maxAmmo;
        #endregion

        #region Animators
        if (legs.transform.Find("AnimatorHolder") != null)
        {
            legsAnimator = legs.transform.Find("AnimatorHolder").GetComponent<Animator>();
        }

        if (rightArmWeapon.transform.Find("AnimatorHolder") != null)
        {
            rightWeaponAnimator = rightArmWeapon.transform.Find("AnimatorHolder").GetComponent<Animator>();
        }

        if (leftArmWeapon.transform.Find("AnimatorHolder") != null)
        {
            leftWeaponAnimator = leftArmWeapon.transform.Find("AnimatorHolder").GetComponent<Animator>();
        }
        #endregion

        #region Stats
        GetComponent<Stats>().AddBallisticArmor(cockpitItem.ballisticArmor + legsItem.ballisticArmor);
        GetComponent<Stats>().AddEnergyArmor(cockpitItem.energyArmor + legsItem.energyArmor);
        GetComponent<Stats>().AddHealthMax(cockpitItem.health + legsItem.health);
        #endregion

        #region Misc.
        hitBox.radius = baseHitBoxRadius * overallScaleFactor;
        hitBox.height = baseHitBoxHeight * overallScaleFactor;
        hitBox.center = new Vector3(0, hitBox.height / 2 + .05f, hitBox.center.z);
        #endregion

        cockpit.transform.localScale = new Vector3(overallScaleFactor, overallScaleFactor, overallScaleFactor);
    }

    private void FirePrimary()
    {
        if (!primary_isReloading)
        {
            if (Time.time > primary_nextFire && primary_ammoCurr > 0)
            {
                primary_nextFire = Time.time + primary_ROF;
                primary_ammoCurr--;

                // Random bullet spray
                float randX = Random.Range(-primary_spread / 50, primary_spread / 50) * ((primarySpreadIncrease / 100) + 1);
                float randY = Random.Range(-primary_spread / 50, primary_spread / 50) * ((primarySpreadIncrease / 100) + 1);
                float randZ = Random.Range(-primary_spread / 50, primary_spread / 50) * ((primarySpreadIncrease / 100) + 1);

                // Create the Bullet from the Bullet Prefab
                var bullet = (GameObject)Instantiate(primary_bullet, rightArmBarrel.position, rightArmBarrel.rotation);
                bullet.transform.rotation *= Quaternion.Euler(90 + randX, randY, randZ);

                // Put stats on the bullet
                bullet.GetComponent<Bullet>().SetBallisticDamage(rightWeaponItem.ballisticDamage * (1- (primaryDamageReduction / 100)));
                bullet.GetComponent<Bullet>().SetEnergyDamage(rightWeaponItem.energyDamage * (1 - (primaryDamageReduction / 100)));
                bullet.GetComponent<Bullet>().SetFaction(0);

                // Add velocity to the bullet
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * primary_bulletSpeed;

                // Destroy the bullet after _ seconds
                Destroy(bullet, primary_bulletLife);
            }
            if (primary_ammoCurr == 0 && !primary_isReloading)
            {
                primary_nextReloadEnd = Time.time + primary_reloadTime;
                primary_isReloading = true;
                rightWeaponAnimator.SetBool("firing", false);
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
        if (!secondary_isReloading)
        {
            if (Time.time > secondary_nextFire && secondary_ammoCurr > 0)
            {
                secondary_nextFire = Time.time + secondary_ROF;
                secondary_ammoCurr--;

                // Random bullet spray
                float randX = Random.Range(-secondary_Spread / 50, secondary_Spread / 50) * ((secondarySpreadIncrease / 100) + 1);
                float randY = Random.Range(-primary_spread / 50, primary_spread / 50) * ((secondarySpreadIncrease / 100) + 1);
                float randZ = Random.Range(-secondary_Spread / 50, secondary_Spread / 50) * ((secondarySpreadIncrease / 100) + 1);

                // Create the Bullet from the Bullet Prefab
                var bullet = (GameObject)Instantiate(secondary_bullet, leftArmBarrel.position, leftArmBarrel.rotation);
                bullet.transform.rotation *= Quaternion.Euler(90 + randX, randY, randZ);

                // Put stats on the bullet
                bullet.GetComponent<Bullet>().SetBallisticDamage(leftWeaponItem.ballisticDamage * (1 - (secondaryDamageReduction / 100)));
                bullet.GetComponent<Bullet>().SetEnergyDamage(leftWeaponItem.energyDamage * (1 - (secondaryDamageReduction / 100)));
                bullet.GetComponent<Bullet>().SetFaction(0);

                // Add velocity to the bullet
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * secondary_bulletSpeed;

                // Destroy the bullet after _ seconds
                Destroy(bullet, secondary_bulletLife);
            }
            if (secondary_ammoCurr == 0 && !secondary_isReloading)
            {
                secondary_nextReloadEnd = Time.time + secondary_reloadTime;
                secondary_isReloading = true;
                leftWeaponAnimator.SetBool("firing", false);
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

    public Vector3 getCenterMass()
    {
        return this.torsoRoot.transform.position;
    }
}
