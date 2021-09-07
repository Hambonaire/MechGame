using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
public class ItemStruct
{
    public SectionIndex refIndex;

    public Item[] items;

    public ItemStruct(int size = 1)
    {
        items = new Item[size];
    }

    public ItemStruct(Item[] _items)
    {
        items = _items;
    }
}

/* This extended class does get used as holder for items for corresponding wep sections */
public class WeaponStruct : ItemStruct
{
	public WeaponItem[] primary;
	public WeaponItem[] secondary;
	public WeaponItem[] tertiary;

    public WeaponStruct(int size = 1)
    {
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
    Accessory[] equippedTorsoItems;
    Accessory[] equippedHeadItems;
    Accessory[] equippedLeftLegItems;
    Accessory[] equippedRightLegItems;
	
    [SerializeField]
    WeaponStruct equippedLeftArmWeapons;
	
	[SerializeField]
    WeaponStruct equippedRightArmWeapons;
	
    WeaponStruct equippedLeftShoulderWeapons;
    WeaponStruct equippedRightShoulderWeapons;

    public Mech()
    {
        equippedTorsoItems = new Accessory[1];
        equippedHeadItems = new Accessory[1];
        equippedLeftLegItems = new Accessory[1];
        equippedRightLegItems = new Accessory[1];

        equippedLeftArmWeapons = new WeaponStruct();
        equippedRightArmWeapons = new WeaponStruct();
        equippedLeftShoulderWeapons = new WeaponStruct();
        equippedRightShoulderWeapons = new WeaponStruct();
    }

    public Mech(MechBase mechBase)
    {
        mechBaseRef = mechBase;

        equippedTorsoItems = new Accessory[mechBaseRef.torsoSlots];
        equippedHeadItems = new Accessory[mechBaseRef.headSlots];
        equippedLeftLegItems = new Accessory[mechBaseRef.leftLegSlots];
        equippedRightLegItems = new Accessory[mechBaseRef.rightLegSlots];

        equippedLeftArmWeapons = new WeaponStruct(mechBaseRef.leftArmSlots);
        equippedRightArmWeapons = new WeaponStruct(mechBaseRef.rightArmSlots);
        equippedLeftShoulderWeapons = new WeaponStruct(mechBaseRef.leftShoulderSlots);
        equippedRightShoulderWeapons = new WeaponStruct(mechBaseRef.rightShoulderSlots);
    }

    public ItemStruct GetSectionItemsByIndex(int index)
    {
        ItemStruct retStruct;// = new ItemStruct();

        if (index == 0)
            retStruct = new ItemStruct(equippedTorsoItems);
        else if (index == 1)
            retStruct = new ItemStruct(equippedHeadItems);
        else if (index == 2)
            retStruct = new ItemStruct(equippedLeftLegItems);
        else if (index == 3)
            retStruct = new ItemStruct(equippedRightLegItems);

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
            equippedTorsoItems = itemStruct.items;
        else if (index == 1)
            equippedHeadItems = itemStruct.items;
        else if (index == 2)
            equippedLeftLegItems = itemStruct.items;
        else if (index == 3)
            equippedRightLegItems = itemStruct.items;
        else if (4 <= index && index <= 7)
            AddToWList(index, itemStruct as WeaponStruct);
        else
            return;
    }

    private void AddToWList(int index, WeaponStruct weaponStruct)
    {
        WeaponStruct refWStruct = new WeaponStruct();

        if (index == 4)
            refWStruct = equippedLeftArmWeapons;
        else if (index == 5)
            refWStruct = equippedRightArmWeapons;
        else if (index == 6)
            refWStruct = equippedLeftShoulderWeapons;
        else if (index == 7)
            refWStruct = equippedRightShoulderWeapons;

        refWStruct.primary = weaponStruct.primary;
        refWStruct.secondary = weaponStruct.secondary;
        refWStruct.tertiary = weaponStruct.tertiary;
    }
}
