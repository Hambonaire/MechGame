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

    public int leftArmSlotsU = 0;
    public int leftArmSlotsP = 0;
    public int leftArmSlotsS = 0;
    public int leftArmSlotsT = 0;

    public int rightArmSlotsU = 0;
    public int rightArmSlotsP = 0;
    public int rightArmSlotsS = 0;
    public int rightArmSlotsT = 0;

    public int leftShoulderSlotsU = 0;
    public int leftShoulderSlotsP = 0;
    public int leftShoulderSlotsS = 0;
    public int leftShoulderSlotsT = 0;

    public int rightShoulderSlotsU = 0;
    public int rightShoulderSlotsP = 0;
    public int rightShoulderSlotsS = 0;
    public int rightShoulderSlotsT = 0;


	[Header("Stats")]
	public float baseMoveSpeed = 5;
}
