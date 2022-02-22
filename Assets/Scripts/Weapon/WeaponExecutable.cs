using System.Collections;
using UnityEngine;

public class WeaponExecutable : MonoBehaviour{

    [HideInInspector]
    public MechManager mechManager;

    [HideInInspector]
    public WeaponSystem refSystem;

    [HideInInspector]
    public Section sectionParent;

    [Header("General")]
    public WeaponItem weaponItemRef;
    public Transform[] bulSpwnLoc;
    int spawnLocIndex = 0;
    FireType refFireType;
    GameObject bullet;

    [Header("Animation")]
    public Animator animator;
    [SerializeField]
    bool animFireTrigger;
    [SerializeField]
    bool animFireResetting;
    bool needsAnimationEvent;
    AnimationTriggers animTriggers;
    bool playFiring;
    bool animationCooldown;

    // Firing Variables
    float damage;
    float reloadTime;
    float secBetweenFire = 0;
    float lastFire = 0;
    [SerializeField]
    float nextFire = 0;
    float nextReloadStart;
    float nextReloadEnd;

    bool fireRequested = false;
    bool isFiring = false;
    bool isReloading = false;

    [HideInInspector]
    public int ammoCurr;
    int ammoMax;

    float bulletSpread;
    public float bloomCurr = 0;
    float bloomInc;
    float bloomDec;

    float volleyCount;
    float volleyDelay;

    bool fired = false;
    // Firing Variables

    //public int weaponGroup = -1;

    // Other Weapon Ref Variables


    /*
    public void Initialize(WeaponItem weapon, Transform[] barrel, Animator anim)
    {
        AmmoType = weapon.AmmoType;

        ammoIcon = weapon.ammoIcon;
        //fireType = weapon.fireMode;
        bulletPrefab = weapon.bullet;
        bulletSpeed = weapon.bulletSpeed;
        bulletLife = weapon.bulletLife;
        damage = weapon.damage;

        rateOfFire = weapon.secBetweenFire;

        reloadTime = weapon.reloadTime;
        ammoMax = weapon.ammoMax;
        spread = weapon.bulletSpread;
        //chargeTime = weapon.chargeTime;
        //beamTime = weapon.beamTime;
        //projectileCount = weapon.projectileCount;

        bulletSpawn = barrel;

        ammoCurr = ammoMax;
        chargeCurr = 0;
        beamCurr = 0;

        animator = anim;
    }
    */

    void Update()
    {

        Animate();

        Bloom();

    }

    void LateUpdate()
    {
        //print(animFireResetting);

        if (needsAnimationEvent && animFireResetting)
            playFiring = false;
        else
            playFiring = fireRequested;

        if (fired) animFireTrigger = false;

        fireRequested = false;
        fired = false;
    }

    void Start()
    {
        bullet = weaponItemRef.bullet;
        refFireType = weaponItemRef.fireType;

        damage = weaponItemRef.damage;

        ammoMax = weaponItemRef.maxAmmo;
        ammoCurr = ammoMax;

        secBetweenFire = weaponItemRef.secBetweenFire;

        reloadTime = weaponItemRef.reloadTime;

        volleyCount = weaponItemRef.volleyCount;
        volleyDelay = weaponItemRef.volleyDelay;

        bulletSpread = weaponItemRef.bulletSpread;
        bloomInc = weaponItemRef.bloomIncrement;
        bloomDec = weaponItemRef.bloomDecrement;

        needsAnimationEvent = weaponItemRef.needsAnimationEvent;
        if (needsAnimationEvent)
        {
            animTriggers = GetComponentInChildren<AnimationTriggers>();
            animTriggers.FireTrigger += AnimFireTrigger;
            animTriggers.FireReset += AnimFireReset;
        }

        if (sectionParent == null)
            sectionParent = GetComponentInParent<Section>();
        if (mechManager == null)
            mechManager = transform.root.GetComponent<MechManager>();

        bloomCurr = 0;

        //weaponGroup = (int)weaponItemRef.AmmoType;
    }

    // TODO
    void Animate()
    {
        if (needsAnimationEvent && Time.time >= nextFire)
            animationCooldown = false;

        if (animator != null)
        {
            if (needsAnimationEvent)
            {
                if (animationCooldown)
                    animator.SetBool("firing", false);
                else
                    animator.SetBool("firing", playFiring);
            }
            else
                animator.SetBool("firing", playFiring);
        }
    }

    public void Fire(Transform targetTransform = null, Vector3 firingSolutionPoint = new Vector3())
    {
        if (!gameObject.activeInHierarchy || ammoCurr <= 0)
            return;

        fireRequested = true;

        if (refFireType == FireType.Regular)
        {
            if (needsAnimationEvent && !animFireTrigger)
                return;
            if (Time.time < nextFire)
                return;

            fired = FireRegular(targetTransform, firingSolutionPoint);
        }
        else if (refFireType == FireType.Volley)
        {
            fired = FireVolley(targetTransform, firingSolutionPoint);
        }
    }

    public void Bloom()
    {
        if (!fired && Time.time >= lastFire + 0.2f) // TODO
        {
            bloomCurr -= bloomDec * Time.deltaTime;
        }
        bloomCurr = Mathf.Clamp(bloomCurr, 1, 4);
    }

    public bool FireRegular(Transform target, Vector3 firingSolution)
    {
        if (!needsAnimationEvent)
            nextFire = Time.time + secBetweenFire;
        else
            nextFire = Time.time + 20f;

        ammoCurr--;

        var bulletObj = Instantiate(bullet, bulSpwnLoc[0].position, bulSpwnLoc[spawnLocIndex].rotation);

        bulletObj.GetComponent<Bullet>().Initialize(mechManager, weaponItemRef, target, firingSolution, BulletSpread(bulletSpread));

        // Bloom
        bloomCurr += bloomInc;
        lastFire = Time.time;

        refSystem.OnSuccessFire(damage);

        spawnLocIndex++;
        if (spawnLocIndex >= bulSpwnLoc.Length)
            spawnLocIndex = 0;

        return true;
    }

    public bool FireVolley(Transform target, Vector3 firingSolution)
    {
        if (!needsAnimationEvent)
            nextFire = Time.time + secBetweenFire;
        else
            nextFire = Time.time + 20f;

        int locIndex = 0;
        for (int vCnt = 0; vCnt < volleyCount && ammoCurr > 0; vCnt++)
        {
            ammoCurr--;

            if (locIndex >= bulSpwnLoc.Length) locIndex = 0;

            StartCoroutine(VolleySpawnDelay(bulSpwnLoc[locIndex], target, firingSolution, volleyDelay * vCnt));
                
            locIndex++;
        }

        return true;
    }

    /* 	
	 *	Returns a time to recall this function
	 *	Or maybe invoke repeating until a time
     */
    public void Reload()
    {
        if (ammoCurr < ammoMax && !isReloading)
        {
            isReloading = true;
			
            if (animator != null) 
				animator.SetBool("firing", false);
			
            nextReloadStart = Time.time;
            nextReloadEnd = Time.time + reloadTime;

            Invoke("Reload", reloadTime);
        }
        else if (isReloading && Time.time >= nextReloadEnd)
        {
            ammoCurr = ammoMax;
            isReloading = false;
        }
    }

    public void OnCooldown()
    {
        if (animator != null)
            animator.SetBool("firing", false);
    }

    public Vector3 BulletSpread(float weaponSpread)
    {
        /* Random bullet spray */
        float randX = Random.Range(-weaponSpread / 20, weaponSpread / 20);
        float randY = Random.Range(-weaponSpread / 20, weaponSpread / 20);
        float randZ = Random.Range(-weaponSpread / 20, weaponSpread / 20);

        return new Vector3(randX, randY, randZ) * bloomCurr;
    }

    IEnumerator VolleySpawnDelay(Transform location, Transform target, Vector3 firingSolution, float time)
    {
        yield return new WaitForSeconds(time);

        var bulletObj = Instantiate(bullet, location.position, location.rotation);

        bulletObj.GetComponent<Bullet>().Initialize(mechManager, weaponItemRef, target, firingSolution, BulletSpread(bulletSpread));

        bloomCurr += bloomInc;
        lastFire = Time.time;

        refSystem.OnSuccessFire(damage);
    }

    public void AnimFireTrigger()
    {
        animFireTrigger = true;
        animFireResetting = true;
    }

    public void AnimFireReset()
    {
        print("Reset");
        animFireResetting = false;
        animationCooldown = true;
        nextFire = Time.time + secBetweenFire;
    }

}
