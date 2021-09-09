using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvSlotHandler : ItemSlotHandler, IDropHandler
{
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
            //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            //BuildFromItemList();
        }

    }

    public override void BuildClean()
	{
        base.BuildClean();

		for (int index = 0; index < handlerItems.Count; index++)
		{
			var newButton = Instantiate(itemButtonPrefab, contentObj.transform);
			newButton.GetComponent<MechItemButton>().Initialize(handlerItems[index].item, handlerItems[index].count, this as ItemSlotHandler);

			//inventoryItems.Add(inventoryItems[index]);
		}
	}

    public override void ParseItemsFromButtons()
    {
        handlerItems.Clear();

        foreach (Transform child in contentObj.transform)
        {
            AddItemToList(child.GetComponent<MechItemButton>().myItem, child.GetComponent<MechItemButton>().count);
        }

        BuildFromItemList();

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
