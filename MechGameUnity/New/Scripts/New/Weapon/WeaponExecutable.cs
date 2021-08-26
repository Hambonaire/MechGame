using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public enum Side { Left, Right }
//public enum WeaponStyle { Regular, Underhand, Shoulder}
//public enum FireType { Regular, Beam, Charge, Multi}

public class WeaponExecutable {
    public Image ammoIcon;

    Transform barrel;
    GameObject bulletPrefab;

    public MechController controller;

    //public ScriptableFloat playerTargetDistance;
    //public ScriptableBool playerTargetInRange;
    bool autoTarget;
    bool needTarget;

    Animator animator;

    float ballisticDamage;
    float energyDamage;

    float ROF;
    float nextFire;
    float reloadTime;
    public float nextReloadStart;
    public float nextReloadEnd;
    public bool isFiring = false;
    public bool isReloading = false;

    public int maxAmmo;
    public int currentAmmo;
    float spread;
    float bulletSpeed;
    float bulletLife;

    FireType fireType;
    float chargeTime;
    float chargeCurr;
    float beamTime;
    float beamCurr;
    float cooldown;             // TODO: Implement this?
    int projectileCount;

    //float trackDistance;        // TODO: Implement this? Make it common to weapon class instead of unique?
    float targetDistance;
    float firingAngle;

    public WeaponExecutable()
    {
        Debug.Log("Made Weapon Executable WITHOUT an Item!");
    }

    public WeaponExecutable(WeaponItem wep, Transform barrel_, Animator anim, MechController controllerRef)
    {
        ammoIcon = wep.ammoIcon;
        fireType = wep.fireMode;
        bulletPrefab = wep.bullet;
        bulletSpeed = wep.bulletSpeed;
        bulletLife = wep.bulletLife;
        ballisticDamage = wep.ballisticDamage;
        energyDamage = wep.energyDamage;

        //ROF = wep.rateOfFire;
        ROF = wep.secBetweenFire;
        //Debug.Log("RoF: " + wep.rateOfFire);
        //Debug.Log("RPM: " + wep.rpm);
        //Debug.Log("SBF: " + wep.secBetweenFire);
        //Debug.Log("Actual SBF: " + (1 / (wep.rpm / 60)));

        reloadTime = wep.reloadTime;
        maxAmmo = wep.maxAmmo;
        spread = wep.bulletSpread;
        chargeTime = wep.chargeTime;
        beamTime = wep.beamTime;
        projectileCount = wep.projectileCount;
        //cooldown = wep.cooldown;         
        //autoTarget = wep.autoTarget;
        barrel = barrel_;

        currentAmmo = maxAmmo;
        chargeCurr = 0;
        beamCurr = 0;

        controller = controllerRef;

        //playerTargetDistance = targetDistanceRef;
        animator = anim;
    }

    public void Fire()
    {
        //if (needTarget && (controller.MyTarget == null || 10 /*playerTargetDistance.value*/ > targetDistance))// && playerTargetInRange.value)
        if (false)  
            return;

        isFiring = true;
        
        if (!isReloading)
        {
            if (Time.time > nextFire && currentAmmo > 0 )
            {
                // (needTarget && playerTarget != null) && playerTargetInRange.value
                if (fireType == FireType.Regular)
                {
                    nextFire = Time.time + ROF;
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
                else if (fireType == FireType.Charge)
                {
                    if (chargeCurr < chargeTime)
                    {
                        chargeCurr += Time.deltaTime;

                        //Debug.Log("Primary Charging: " + right_chargeCurr + " out of: " + right_chargeTime);
                    }
                    else if (chargeCurr >= chargeTime)
                    {
                        nextFire = Time.time + ROF;
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

                        chargeCurr = 0;
                    }
                }
                else if (fireType == FireType.Multi)
                {
                    nextFire = Time.time + ROF;
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

                        nextFire = Time.time + ROF;
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
            else if (currentAmmo == 0 && !isReloading)
            {
                Reload();
                isFiring = false;
            }
            if (animator != null)
            {
                animator.SetBool("firing", true);
            }
        }
        else
        {
            isFiring = false;
            if (animator != null)
            {
                animator.SetBool("firing", false);
            }
        }
    }
    
    // Returns a time to recall this function
    // Or maybe invoke repeating until a time
    public void Reload ()
    {
        isFiring = false;
        if (currentAmmo < maxAmmo && !isReloading)
        {
            isReloading = true;
            if (animator != null) animator.SetBool("firing", false);
            //Invoke("Reload", reloadTime);
            nextReloadStart = Time.time;
            nextReloadEnd = Time.time + reloadTime;
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
        {
            animator.SetBool("firing", false);
        }
    }
}
