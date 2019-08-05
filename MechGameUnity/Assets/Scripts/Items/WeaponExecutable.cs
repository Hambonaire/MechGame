﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum Side { Left, Right }
//public enum WeaponStyle { Regular, Underhand, Shoulder}
//public enum FireType { Regular, Beam, Charge, Multi}

public class WeaponExecutable {
    Transform barrel;
    GameObject bulletPrefab;
    
    public ScriptableGameObject playerTarget;

    float isFiring = false;
    
    float ballisticDamage;
    float energyDamage;
    
    float ROF;
    float nextFire;
    float reloadTime;
    float nextReloadEnd;
    bool isReloading = false;
    
    int maxAmmo;
    int currentAmmo;
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
    
    bool autoTrack;
    float trackDistance;        // TODO: Implement this? Make it common to weapon class instead of unique?

    // Constructor, pass in Weapon
    public WeaponExecutable(Weapon wep, Transform barrel_)
    {
        fireType = wep.fireMode;    
        bulletPrefab = wep.bullet;
        bulletSpeed = wep.bulletSpeed;
        bulletLife = wep.bulletLife;      
        ballisticDamage = wep.ballisticDamage;
        energyDamage = wep.energyDamage;
        ROF = wep.rateOfFire;
        reloadTime = wep.reloadTime;
        maxAmmo = wep.maxAmmo;
        spread = wep.bulletSpread;
        chargeTime = wep.chargeTime;
        projectileCount = wep.projectileCount;
        beamTime = wep.beamTime;
        cooldown = wep.cooldown;         
        autoTrack = wep.autoTrack;
        barrel = barrel_;
        
        currentAmmo = maxAmmo;
        chargeCurr = 0;
        beamCurr = 0;
    }
    
    void Fire()
    {
        isFiring = true;
        
        if (!isReloading)
        {
            if (Time.time > nextFire && currentAmmo > 0)
            {
                if (fireType == FireType.Regular)
                {
                    nextFire = Time.time + ROF;
                    currentAmmo--;

                    // Random bullet spray
                    float randX = Random.Range(-spread / 50, spread / 50);
                    float randY = Random.Range(-spread / 50, spread / 50);
                    float randZ = Random.Range(-spread / 50, spread / 50);

                    // Create the Bullet from the Bullet Prefab
                    var bullet = (GameObject)Instantiate(bulletPrefab, barrel.position, barrel.rotation);

                    if (autoTrack && playerTarget.value != null)
                    {
                        bullet.transform.LookAt(playerTarget.value.transform);
                        bullet.transform.rotation *= Quaternion.Euler(92 + randX, -.01f * targetDistance + randY, randZ);
                        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * bulletSpeed;
                    }
                    else
                    {
                        bullet.transform.rotation *= Quaternion.Euler(92 + randX, (-firingAngle * 0.7f) + randY, randZ);
                        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * bulletSpeed;
                    }

                    // Put stats on the bullet
                    bullet.GetComponent<Bullet>().SetBallisticDamage(ballisticDamage);
                    bullet.GetComponent<Bullet>().SetEnergyDamage(energyDamage);
                    bullet.GetComponent<Bullet>().SetFaction(0);

                    // Destroy the bullet after _ seconds
                    Destroy(bullet, bulletLife);
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
                        var bullet = (GameObject)Instantiate(bulletPrefab, barrel.position, barrel.rotation);

                        if (autoTrack && playerTarget.value != null)
                        {
                            bullet.transform.LookAt(playerTarget.value.transform);
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, -.01f * targetDistance + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * bulletSpeed;
                        }
                        else
                        {
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, (-firingAngle * 0.7f) + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * bulletSpeed;
                        }

                        // Put stats on the bullet
                        bullet.GetComponent<Bullet>().SetBallisticDamage(ballisticDamage);
                        bullet.GetComponent<Bullet>().SetEnergyDamage(energyDamage);
                        bullet.GetComponent<Bullet>().SetFaction(0);

                        // Destroy the bullet after _ seconds
                        Destroy(bullet, bulletLife);

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
                        var bullet = (GameObject)Instantiate(bulletPrefab, barrel.position, barrel.rotation);

                        if (autoTrack && playerTarget.value != null)
                        {
                            bullet.transform.LookAt(playerTarget.value.transform);
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, -.01f * targetDistance + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * bulletSpeed;
                        }
                        else
                        {
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, (-firingAngle * 0.7f) + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * bulletSpeed;
                        }

                        // Put stats on the bullet
                        bullet.GetComponent<Bullet>().SetBallisticDamage(ballisticDamage);
                        bullet.GetComponent<Bullet>().SetEnergyDamage(energyDamage);
                        bullet.GetComponent<Bullet>().SetFaction(0);
                        
                        // Destroy the bullet after _ seconds
                        Destroy(bullet, bulletLife);
                    }
                }
                else if (right_fireType == FireType.Beam)
                {
                    if (right_beamCurr >= right_beamTime)
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
                        var bullet = (GameObject)Instantiate(bulletPrefab, barrel.position, barrel.rotation);

                        if (right_autoTrack && playerTarget.value != null)
                        {
                            bullet.transform.LookAt(playerTarget.value.transform);
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, -.01f * targetDistance + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * bulletSpeed;
                        }
                        else
                        {
                            bullet.transform.rotation *= Quaternion.Euler(92 + randX, (-firingAngle * 0.7f) + randY, randZ);
                            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * bulletSpeed;
                        }

                        // Put stats on the bullet
                        bullet.GetComponent<Bullet>().SetBallisticDamage(ballisticDamage);
                        bullet.GetComponent<Bullet>().SetEnergyDamage(energyDamage);
                        bullet.GetComponent<Bullet>().SetFaction(0);
                        
                        // Destroy the bullet after _ seconds
                        Destroy(bullet, bulletLife);
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
    
    void Reload ()
    {
        isFiring = false;
        if (currentAmmo < maxAmmo && !isReloading)
        {
            isReloading = true;
            if (animator != null) animator.SetBool("firing", false);
            Invoke("Reload", reloadTime);
            nextReloadEnd = Time.time + reloadTime;
        }
        else if (isReloading && Time.time >= (nextReloadEnd - 0.1f))
        {
            currentAmmo = maxAmmo;
            isReloading = false;
        }
    }
    
    void OnCooldown()
    {
        isFiring = false;
        
        if (animator != null)
        {
            leftWeaponAnimator.SetBool("firing", false);
        }

        if (fireType == FireType.Charge)
        {
            chargeCurr -= Time.deltaTime;
            chargeCurr = Mathf.Clamp(left_chargeCurr, 0, Mathf.Infinity);
        }
        else if (fireType == FireType.Beam)
        {
            beamCurr -= Time.deltaTime;
            beamCurr = Mathf.Clamp(left_beamCurr, 0, left_beamTime);
        }
    }
}
