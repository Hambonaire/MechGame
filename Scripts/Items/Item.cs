using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum Rarity {Common, Rare, Epic, Legendary}
public enum Rarity {Junk, Shoddy, Inferior, Generic, Calibrated, Flawless, Limited, Unique}

public enum Type { }

[System.Serializable]
public class Item : ScriptableObject
{
    [Header("General")]
    new public string name = "New Item";
    public Sprite icon = null;
    [Range(1, 5)]
    public int tier = 1;
    public int price = 1000;
    
    public float bonusArmor = 0;
    public float bonusIntegrity = 0;
    public float bonusShield = 0;
    public float bonusShieldRegen = 0;
    
    #region Flavor
    [Header("Flavor")]
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
