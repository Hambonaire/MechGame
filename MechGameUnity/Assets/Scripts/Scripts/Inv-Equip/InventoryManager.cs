using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    // Our current list of items in the inventory
    //[HideInInspector]
    public List<Item> items = new List<Item>();
    
    public Database database;

    public Item DEFAULT_TEST;

    #region Singleton

    public static InventoryManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<InventoryManager>();
            }
            return _instance;
        }
    }
    static InventoryManager _instance;

    #endregion

    void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
           Destroy(this);
        }

        //onItemChangedCallback += Save;
    }

    void Start()
    {
        //EquipmentManager.OnEquipmentChanged += Add();

        //BuildFromSave();

        Add(DEFAULT_TEST);
    }

    public void BuildFromSave()
    {
        SaveData save = SaveService.instance.Load();//= SaveData.Load();

        foreach (int i in save.inventoryItems)
        {
            Item it = database.GetActual(i);

            Add(it);
        }

        //Rebuild();
    }

    // Add a new item
    public void Add(Item item)
    {
        if (item != null) items.Add(item);

        //List<int> ids = new List<int>();

        /*
        foreach (Item item_ in items)
        {
            if (item != null)
                ids.Add(item_.idNum);
        }
        */

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

    public bool Contains(Item item)
    {
        if (items.Contains(item))
        {
            print("Inv contains " + item.name);
            return true;
        }
        return false;
    }

    public void OnEquipmentChanged(Item newItem, Item OldItem)
    {

    }

}
