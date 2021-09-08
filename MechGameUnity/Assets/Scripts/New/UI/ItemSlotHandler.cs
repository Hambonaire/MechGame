﻿using System.Collections;
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

public class ItemSlotHandler : MonoBehaviour
{
    HangarManager hangarManager;
    GameManager gameManager;

    [SerializeField]
    public List<ListItem> handlerItems = new List<ListItem>();

    public GameObject contentObj;

    public GameObject itemButtonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager._instance;
        hangarManager = HangerManager._instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildClean()
    {
        handlerItems.Clear();

        foreach (Transform child in contentObj.transform)
            Destroy(child.gameObject);
    }

    public void BuildFromItemList()
    {
        foreach (Transform child in contentObj.transform)
            Destroy(child.gameObject);

        foreach (ListItem item in handlerItems)
        {
            var newButton = Instantiate(itemButtonPrefab, contentObj.transform);

            newButton.GetComponent<MechItemButton>().Initialize(item.item, item.count, this as ItemSlotHandler);
        }
    }

    public void ParseItemsFromButtons()
    {
        handlerItems.Clear();

        foreach (Transform child in contentObj.transform)
        {
            handlerItems.Add(new ListItem(child.GetComponent<MechItemButton>().myItem, child.GetComponent<MechItemButton>().count));
        }

    }

    public bool AddItemToList(Item newItem, int count)
    {
        var existingItem = handlerItems.Find(x => x.item.Equals(newItem));

        if (existingItem != null)
        {
            existingItem.count += count;
            return true;
        }
        else
        {
            handlerItems.Add(new ListItem(newItem, count));
            return true;
        }
    }

    public bool RemoveItemFromList(Item newItem, int count)
    {
        var existingItem = handlerItems.Find(x => x.item.Equals(newItem));

        if (existingItem != null)
        {
            print("Found -> Removing");

            existingItem.count -= count;

            if (existingItem.count <= 0)
                handlerItems.Remove(existingItem);

            return true;
        }

        return false;
    }

}
