using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponSlot { LeftHand, RightHand, LeftShoulder, RightShoulder }
public enum WeaponStyle { Regular, Underhand, Shoulder}
public enum FireType { Regular, Beam, Charge, Multi}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Mecha/Weapon")]
public class WeaponItem : Item {

    //public GameObject animatorHolder;
    public GameObject prefab;
    public Image ammoIcon;
    public GameObject bullet;

    [Header("General")]
    public WeaponSlot slot = WeaponSlot.LeftHand;
    public WeaponStyle style = WeaponStyle.Regular;
    public FireType fireMode = FireType.Regular;
    public float chargeTime;
    public int projectileCount;
    public float beamTime;
    public float cooldown;

    public bool autoTarget;
    public bool needTarget;
    public float maxTrackDistance;

    [Header("Weapon Stats")]
    public int maxAmmo;

    public float damage;
    //public float energyDamage;

    //public float rateOfFire;    //Deprecate eventually
    public float rpm;
    [HideInInspector]
    public float secBetweenFire;

    public float reloadTime;
    [Range(0, 500)]
    public float bulletSpread;
    public float bulletSpeed;
    public float bulletLife;

}
