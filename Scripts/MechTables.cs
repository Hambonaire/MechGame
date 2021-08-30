using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TableEntryWeapon
{
    public Weapon item; 
}

public class TableEntryMech
{
    public Mech mech;
}

public class MechTables : MonoBehaviour
{

    public List<Item> itemTable = new List<Item>();
    
    public List<WeaponItem> weaponTable = new List<WeaponItem>();

    public List<MechBase> mechTemplateTable = new List<MechBase>();

    public List<MechPattern> mechPatternTable = new List<MechPattern>();


}
