using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side { Left, Right }
public enum WeaponStyle { Regular, Underhand, Shoulder}
public enum FireType { Regular, Beam, Charge, Multi}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Mecha/Weapon")]
public class Weapon : Item {

    //public GameObject animatorHolder;
    public GameObject rightPrefab;
    public GameObject leftPrefab;

    public Side side = Side.Left;
    public WeaponStyle style = WeaponStyle.Regular;
    public FireType fireMode = FireType.Regular;
    public float chargeTime;
    public int projectileCount;
    public float beamTime;
    public float cooldown;

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
