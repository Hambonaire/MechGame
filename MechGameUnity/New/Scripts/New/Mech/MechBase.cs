using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 *  Mech Base holds the base model for mechs
 *  
 *  Mech Bases have
 *  - Base model, usually legs, torso, head, arms, shoulders(?)
 *  - The slot number for items for each section
 */
[CreateAssetMenu(fileName = "New Mech Base", menuName = "Mech/Base")]
public class MechBase : ScriptableObject
{
    public string baseName = "";

    /* Object of legs, torso, head(?) */
    public GameObject basePrefab;

    public int headSlots = 1;

    public int torsoSlots = 1;

    public int leftLegSlots = 1;
    public int rightLegSlots = 1;

    public int leftArmSlots = 1;
    public int rightArmSlots = 1;

    public int leftShoulderSlots = 1;
    public int rightShoulderSlots = 1;

}
