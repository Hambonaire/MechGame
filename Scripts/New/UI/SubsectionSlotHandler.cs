using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubsectionSlotHandler : ItemSlotHandler, IDropHandler
{
	[SerializeField]
	List<GameObject> subsectionSlotIcon = new List<GameObject>();
	
    public void OnDrop(PointerEventData eventData)
    {
		if (eventData.pointerDrag != null)
		{	
			OnItemDrop(eventData);
		}
    }

	public new void BuildClean()
	{
        base.BuildClean();

        /* Enable the subsection slot icons */
        for (int index = 0; index < subsectionSlotIcon.Count; index++)
        {
            subsectionSlotIcon[index].SetActive(false);

            if (HangarManager._instance.currentylSelectedSectionIndex == -1)
                continue;

            if (index < GameManager._instance.availableMechs[HangarManager._instance.currentlySelectedMechIndex].GetSubsectionCountByIndex(index))
            {

                if (GameManager._instance.availableMechs[HangarManager._instance.currentlySelectedMechIndex].GetSectionItemsByIndex(HangarManager._instance.currentylSelectedSectionIndex)[index] != null)
                {
                    var newButton = Instantiate(itemButtonPrefab, contentObj.transform);

                    handlerItems.Add(new ListItem(1, 
                        GameManager._instance.availableMechs[HangarManager._instance.currentlySelectedMechIndex].GetSectionItemsByIndex(HangarManager._instance.currentylSelectedSectionIndex)[index]));

                    newButton.GetComponent<MechItemButton>().Initialize(GameManager._instance.availableMechs[HangarManager._instance.currentlySelectedMechIndex].GetSectionItemsByIndex(HangarManager._instance.currentylSelectedSectionIndex)[index], 
                        1, this as ItemSlotHandler);
                }
            }

        }

    }

    public void OnItemDrop(PointerEventData eventData)
	{
        // Section has space and its a mechitembutton
		if (handlerItems.Count < GameManager._instance.availableMechs[HangarManager._instance.currentlySelectedMechIndex].GetSubsectionCountByIndex(HangarManager._instance.currentylSelectedSectionIndex) && eventData.pointerDrag.GetComponent<MechItemButton>() != null)
        {
            eventData.pointerDrag.GetComponent<MechItemButton>().ChangeHandler(this as ItemSlotHandler);

            handlerItems.Add(new ListItem(1, eventData.pointerDrag.GetComponent<MechItemButton>().myItem));

            eventData.pointerDrag.transform.parent = contentObj.transform;
            //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            //BuildFromItemList();
		}

	}

    public new void AddItemToList(Item newItem, int count)
    {
        handlerItems.Add(new ListItem(count, newItem));
    }

    public List<Item> GetItemsAsList()
    {
        List<Item> returnList = new List<Item>();

        foreach (ListItem listItem in handlerItems)
            returnList.Add(listItem.item);

        return returnList;
    }
}
