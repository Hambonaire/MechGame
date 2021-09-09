using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 *  Mech objects (not GameObjects) are held in memory as the complete mechs
 *  - Has Base Model & equipped weapons and items
 *  
 *  Mech's are built from the MechBase scriptable object reference & equipped items lists
 *  - Mech builder creates the base model
 *  - Mech builder creates the weapon models and places refs in the MechManager in the new mech's encapsulating GameObject 
 *  
 */

public enum SectionIndex { torso, head, leftLeg, rightLeg, leftArm, rightArm, leftShoulder, rightShoulder};

/* For passing items from scripts, not for usage in the Mech really */
[System.Serializable]
public class ItemStruct
{
    SectionIndex refIndex;

    public Accessory[] upgrades = new Accessory[0];

    public WeaponItem[] primary = new WeaponItem[0];
	public WeaponItem[] secondary = new WeaponItem[0];
	public WeaponItem[] tertiary = new WeaponItem[0];

    public ItemStruct(int size = 0)
    {
        upgrades = new Accessory[size];

        primary = new WeaponItem[size];
        secondary = new WeaponItem[size];
        tertiary = new WeaponItem[size];
    }
}

[System.Serializable]
public class Mech
{
    /* 
     * -1: Nothing
     * 0: Torso
     * 1: Head
     * 2: Left Leg
     * 3: Right Leg
     * 4: Left Arm
     * 5: Right Arm
     * 6: Left Shoulder
     * 7: Right Shoulder 
     */

    /* Mech Base scriptable object ref */
    public MechBase mechBaseRef;

    /* Weapons/Items sriptable object refs */
    [SerializeField]
    ItemStruct equippedTorsoItems;

    [SerializeField]
    ItemStruct equippedHeadItems;

    [SerializeField]
    ItemStruct equippedLeftLegItems;

    [SerializeField]
    ItemStruct equippedRightLegItems;
	
    [SerializeField]
    ItemStruct equippedLeftArmWeapons;
	
	[SerializeField]
    ItemStruct equippedRightArmWeapons;

    [SerializeField]
    ItemStruct equippedLeftShoulderWeapons;

    [SerializeField]
    ItemStruct equippedRightShoulderWeapons;

    public Mech()
    {
        equippedTorsoItems = new ItemStruct();
        equippedHeadItems = new ItemStruct();
        equippedLeftLegItems = new ItemStruct();
        equippedRightLegItems = new ItemStruct();

        equippedLeftArmWeapons = new ItemStruct();
        equippedRightArmWeapons = new ItemStruct();
        equippedLeftShoulderWeapons = new ItemStruct();
        equippedRightShoulderWeapons = new ItemStruct();
    }

    public Mech(MechBase mechBase)
    {
        mechBaseRef = mechBase;

        equippedTorsoItems = new ItemStruct(mechBaseRef.torsoSlots);
        equippedHeadItems = new ItemStruct(mechBaseRef.headSlots);
        equippedLeftLegItems = new ItemStruct(mechBaseRef.leftLegSlots);
        equippedRightLegItems = new ItemStruct(mechBaseRef.rightLegSlots);

        equippedLeftArmWeapons = new ItemStruct(mechBaseRef.leftArmSlots);
        equippedRightArmWeapons = new ItemStruct(mechBaseRef.rightArmSlots);
        equippedLeftShoulderWeapons = new ItemStruct(mechBaseRef.leftShoulderSlots);
        equippedRightShoulderWeapons = new ItemStruct(mechBaseRef.rightShoulderSlots);
    }

    public void Initialize()
    {
        
    }

    public ItemStruct GetSectionItemsByIndex(int index)
    {
        if (index == 0)
            return equippedTorsoItems;
        else if (index == 1)
            return equippedHeadItems;
        else if (index == 2)
            return equippedLeftLegItems;
        else if (index == 3)
            return equippedRightLegItems;

        else if (index == 4)
            return equippedLeftArmWeapons;

        else if (index == 5)
            return equippedRightArmWeapons;

        else if (index == 6)
            return equippedLeftShoulderWeapons;

        else if (index == 7)
            return equippedRightShoulderWeapons;

        else
            return null;
    }

    public int GetSubsectionCountByIndex(int index, int subIndex = 0)
    {
        if (index == 0)
            return mechBaseRef.torsoSlots;
        else if (index == 1)
            return mechBaseRef.headSlots;
        else if (index == 2)
            return mechBaseRef.leftLegSlots;
        else if (index == 3)
            return mechBaseRef.rightLegSlots;

        else if (index == 4 && subIndex == 0)
            return mechBaseRef.leftArmSlots;
        else if (index == 4 && subIndex == 1)
            return mechBaseRef.leftArmSlots;
        else if (index == 4 && subIndex == 2)
            return mechBaseRef.leftArmSlots;

        else if (index == 5 && subIndex == 0)
            return mechBaseRef.rightArmSlots;
        else if (index == 5 && subIndex == 1)
            return mechBaseRef.rightArmSlots;
        else if (index == 5 && subIndex == 2)
            return mechBaseRef.rightArmSlots;

        else if (index == 6 && subIndex == 0)
            return mechBaseRef.leftShoulderSlots;
        else if (index == 6 && subIndex == 1)
            return mechBaseRef.leftShoulderSlots;
        else if (index == 6 && subIndex == 2)
            return mechBaseRef.leftShoulderSlots;

        else if (index == 7 && subIndex == 0)
            return mechBaseRef.rightShoulderSlots;
        else if (index == 7 && subIndex == 1)
            return mechBaseRef.rightShoulderSlots;
        else if (index == 7 && subIndex == 2)
            return mechBaseRef.rightShoulderSlots;
        else
            return 0;
    }

    public void SetSectionItemsByIndex(int index, ItemStruct itemStruct)
    {
        if (index == 0)
            equippedTorsoItems = itemStruct;
        else if (index == 1)
            equippedHeadItems = itemStruct;
        else if (index == 2)
            equippedLeftLegItems = itemStruct;
        else if (index == 3)
            equippedRightLegItems = itemStruct;
        else if (index == 4)
            equippedLeftArmWeapons = itemStruct;
        else if (index == 5)
            equippedRightArmWeapons = itemStruct;
        else if (index == 6)
            equippedLeftShoulderWeapons = itemStruct;
        else if (index == 7)
            equippedRightShoulderWeapons = itemStruct;
        else
            return;
    }

}
