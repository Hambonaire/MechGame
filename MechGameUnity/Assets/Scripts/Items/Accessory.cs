using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Accessory", menuName = "Mecha/Accessory")]
public class Accessory : Item
{

    public List<Modifier> modifiers;

    /* Defense - Already in Item.cs base */
    //public float bonusArmor = 0;
    //public float bonusIntegrity = 0;
    //public float bonusShield = 0;
    //public float bonusShieldRegen = 0;

    /*  */
    [Header("Weapon Systems")]
    public string ballisticDamageDesc = "HE Ammo";
    [Range(0, 1)]
    public float ballisticDamage = 0.0f;

    public string ballisticAccuracyDesc = "Recoil Compensator";
    [Range(0, 1)]
    public float ballisticAccuracy = 0.0f;

    public string laserDamageDesc = "Beam Amplifier";
    [Range(0, 1)]
    public float laserDamage = 0.0f;

    public string laserOverheatDesc = "Heatsink";
    [Range(0, 1)]
    public float laserOverheat = 0.0f;

    public string laserFireRateDesc = "Cycler";
    [Range(0, 1)]
    public float laserFireRate = 0.0f;


}
