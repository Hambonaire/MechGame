using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region Singleton

    public static InventoryManager instance;
    
    void Awake()
    {
        instance = this;

        //onItemChangedCallback += Save;
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    // Our current list of items in the inventory
    public List<Item> items = new List<Item>();

    // Add a new item
    public void Add(Item item)
    {
        items.Add(item);

        List<int> ids = new List<int>();

        foreach (Item item_ in items)
        {
            ids.Add(item_.idNum);
        }
        
        //SaveData.SetInventoryItems(ids);
        
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    // Remove an item
    public void Remove(Item item)
    {
        items.Remove(item);

        List<int> ids = new List<int>();

        foreach (Item item_ in items)
        {
            ids.Add(item_.idNum);
        }

        //SaveData.SetInventoryItems(ids);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
    
}
