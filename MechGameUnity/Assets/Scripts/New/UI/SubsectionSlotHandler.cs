using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubsectionSlotHandler : ItemSlotHandler, IDropHandler
{
	[SerializeField]
	List<GameObject> subsectionSlotIcon = new List<GameObject>();

    ItemStruct subsectionItems;
	
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

        subsectionItems = GameManager._instance.availableMechs[hangarManager.currentlySelectedMechIndex].GetSectionItemsByIndex(hangarManager.currentylSelectedSectionIndex);

        /* Enable the subsection slot icons */
        for (int index = 0; index < subsectionSlotIcon.Count; index++)
        {
            subsectionSlotIcon[index].SetActive(false);

            if (HangarManager._instance.currentylSelectedSectionIndex == -1)
                continue;
        }

        BuildFromStruct();

    }

    public void OnItemDrop(PointerEventData eventData)
	{
        // Section has space and its a mechitembutton
		if (eventData.pointerDrag.GetComponent<MechItemButton>() != null)
        {
            if (AddItemToList(eventData.pointerDrag.GetComponent<MechItemButton>().myItem, 1))
            {
                eventData.pointerDrag.GetComponent<MechItemButton>().ChangeHandler(this as ItemSlotHandler);

                //handlerItems.Add(new ListItem(1, eventData.pointerDrag.GetComponent<MechItemButton>().myItem));

                eventData.pointerDrag.transform.parent = contentObj.transform;
            }
            else
            {

            }
			
            //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            //BuildFromItemList();
		}

	}

    public new bool RemoveItemFromList(Item newItem, int count)
    {   
        if (newItem is WeaponItem)
        {
            WeaponItem newWeapon = newItem as WeaponItem;
            WeaponItem[] classArray;

            if (newWeapon.WeaponClass == WeaponClass.primary)
                classArray = subsectionItems.primary;
            if (newWeapon.WeaponClass == WeaponClass.secondary)
                classArray = subsectionItems.secondary;
            if (newWeapon.WeaponClass == WeaponClass.tertiary)
                classArray = subsectionItems.tertiary;

            for (int index = 0; index < classArray.Length; index++)
            {
                if (classArray[index].Equals(newWeapon))
                    classArray[index] = null;
            }
        }
        else
        {
            
        }
    }

    public new bool AddItemToList(Item newItem, int count)
    {
        if (newItem is WeaponItem)
        {
            WeaponItem newWeapon = newItem as WeaponItem;
            WeaponItem[] classArray;

            if (newWeapon.WeaponClass == WeaponClass.primary)
                classArray = subsectionItems.primary;
            if (newWeapon.WeaponClass == WeaponClass.secondary)
                classArray = subsectionItems.secondary;
            if (newWeapon.WeaponClass == WeaponClass.tertiary)
                classArray = subsectionItems.tertiary;

            for (int index = 0; index < classArray.Length; index++)
            {
                if (classArray[index] == null)
                    classArray[index] = newWeapon;
            }
        }
        else
        {
            
        }
    }

    public ItemStruct GetItemsAsList()
    {
        return subsectionItems;
    }

    void BuildFromStruct()
    {
        foreach(WeaponItem wItem in subsectionItems.primary)
        {
            var newButton = Instantiate(itemButtonPrefab, contentObj.transform);

            //handlerItems.Add(new ListItem(1, GameManager._instance.availableMechs[hangarManager.currentlySelectedMechIndex].GetSectionItemsByIndex(hangarManager.currentylSelectedSectionIndex)[index]));

            newButton.GetComponent<MechItemButton>().Initialize(wItem, 1, this as ItemSlotHandler);
        }

        foreach(WeaponItem wItem in subsectionItems.secondary)
        {
            var newButton = Instantiate(itemButtonPrefab, contentObj.transform);

            //handlerItems.Add(new ListItem(1, GameManager._instance.availableMechs[hangarManager.currentlySelectedMechIndex].GetSectionItemsByIndex(hangarManager.currentylSelectedSectionIndex)[index]));

            newButton.GetComponent<MechItemButton>().Initialize(wItem, 1, this as ItemSlotHandler);
        }

        foreach(WeaponItem wItem in subsectionItems.tertiary)
        {
            var newButton = Instantiate(itemButtonPrefab, contentObj.transform);

            //handlerItems.Add(new ListItem(1, GameManager._instance.availableMechs[hangarManager.currentlySelectedMechIndex].GetSectionItemsByIndex(hangarManager.currentylSelectedSectionIndex)[index]));

            newButton.GetComponent<MechItemButton>().Initialize(wItem, 1, this as ItemSlotHandler);
        }

        foreach(Accessory aItem in subsectionItems.accessories)
        {
            var newButton = Instantiate(itemButtonPrefab, contentObj.transform);

            //handlerItems.Add(new ListItem(1, GameManager._instance.availableMechs[hangarManager.currentlySelectedMechIndex].GetSectionItemsByIndex(hangarManager.currentylSelectedSectionIndex)[index]));

            newButton.GetComponent<MechItemButton>().Initialize(wItem, 1, this as ItemSlotHandler);             
        }

        /*
        if (index < gameManager.availableMechs[hangarManager.currentlySelectedMechIndex].GetSubsectionCountByIndex(index))
        {

            if (gameManager.availableMechs[hangarManager.currentlySelectedMechIndex].GetSectionItemsByIndex(hangarManager.currentylSelectedSectionIndex)[index] != null)
            {
                var newButton = Instantiate(itemButtonPrefab, contentObj.transform);

                handlerItems.Add(new ListItem(1, 
                    GameManager._instance.availableMechs[hangarManager.currentlySelectedMechIndex].GetSectionItemsByIndex(hangarManager.currentylSelectedSectionIndex)[index]));

                newButton.GetComponent<MechItemButton>().Initialize(gameManager.availableMechs[hangarManager.currentlySelectedMechIndex].GetSectionItemsByIndex(hangarManager.currentylSelectedSectionIndex)[index], 
                    1, this as ItemSlotHandler);
            }
        }
        */
    }

}
