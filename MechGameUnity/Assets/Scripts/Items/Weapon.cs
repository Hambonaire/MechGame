﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireType { Regular, Beam, Charge, Multi}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Mecha/Weapon")]
public class Weapon : Item {

    //public GameObject animatorHolder;
    public GameObject rightPrefab;
    public GameObject leftPrefab;

    public FireType fireMode = FireType.Regular;
    public float chargeTimeIfChargeType;
    public int projectilesCountIfMultiType;
    public float beamTimeIfBeamType;
    public float beamCooldownIfBeamType;

    public bool autoTarget;

    public int maxAmmo;

    public GameObject bullet;

    public float ballisticDamage;
    public float energyDamage;
    public float rateOfFire;
    public float reloadTime;
    [Range(0, 500)]
    public float bulletSpray;
    public float bulletSpeed;
    public float bulletLife;

}