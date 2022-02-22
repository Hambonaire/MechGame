using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponClass { Primary, Secondary, Tertiary }

public enum FireType { Regular, Volley, Beam, Charge }

public enum AmmoType { Ballistic, Energy, Missile }

[System.Serializable]
[CreateAssetMenu(fileName = "New Weapon", menuName = "Mech Item/Weapon")]
public class WeaponItem : Item {

    public GameObject prefab;
    public Image ammoIcon;
    public GameObject bullet;

    [Header("Animation")]
    public ParticleSystem hitEffect; 
    public bool needsAnimationEvent;


    [Header("Weapon")]
    public FireType fireType = FireType.Regular;
    public AmmoType ammoType = AmmoType.Ballistic;
    public WeaponClass weaponClass = WeaponClass.Primary;

    //public float chargeTime;
    [Header("Multi & Missile")]
    public int volleyCount;
    public float volleyDelay;
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

    public float bulletSpeed;
    public float bulletLife;

    [Header("Spread")]
    [Range(0, 40)]
    public int bulletSpread = 20;

    [Header("Bloom")]
    // % per shot
    [Range(0, 1)]
    public float bloomIncrement = 0.1f;
    // % per second
    [Range(0, 1)]
    public float bloomDecrement = 0.3f;

    [Header("Missile Vars")]
    public float explosionRadius = 0.5f;
    public Vector2 turnMinMax = new Vector3(50, 400);
    public Vector2 offsetMinMax = new Vector2(3, 50);
}
