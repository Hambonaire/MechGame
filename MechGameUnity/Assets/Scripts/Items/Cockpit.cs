using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cockpit", menuName = "Mecha/Cockpit")]
public class Cockpit : Item {

    public GameObject prefab;
    public float ballisticArmor;
    public float energyArmor;
    public float health;

    public float armDistanceX = 0;
    public float scaleFactor = 1;
}