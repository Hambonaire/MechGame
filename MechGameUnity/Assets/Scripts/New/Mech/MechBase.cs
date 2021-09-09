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

	[Header("Slots")]
    public int headSlots = 0;
    public int torsoSlots = 0;
    public int leftLegSlots = 0;
    public int rightLegSlots = 0;
    public int leftArmSlots = 0;
    public int rightArmSlots = 0;
    public int leftShoulderSlots = 0;
    public int rightShoulderSlots = 0;

	[Header("Stats")]
	public float baseMoveSpeed = 5;
}
