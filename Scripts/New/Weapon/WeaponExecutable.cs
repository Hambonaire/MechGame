using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public enum Side { Left, Right }
//public enum WeaponStyle { Regular, Underhand, Shoulder}
//public enum FireType { Regular, Beam, Charge, Multi}

[System.Serializable]
public class WeaponExecutable {
    public Image ammoIcon;

    public Transform bulletSpawn;
    public GameObject bulletPrefab;

    public MechController controller;

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

    public WeaponExecutable(GameObject obj, WeaponItem weapon, Transform barrel, Animator anim)
    {
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
        chargeTime = weapon.chargeTime;
        beamTime = weapon.beamTime;
        projectileCount = weapon.projectileCount;

        bulletSpawn = barrel;

        currentAmmo = maxAmmo;
        chargeCurr = 0;
        beamCurr = 0;

        animator = anim;
    }

    public bool Fire()
    {
        bool ret = false;

        //if (needTarget && (controller.MyTarget == null || 10 /*playerTargetDistance.value*/ > targetDistance))// && playerTargetInRange.value)
        //    return;

        isFiring = true;
        
        if (!isReloading)
        {
            if (Time.time > nextFire && currentAmmo > 0 )
            {
				nextFire = Time.time + rateOfFire;
				currentAmmo--;

				/* Random bullet spray */
				float randX = Random.Range(-spread / 50, spread / 50);
				float randY = Random.Range(-spread / 50, spread / 50);
				float randZ = Random.Range(-spread / 50, spread / 50);

				var bullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

                /* Auto targeting */
                /*
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
                */

                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * bulletSpeed;

                bullet.GetComponent<Bullet>().SetDamage(damage);

				GameObject.Destroy(bullet, bulletLife);
				
				/**
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
				*/
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
    public bool Reload()
    {
        isFiring = false;

        if (currentAmmo < maxAmmo && !isReloading)
        {
            isReloading = true;
			
            if (animator != null) 
				animator.SetBool("firing", false);
			
            nextReloadStart = Time.time;
            nextReloadEnd = Time.time + reloadTime;

            return true;
        }
        else if (isReloading && Time.time >= nextReloadEnd)
        {
            currentAmmo = maxAmmo;
            isReloading = false;

            return false;
        }

        return false;
    }



}
