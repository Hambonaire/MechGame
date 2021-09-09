using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponExecutable : MonoBehaviour{

    public bool fireTrigger;

    public WeaponItem weaponItemRef;
    public Weapon weapon;

    Animator animator;

    float nextFire;
    public float nextReloadStart;
    public float nextReloadEnd;
    public bool isFiring = false;
    public bool isReloading = false;

    public int maxAmmo;
    public int currentAmmo;
    float spread;
    float bulletSpeed;
    float bulletLife;

    //FireType fireType;
    float chargeTime;
    float chargeCurr;
    float beamTime;
    float beamCurr;
    float cooldown;             // TODO: Implement this?
    int projectileCount;

    //float trackDistance;        // TODO: Implement this? Make it common to weapon class instead of unique?
    float targetDistance;
    float firingAngle;

    /*
    public void Initialize(WeaponItem weapon, Transform[] barrel, Animator anim)
    {
        weaponType = weapon.weaponType;

        ammoIcon = weapon.ammoIcon;
        //fireType = weapon.fireMode;
        bulletPrefab = weapon.bullet;
        bulletSpeed = weapon.bulletSpeed;
        bulletLife = weapon.bulletLife;
        damage = weapon.damage;

        rateOfFire = weapon.secBetweenFire;

        reloadTime = weapon.reloadTime;
        maxAmmo = weapon.maxAmmo;
        spread = weapon.bulletSpread;
        //chargeTime = weapon.chargeTime;
        //beamTime = weapon.beamTime;
        //projectileCount = weapon.projectileCount;

        bulletSpawn = barrel;

        currentAmmo = maxAmmo;
        chargeCurr = 0;
        beamCurr = 0;

        animator = anim;
    }
    */

    void Update()
    {

        Animate();

    }

    void Start()
    {
        weapon = GetComponent<Weapon>();

        animator = weapon.wepAnim;

        maxAmmo = weaponItemRef.maxAmmo;
        currentAmmo = maxAmmo;

        spread = weaponItemRef.bulletSpread;
        bulletSpeed = weaponItemRef.bulletSpeed;
        bulletLife = weaponItemRef.bulletLife;
    }

    void Animate()
    {
        if (animator != null)
        {
            if (isFiring)
                animator.SetBool("firing", true);
            else
                animator.SetBool("firing", false);
        }

        isFiring = false;

    }

    public void Fire(Vector3 firingSolutionPoint = new Vector3(), GameObject targetobject = null)
    {
        if (weaponItemRef.weaponType == WeaponType.Ballistic)
        {
            FireBallistic(firingSolutionPoint);
        }
        else if (weaponItemRef.weaponType == WeaponType.Missile)
        {
            FireMissileVolley(targetobject);
        }

        /*
        bool ret = false;

        //if (needTarget && (controller.MyTarget == null || playerTargetDistance.value > targetDistance))// && playerTargetInRange.value)
        //    return;

        isFiring = true;
        
        if (!isReloading)
        {
            if (Time.time > nextFire && currentAmmo > 0 )
            {
				nextFire = Time.time + rateOfFire;
				currentAmmo--;

				// Random bullet spray
				float randX = Random.Range(-spread / 50, spread / 50);
				float randY = Random.Range(-spread / 50, spread / 50);
				float randZ = Random.Range(-spread / 50, spread / 50);

				var bullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

                // Auto targeting 
				if (false)//(autoTarget && controller.MyTarget != null)
				{
					//bullet.transform.LookAt(controller.MyTarget.transform);
					bullet.transform.rotation *= Quaternion.Euler(92 + randX, -.01f * targetDistance + randY, randZ);
					bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * bulletSpeed;
				}
				else
				{
					bullet.transform.rotation *= Quaternion.Euler(92 + randX, (-firingAngle * 0.7f) + randY, randZ);
					bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * bulletSpeed;
				}

                if (firingSolutionPoint != Vector3.zero)
                    bullet.transform.LookAt(firingSolutionPoint);

                bullet.transform.rotation *= Quaternion.Euler(randX, randY, randZ);

                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

                bullet.GetComponent<Bullet>().SetDamage(damage);

				GameObject.Destroy(bullet, bulletLife);
				
                else if (fireType == FireType.Charge)
                {
                    if (chargeCurr < chargeTime)
                    {
                        chargeCurr += Time.deltaTime;
                    }
                    else if (chargeCurr >= chargeTime)
                    {
                        nextFire = Time.time + rateOfFire;
                        currentAmmo--;

                        // Random bullet spray
                        float randX = Random.Range(-spread / 50, spread / 50);
                        float randY = Random.Range(-spread / 50, spread / 50);
                        float randZ = Random.Range(-spread / 50, spread / 50);

                        // Create the Bullet from the Bullet Prefab
                        var bullet = GameObject.Instantiate(bulletPrefab, barrel.position, barrel.rotation);

                        if (false)//(autoTarget && controller.MyTarget != null)
                        {
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, -.01f * targetDistance + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * bulletSpeed;
                        }
                        else
                        {
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, (-firingAngle * 0.7f) + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * bulletSpeed;
                        }

                        bullet.GetComponent<Bullet>().SetDamage(damage);

                        GameObject.Destroy(bullet, bulletLife);

                        chargeCurr = 0;
                    }
                }
                else if (fireType == FireType.Multi)
                {
                    nextFire = Time.time + rateOfFire;
                    currentAmmo--;

                    for (int i = 0; i < projectileCount; i++)
                    {
                        // Random bullet spray
                        float randX = Random.Range(-spread / 50, spread / 50);
                        float randY = Random.Range(-spread / 50, spread / 50);
                        float randZ = Random.Range(-spread / 50, spread / 50);

                        // Create the Bullet from the Bullet Prefab
                        var bullet = GameObject.Instantiate(bulletPrefab, barrel.position, barrel.rotation);

                        if (false)//(autoTarget && controller.MyTarget != null)
                        {
                            //bullet.transform.LookAt(controller.MyTarget.transform);
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, -.01f * targetDistance + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * bulletSpeed;
                        }
                        else
                        {
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, (-firingAngle * 0.7f) + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * bulletSpeed;
                        }

                        // Put stats on the bullet
                        bullet.GetComponent<Bullet>().SetDamage(ballisticDamage);
                        //bullet.GetComponent<Bullet>().SetEnergyDamage(energyDamage);
                        bullet.GetComponent<Bullet>().SetFaction(0);
                        
                        // Destroy the bullet after _ seconds
                        GameObject.Destroy(bullet, bulletLife);
                    }
                }
                else if (fireType == FireType.Beam)
                {
                    if (beamCurr >= beamTime)
                    {
                        beamCurr = Mathf.Clamp(beamCurr, 0, beamTime);
                        Reload();
                    }
                    else if (beamCurr < beamTime)
                    {
                        beamCurr += Time.deltaTime;

                        nextFire = Time.time + rateOfFire;
                        currentAmmo--;

                        // Random bullet spray
                        float randX = Random.Range(-spread / 50, spread / 50);
                        float randY = Random.Range(-spread / 50, spread / 50);
                        float randZ = Random.Range(-spread / 50, spread / 50);

                        // Create the Bullet from the Bullet Prefab
                        var bullet = GameObject.Instantiate(bulletPrefab, barrel.position, barrel.rotation);

                        if (false)//(autoTarget && controller.MyTarget != null)
                        {
                            //bullet.transform.LookAt(controller.MyTarget.transform);
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, -.01f * targetDistance + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * bulletSpeed;
                        }
                        else
                        {
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, (-firingAngle * 0.7f) + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * bulletSpeed;
                        }

                        // Put stats on the bullet
                        bullet.GetComponent<Bullet>().SetDamage(ballisticDamage);
                        //bullet.GetComponent<Bullet>().SetEnergyDamage(energyDamage);
                        bullet.GetComponent<Bullet>().SetFaction(0);
                        
                        // Destroy the bullet after _ seconds
                        GameObject.Destroy(bullet, bulletLife);
                    }
                }
            }
            
            if (animator != null)
                animator.SetBool("firing", true);

            if (currentAmmo == 0 && !isReloading)
            {
                isFiring = false;
                ret = Reload();
            }
        }
        else
        {
            isFiring = false;
            if (animator != null)
                animator.SetBool("firing", false);
        }

        return ret;
        */
    }

    public void FireBallistic(Vector3 firingSolutionPoint)
    {
        isFiring = true;

        //if (!isReloading)
        //{
            if (Time.time > nextFire && currentAmmo > 0)
            {
                nextFire = Time.time + weaponItemRef.secBetweenFire;
                currentAmmo--;

                /* Random bullet spray */
                float randX = Random.Range(-spread / 50, spread / 50);
                float randY = Random.Range(-spread / 50, spread / 50);
                float randZ = Random.Range(-spread / 50, spread / 50);

                var bullet = GameObject.Instantiate(weaponItemRef.bullet, weapon.bulSpwnLoc[0].position, weapon.bulSpwnLoc[0].rotation);

                if (firingSolutionPoint != Vector3.zero)
                    bullet.transform.LookAt(firingSolutionPoint);

                bullet.transform.rotation *= Quaternion.Euler(randX, randY, randZ);
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

                bullet.GetComponent<Bullet>().Initialize(weaponItemRef);
            }
            //else if (currentAmmo <= 0)
            //{
            //    Reload();
            //}

        //}

    }

    /* If this weapon is a  */
    public void FireMissileVolley(GameObject targetObject)
    {
        isFiring = true;

        if (Time.time > nextFire && currentAmmo > 0)
        {
            nextFire = Time.time + weaponItemRef.secBetweenFire;
            currentAmmo--;

            /* Random bullet spray */
            //float randX = Random.Range(-spread / 50, spread / 50);
            //float randY = Random.Range(-spread / 50, spread / 50);
            //float randZ = Random.Range(-spread / 50, spread / 50);

            //bullet.transform.rotation *= Quaternion.Euler(randX, randY, randZ);

            //TODO: if (targetObject != null) ;

            for (int locIndex = 0; locIndex < weapon.bulSpwnLoc.Length; locIndex++)
            {
                StartCoroutine(MissileSpawnDelay(weapon.bulSpwnLoc[locIndex], targetObject, 0.05f * locIndex));
            }

        }

    }

    /* 	
	 *	Returns a time to recall this function
	 *	Or maybe invoke repeating until a time
     */
    public void Reload()
    {
        isFiring = false;

        if (currentAmmo < maxAmmo && !isReloading)
        {
            isReloading = true;
			
            if (animator != null) 
				animator.SetBool("firing", false);
			
            nextReloadStart = Time.time;
            nextReloadEnd = Time.time + weaponItemRef.reloadTime;

            Invoke("Reload", weaponItemRef.reloadTime);
        }
        else if (isReloading && Time.time >= nextReloadEnd)
        {
            currentAmmo = maxAmmo;
            isReloading = false;
        }
    }

    public void OnCooldown()
    {
        isFiring = false;
        if (animator != null)
            animator.SetBool("firing", false);
    }

    IEnumerator MissileSpawnDelay(Transform location, GameObject target, float time)
    {
        yield return new WaitForSeconds(time);

        var bullet = Instantiate(weaponItemRef.bullet, location.position, location.rotation);

        bullet.GetComponent<Missile>().Initialize(weaponItemRef, target.transform);
    }

}
