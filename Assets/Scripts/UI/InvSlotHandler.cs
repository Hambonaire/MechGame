﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvSlotHandler : ItemSlotHandler, IDropHandler
{
    public override void Start()
    {
        base.Start();

        handlerItems = Inventory._instance.playerInventory;
    }

	public void OnDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag != null)
		{
			OnItemDrop(eventData);
		}
	}

    public void OnItemDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<MechItemButton>() != null)
        {
            AddItemToList(eventData.pointerDrag.GetComponent<MechItemButton>().myItem, eventData.pointerDrag.GetComponent<MechItemButton>().count);

            eventData.pointerDrag.transform.parent = contentObj.transform;
        }

    }

    public override void BuildClean()
	{
        //base.BuildClean();
        foreach (Transform child in contentObj.transform)
            Destroy(child.gameObject);

        for (int index = 0; index < handlerItems.Count; index++)
		{
			var newButton = Instantiate(itemButtonPrefab, contentObj.transform);
			newButton.GetComponent<MechItemButton>().Initialize(handlerItems[index].item, handlerItems[index].count, this as ItemSlotHandler);
		}
	}

    public override void ParseItemsFromButtons()
    {
        /*
        handlerItems.Clear();

        foreach (Transform child in contentObj.transform)
        {
            AddItemToList(child.GetComponent<MechItemButton>().myItem, child.GetComponent<MechItemButton>().count);
        }

        BuildFromItemList();
        */
    }

    public override bool AddItemToList(Item newItem, int count)
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
}
