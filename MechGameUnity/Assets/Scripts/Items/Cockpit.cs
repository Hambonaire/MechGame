using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cockpit", menuName = "Mecha/Cockpit")]
public class Cockpit : Item {

    public int rightRegularCount = 1;
    public int rightUnderhandCount = 0;
    public int rightShoulderCount = 0;
    
    public int leftRegularCount = 1;
    public int leftUnderhandCount = 0;
    public int leftShoulderCount = 0;
    
    public int accessoryCount = 0;
    
    public GameObject prefab;
    public float ballisticArmor;
    public float energyArmor;
    public float health;

    public float armDistanceX = 0;
    public float scaleFactor = 1;
}
