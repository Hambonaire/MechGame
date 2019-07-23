using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Legs", menuName = "Mecha/Legs")]
public class Legs : Item {

    public GameObject prefab;
    public float ballisticArmor;
    public float energyArmor;
    public float health;

    public float walkSpeed;
    public float runSpeed;

    public float scaleFactor = 1;
}
