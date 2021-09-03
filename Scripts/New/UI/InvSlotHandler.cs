using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvSlotHandler : MonoBehaviour, IDropHandler
{
	[SerializeField]
	GameObject contentObj;

	[SerializeField]
	GameObject itemButtonPrefab;

	[HideInInspector]
	List<Item> inventoryItems = new List<Item>();

	public void OnDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag != null)
		{
			OnItemDrop(eventData);
		}
	}

	public void Build()
	{
		inventoryItems.Clear();

		foreach (Transform child in contentObj.transform)
			Destroy(child.gameObject);

		for (int index = 0; index < inventoryItems.Count; index++)
		{
			var newButton = Instantiate(itemButtonPrefab, contentObj.transform);
			newButton.GetComponent<MechItemButton>().Initialize(inventoryItems[index]);

			inventoryItems.Add(inventoryItems[index]);
		}
	}

	public void ParseItemsFromButtons()
	{
		inventoryItems.Clear();

		foreach (Transform child in contentObj.transform)
		{
			inventoryItems.Add(child.GetComponent<MechItemButton>().myItem);
		}

	}

	public void OnItemDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag.GetComponent<MechItemButton>() != null)
		{
			inventoryItems.Add(eventData.pointerDrag.GetComponent<MechItemButton>().myItem);

			eventData.pointerDrag.transform.parent = contentObj.transform;
			eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
		}

	}
}
