using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WeaponExecutable {

    public WeaponItem weaponItemRef;
    public WeaponType weaponType;

    public GameObject weaponObj;

    //public GameObject targetObject;

    public Image ammoIcon;

    public Transform[] bulletSpawn;
    public GameObject bulletPrefab;


    //public MechController controller;

    bool autoTarget;
    bool needTarget;

    Animator animator;

    float damage;

    float rateOfFire;
    float nextFire;
    public float reloadTime;
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

    public WeaponExecutable(GameObject obj, WeaponItem weapon, Transform[] barrel, Animator anim)
    {
        weaponObj = obj;

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

    public int Fire(Vector3 firingSolutionPoint = new Vector3(), GameObject targetobject = null)
    {
        if (weaponObj == null || weaponObj.activeSelf == false)
            return 2;

        if (weaponType == WeaponType.Ballistic)
        {
            return FireBallistic(firingSolutionPoint);
        }
        else if (weaponType == WeaponType.Missile)
        {
            return FireMissileVolley(targetobject);
        }

        return 2;

        /**
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

    public int FireBallistic(Vector3 firingSolutionPoint)
    {
        int ret = 0;

        isFiring = true;

        if (!isReloading)
        {
            if (Time.time > nextFire && currentAmmo > 0)
            {
                nextFire = Time.time + rateOfFire;
                currentAmmo--;

                /* Random bullet spray */
                float randX = Random.Range(-spread / 50, spread / 50);
                float randY = Random.Range(-spread / 50, spread / 50);
                float randZ = Random.Range(-spread / 50, spread / 50);

                var bullet = GameObject.Instantiate(bulletPrefab, bulletSpawn[0].position, bulletSpawn[0].rotation);

                if (firingSolutionPoint != Vector3.zero)
                    bullet.transform.LookAt(firingSolutionPoint);

                bullet.transform.rotation *= Quaternion.Euler(randX, randY, randZ);

                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

                bullet.GetComponent<Bullet>().SetDamage(damage);

                GameObject.Destroy(bullet, bulletLife);
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
    }

    /* If this weapon is a  */
    public int FireMissileVolley(GameObject targetObject)
    {
        int ret = 0;

        isFiring = true;

        if (!isReloading)
        {
            if (Time.time > nextFire && currentAmmo > 0)
            {
                nextFire = Time.time + rateOfFire;
                currentAmmo--;

                /* Random bullet spray */
                //float randX = Random.Range(-spread / 50, spread / 50);
                //float randY = Random.Range(-spread / 50, spread / 50);
                //float randZ = Random.Range(-spread / 50, spread / 50);

                for (int locIndex = 0; locIndex < bulletSpawn.Length; locIndex++)
                {
                    var bullet = GameObject.Instantiate(bulletPrefab, bulletSpawn[locIndex].position, bulletSpawn[locIndex].rotation);

                    if (targetObject != null) ;
                    //bullet.transform.LookAt(firingSolutionPoint);

                    //bullet.transform.rotation *= Quaternion.Euler(randX, randY, randZ);

                    //bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 50;

                    bullet.GetComponent<Missile>().target = targetObject.transform;

                    //GameObject.Destroy(bullet, bulletLife);
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
    }

    /* 	
	 *	Returns a time to recall this function
	 *	Or maybe invoke repeating until a time
     */
    public int Reload()
    {
        isFiring = false;

        if (currentAmmo < maxAmmo && !isReloading)
        {
            isReloading = true;
			
            if (animator != null) 
				animator.SetBool("firing", false);
			
            nextReloadStart = Time.time;
            nextReloadEnd = Time.time + reloadTime;

            return 1;
        }
        else if (isReloading && Time.time >= nextReloadEnd)
        {
            currentAmmo = maxAmmo;
            isReloading = false;

            return 0;
        }

        return 1;
    }

    public void OnCooldown()
    {
        isFiring = false;
        if (animator != null)
            animator.SetBool("firing", false);
    }
}
