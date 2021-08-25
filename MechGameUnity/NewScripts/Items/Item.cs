using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum Rarity {Common, Rare, Epic, Legendary}
public enum Rarity {Junk, Shoddy, Inferior, Generic, Calibrated, Flawless, Limited, Unique}

public enum Type { }

public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public int idNum = 0;
    public int price = 1000;
    
    public float ballisticArmor = 0;
    public float energyArmor = 0;
    public float health = 0;
    public float shield = 0;
    public float shieldRegen = 0;
    
    #region Flavor
    public Rarity rarity;
    public string description;
    public string manufacturer;
    #endregion
    
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
