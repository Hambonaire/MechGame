using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mech
{
    /* 
     * -1: Nothing
     * 0: Torso
     * 1: Head
     * 2: Legs
     * 3: Left Arm
     * 4: Right Arm
     * 5: Left Shoulder
     * 6: Right Shoulder 
     */

    /* Mech Base scriptable object ref */
    public MechBase mechBaseRef;

    /* Weapons/Items sriptable object refs */
    Item[] equippedTorsoItems;
    Item[] equippedHeadItems;
    Item[] equippedLegItems;
    [SerializeField]
    Item[] equippedLeftArmWeapons;
    [SerializeField]
    Item[] equippedRightArmWeapons;
    Item[] equippedLeftShoulderWeapons;
    Item[] equippedRightShoulderWeapons;

    /* Weapon Gameobject refs */
    List<GameObject> equippedLeftArmObj;
    List<GameObject> equippedRightArmObj;
    List<GameObject> equippedLeftShoulderObj;
    List<GameObject> equippedRightShoulderObj;

    public Mech()
    {
        equippedTorsoItems = new Item[1];
        equippedHeadItems = new Item[1];
        equippedLegItems = new Item[1];
        equippedLeftArmWeapons = new Item[1];
        equippedRightArmWeapons = new Item[1];
        equippedLeftShoulderWeapons = new Item[1];
        equippedRightShoulderWeapons = new Item[1];

        equippedLeftArmObj = new List<GameObject>();
        equippedRightArmObj = new List<GameObject>();
        equippedLeftShoulderObj = new List<GameObject>();
        equippedRightShoulderObj = new List<GameObject>();
    }

    public Mech(MechBase mechBase)
    {
        mechBaseRef = mechBase;

        equippedTorsoItems = new Item[mechBaseRef.torsoSlots];
        equippedHeadItems = new Item[mechBaseRef.headSlots];
        equippedLegItems = new Item[mechBaseRef.legSlots];
        equippedLeftArmWeapons = new Item[mechBaseRef.leftArmSlots];
        equippedRightArmWeapons = new Item[mechBaseRef.rightArmSlots];
        equippedLeftShoulderWeapons = new Item[mechBaseRef.leftShoulderSlots];
        equippedRightShoulderWeapons = new Item[mechBaseRef.rightShoulderSlots];

        equippedLeftArmObj = new List<GameObject>();
        equippedRightArmObj = new List<GameObject>();
        equippedLeftShoulderObj = new List<GameObject>();
        equippedRightShoulderObj = new List<GameObject>();
    }

    public Item[] GetSectionItemsByIndex(int index)
    {
        if (index == 0)
            return equippedTorsoItems;
        else if (index == 1)
            return equippedHeadItems;
        else if (index == 2)
            return equippedLegItems;
        else if (index == 3)
            return equippedLeftArmWeapons;
        else if (index == 4)
            return equippedRightArmWeapons;
        else if (index == 5)
            return equippedLeftShoulderWeapons;
        else if (index == 6)
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
            return mechBaseRef.legSlots;
        else if (index == 3)
            return mechBaseRef.leftArmSlots;
        else if (index == 4)
            return mechBaseRef.rightArmSlots;
        else if (index == 5)
            return mechBaseRef.leftShoulderSlots;
        else if (index == 6)
            return mechBaseRef.rightShoulderSlots;
        else
            return 0;
    }

    public List<GameObject> GetWeaponObjByIndex(int index)
    {
        if (index == 3)
            return equippedLeftArmObj;
        else if (index == 4)
            return equippedRightArmObj;
        else if (index == 5)
            return equippedLeftShoulderObj;
        else if (index == 6)
            return equippedRightShoulderObj;
        else
            return null;
    }
    
    public void ResetObjLists()
    {
        equippedLeftArmObj.Clear();
        equippedRightArmObj.Clear();
        equippedLeftShoulderObj.Clear();
        equippedRightShoulderObj.Clear();
    }
}
