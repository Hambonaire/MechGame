using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechTables : MonoBehaviour
{
	public static MechTables _instance;

    public List<Item> itemTable = new List<Item>();
    
    public List<WeaponItem> weaponTable = new List<WeaponItem>();

    public List<MechBase> mechTemplateTable = new List<MechBase>();

    public List<MechPattern> mechPatternTable = new List<MechPattern>();

	void Awake()
	{
		_instance = this;
	}
	
	public List<Item> GenerateListOfItems(int tier = 1, int rarity = 0, string manufacturer = "")
	{
		foreach (WeaponItem item in weaponTable)
		{
			if (item.tier <= tier && (int)item.rarity <= rarity)
		}
	}
}
