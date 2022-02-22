using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Legs", menuName = "Mech Item/Legs")]
public class Legs : Item {

    public GameObject prefab;

    public float walkSpeed;
    public float runSpeed;

    public float scaleFactor = 1;
}
