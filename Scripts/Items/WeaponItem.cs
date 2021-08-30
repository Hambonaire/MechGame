﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FireType { Regular, Beam, Charge, Multi}

public enum WeaponType { Ballistic, Energy, Missile }

[System.Serializable]
[CreateAssetMenu(fileName = "New Weapon", menuName = "Mecha/Weapon")]
public class WeaponItem : Item {

    public GameObject prefab;
    public Image ammoIcon;
    public GameObject bullet;

    [Header("Weapon")]
    public FireType fireType = FireType.Regular;
    public WeaponType weaponType = WeaponType.Ballistic;
    //public float chargeTime;
    //public int projectileCount;
    //public float beamTime;
    //public float cooldown;

    public bool autoTarget;
    public bool needTarget;
    public float maxTrackDistance;

    [Header("Weapon Stats")]
    public int maxAmmo;

    public float damage;

    //public float rateOfFire;    //Deprecate eventually
    //public float rpm;
    public float secBetweenFire;

    public float reloadTime;
    [Range(0, 500)]
    public float bulletSpread;
    public float bulletSpeed;
    public float bulletLife;

}