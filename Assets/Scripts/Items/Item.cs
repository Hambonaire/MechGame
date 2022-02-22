using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListItem
{
    public int count = 0;
    public Item item;

    public ListItem(Item _item, int _count)
    {
        count = _count;
        item = _item;
    }
}

public enum Tier {Common, Uncommon, Rare, Epic, Legendary}

public enum Rarity {Junk, Inferior, Stock, Calibrated, Perfect}

public enum Manufacturer { Callamax, Horizon, Immostrom, Imperial, JAndP, MilSurplus, Panther, Spyre, TriStar }

//public enum Type { }

[System.Serializable]
public class Item : ScriptableObject
{
    [Header("General")]
    new public string name = "New Item";
    public Sprite icon = null;
	
    public Tier tier;
	public Rarity rarity;

    public int price = 1000;
    
    public float bonusArmor = 0;
    public float bonusIntegrity = 0;
    public float bonusShield = 0;
    public float bonusShieldRegen = 0;
    
    [Header("Flavor")]
    public string description;
    public string manufacturer;

    
    // Called when the item is pressed in the inventory
    public virtual void Use()
    {
        // Use the item
        // Something may happen

    }

    // Call this method to remove the item from inventory
    public void RemoveFromInventory()
    {
        //InventoryManager.Instance.Remove(this);

    }
    
}
