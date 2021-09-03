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
    Item[] equippedTorsoItems;
    Item[] equippedHeadItems;
    Item[] equippedLeftLegItems;
    Item[] equippedRightLegItems;
    [SerializeField]
    Item[] equippedLeftArmWeapons;
    [SerializeField]
    Item[] equippedRightArmWeapons;
    Item[] equippedLeftShoulderWeapons;
    Item[] equippedRightShoulderWeapons;

    public Mech()
    {
        equippedTorsoItems = new Item[1];
        equippedHeadItems = new Item[1];
        equippedLeftLegItems = new Item[1];
        equippedRightLegItems = new Item[1];
        equippedLeftArmWeapons = new Item[1];
        equippedRightArmWeapons = new Item[1];
        equippedLeftShoulderWeapons = new Item[1];
        equippedRightShoulderWeapons = new Item[1];
    }

    public Mech(MechBase mechBase)
    {
        mechBaseRef = mechBase;

        equippedTorsoItems = new Item[mechBaseRef.torsoSlots];
        equippedHeadItems = new Item[mechBaseRef.headSlots];
        equippedLeftLegItems = new Item[mechBaseRef.leftLegSlots];
        equippedRightLegItems = new Item[mechBaseRef.rightLegSlots];
        equippedLeftArmWeapons = new Item[mechBaseRef.leftArmSlots];
        equippedRightArmWeapons = new Item[mechBaseRef.rightArmSlots];
        equippedLeftShoulderWeapons = new Item[mechBaseRef.leftShoulderSlots];
        equippedRightShoulderWeapons = new Item[mechBaseRef.rightShoulderSlots];
    }

    public Item[] GetSectionItemsByIndex(int index)
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

    public int GetSubsectionCountByIndex(int index)
    {
        if (index == 0)
            return mechBaseRef.torsoSlots;
        else if (index == 1)
            return mechBaseRef.headSlots;
        else if (index == 2)
            return mechBaseRef.leftLegSlots;
        else if (index == 3)
            return mechBaseRef.rightLegSlots;
        else if (index == 4)
            return mechBaseRef.leftArmSlots;
        else if (index == 5)
            return mechBaseRef.rightArmSlots;
        else if (index == 6)
            return mechBaseRef.leftShoulderSlots;
        else if (index == 7)
            return mechBaseRef.rightShoulderSlots;
        else
            return 0;
    }

    public void SetSectionItemsByIndex(int index, List<Item> items)
    {
        Item[] modArray;

        if (index == 0)
            modArray = equippedTorsoItems;
        else if (index == 1)
            modArray = equippedHeadItems;
        else if (index == 2)
            modArray = equippedLeftLegItems;
        else if (index == 3)
            modArray = equippedRightLegItems;
        else if (index == 4)
            modArray = equippedLeftArmWeapons;
        else if (index == 5)
            modArray = equippedRightArmWeapons;
        else if (index == 6)
            modArray = equippedLeftShoulderWeapons;
        else if (index == 7)
            modArray = equippedRightShoulderWeapons;
        else
            return;

        for (int ii = 0; ii < modArray.Length; ii++)
        {
            if (ii >= items.Count || items[ii] == null)
                modArray[ii] = null;
            else
                modArray[ii] = items[ii];
        }
    }
}
