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

public enum SectionIndex { torso, head, legs, leftArm, rightArm, leftShoulder, rightShoulder};

/* For passing items from scripts, not for usage in the Mech really */
[System.Serializable]
public class ItemStruct
{
    SectionIndex refIndex;

    private int sizeU;
    private int sizeP;
    private int sizeS;
    private int sizeT;

    public Accessory[] upgrades = new Accessory[0];

    public WeaponItem[] primary = new WeaponItem[0];
	public WeaponItem[] secondary = new WeaponItem[0];
	public WeaponItem[] tertiary = new WeaponItem[0];

    public ItemStruct(int sizeU = 0, int sizeP = 0, int sizeS = 0, int sizeT = 0)
    {
        this.sizeU = sizeU;
        this.sizeP = sizeP;
        this.sizeS = sizeS;
        this.sizeT = sizeT;

        upgrades = new Accessory[sizeU];

        primary = new WeaponItem[sizeP];
        secondary = new WeaponItem[sizeS];
        tertiary = new WeaponItem[sizeT];
    }

    public void Init()
    {
        Array.Resize(ref upgrades, sizeU);
        Array.Resize(ref primary, sizeP);
        Array.Resize(ref secondary, sizeS);
        Array.Resize(ref tertiary, sizeT);
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
    ItemStruct equippedLegItems;
	
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
        equippedLegItems = new ItemStruct();

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
        equippedLegItems = new ItemStruct(mechBaseRef.legSlots);

        equippedLeftArmWeapons = new ItemStruct(mechBaseRef.leftArmSlotsU, mechBaseRef.leftArmSlotsP, mechBaseRef.leftArmSlotsS, mechBaseRef.leftArmSlotsT);
        equippedRightArmWeapons = new ItemStruct(mechBaseRef.rightArmSlotsU, mechBaseRef.rightArmSlotsP, mechBaseRef.rightArmSlotsS, mechBaseRef.rightArmSlotsT);
        equippedLeftShoulderWeapons = new ItemStruct(mechBaseRef.leftShoulderSlotsU, mechBaseRef.leftShoulderSlotsP, mechBaseRef.leftShoulderSlotsS, mechBaseRef.leftShoulderSlotsT);
        equippedRightShoulderWeapons = new ItemStruct(mechBaseRef.rightShoulderSlotsU, mechBaseRef.rightShoulderSlotsP, mechBaseRef.rightShoulderSlotsS, mechBaseRef.rightShoulderSlotsT);
    }

    public void Initialize()
    {
        equippedTorsoItems.Init();
        equippedHeadItems.Init();
        equippedLegItems.Init();
        equippedLeftArmWeapons.Init();
        equippedRightArmWeapons.Init();
        equippedLeftShoulderWeapons.Init();
        equippedRightShoulderWeapons.Init();
    }

    public ItemStruct GetSectionItemsByIndex(int index)
    {
        if (index == (int)SectionIndex.torso)
            return equippedTorsoItems;
        else if (index == (int)SectionIndex.head)
            return equippedHeadItems;
        else if (index == (int)SectionIndex.legs)
            return equippedLegItems;
        else if (index == (int)SectionIndex.leftArm)
            return equippedLeftArmWeapons;

        else if (index == (int)SectionIndex.rightArm)
            return equippedRightArmWeapons;

        else if (index == (int)SectionIndex.leftShoulder)
            return equippedLeftShoulderWeapons;

        else if (index == (int)SectionIndex.rightShoulder)
            return equippedRightShoulderWeapons;

        else
            return null;
    }

    public int GetSubsectionCountByIndex(int index, int subIndex = 0)
    {
        if (index == (int)SectionIndex.torso)
            return mechBaseRef.torsoSlots;
        else if (index == (int)SectionIndex.head)
            return mechBaseRef.headSlots;
        else if (index == (int)SectionIndex.legs)
            return mechBaseRef.legSlots;

        else if (index == (int)SectionIndex.leftArm && subIndex == 0)
            return mechBaseRef.leftArmSlotsP;
        else if (index == (int)SectionIndex.leftArm && subIndex == 1)
            return mechBaseRef.leftArmSlotsP;
        else if (index == (int)SectionIndex.leftArm && subIndex == 2)
            return mechBaseRef.leftArmSlotsT;

        else if (index == (int)SectionIndex.rightArm && subIndex == 0)
            return mechBaseRef.rightArmSlotsP;
        else if (index == (int)SectionIndex.rightArm && subIndex == 1)
            return mechBaseRef.rightArmSlotsS;
        else if (index == (int)SectionIndex.rightArm && subIndex == 2)
            return mechBaseRef.rightArmSlotsT;

        else if (index == (int)SectionIndex.leftShoulder && subIndex == 0)
            return mechBaseRef.leftShoulderSlotsP;
        else if (index == (int)SectionIndex.leftShoulder && subIndex == 1)
            return mechBaseRef.leftShoulderSlotsS;
        else if (index == (int)SectionIndex.leftShoulder && subIndex == 2)
            return mechBaseRef.leftShoulderSlotsT;

        else if (index == (int)SectionIndex.rightShoulder && subIndex == 0)
            return mechBaseRef.rightShoulderSlotsP;
        else if (index == (int)SectionIndex.rightShoulder && subIndex == 1)
            return mechBaseRef.rightShoulderSlotsS;
        else if (index == (int)SectionIndex.rightShoulder && subIndex == 2)
            return mechBaseRef.rightShoulderSlotsT;
        else
            return 0;
    }

    public void SetSectionItemsByIndex(int index, ItemStruct itemStruct)
    {
        if (index == (int)SectionIndex.torso)
            equippedTorsoItems = itemStruct;
        else if (index == (int)SectionIndex.head)
            equippedHeadItems = itemStruct;
        else if (index == (int)SectionIndex.legs)
            equippedLegItems = itemStruct;
        else if (index == (int)SectionIndex.leftArm)
            equippedLeftArmWeapons = itemStruct;
        else if (index == (int)SectionIndex.rightArm)
            equippedRightArmWeapons = itemStruct;
        else if (index == (int)SectionIndex.leftShoulder)
            equippedLeftShoulderWeapons = itemStruct;
        else if (index == (int)SectionIndex.rightShoulder)
            equippedRightShoulderWeapons = itemStruct;
        else
            return;
    }

}
