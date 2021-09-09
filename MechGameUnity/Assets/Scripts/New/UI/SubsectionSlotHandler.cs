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
    public override void BuildClean()
	{
        base.BuildClean();

        if (HangarManager._instance.currentylSelectedSectionIndex == -1)
            return;

        subsectionItems = GameManager._instance.availableMechs[hangarManager.currentlySelectedMechIndex].GetSectionItemsByIndex(hangarManager.currentylSelectedSectionIndex);

        BuildFromStruct();

        /* Enable the subsection slot icons */
        for (int index = 0; index < subsectionSlotIcon.Count; index++)
        {
            subsectionSlotIcon[index].SetActive(false);

            if (HangarManager._instance.currentylSelectedSectionIndex == -1)
                continue;
        }

    }

    public override void BuildFromItemList()
    {
        foreach (Transform child in contentObj.transform)
            Destroy(child.gameObject);

        BuildFromStruct();
    }

    public override bool RemoveItemFromList(Item newItem, int count)
    {
        print("SubSlotHand Remove");

        if (newItem is WeaponItem)
        {
            WeaponItem newWeapon = newItem as WeaponItem;
            WeaponItem[] classArray = new WeaponItem[0];

            if (newWeapon.weaponClass == WeaponClass.Primary)
                classArray = subsectionItems.primary;
            else if (newWeapon.weaponClass == WeaponClass.Secondary)
                classArray = subsectionItems.secondary;
            else if (newWeapon.weaponClass == WeaponClass.Tertiary)
                classArray = subsectionItems.tertiary;

            for (int index = 0; index < classArray.Length; index++)
            {
                if (classArray[index] == newWeapon)
                {
                    classArray[index] = null;
                    print("Found removed inv");
                    return true;
                }
            }
        }
        else
        {

        }

        return false;
    }

    public override bool AddItemToList(Item newItem, int count)
    {
        if (newItem is WeaponItem)
        {
            WeaponItem newWeapon = newItem as WeaponItem;
            WeaponItem[] classArray = new WeaponItem[0];

            if (newWeapon.weaponClass == WeaponClass.Primary)
                classArray = subsectionItems.primary;
            if (newWeapon.weaponClass == WeaponClass.Secondary)
                classArray = subsectionItems.secondary;
            if (newWeapon.weaponClass == WeaponClass.Tertiary)
                classArray = subsectionItems.tertiary;

            if (classArray.Length == 0)
                return false;

            for (int index = 0; index < classArray.Length; index++)
            {
                if (classArray[index] == null)
                {
                    classArray[index] = newWeapon;
                    return true;
                }
            }
        }
        else
        {
            
        }

        return false;
    }

    public ItemStruct GetItemsAsList()
    {
        return subsectionItems;
    }

    void BuildFromStruct()
    {
        foreach(WeaponItem wItem in subsectionItems.primary)
        {
            if (wItem == null)
                return;

            var newButton = Instantiate(itemButtonPrefab, contentObj.transform);

            //handlerItems.Add(new ListItem(1, GameManager._instance.availableMechs[hangarManager.currentlySelectedMechIndex].GetSectionItemsByIndex(hangarManager.currentylSelectedSectionIndex)[index]));

            newButton.GetComponent<MechItemButton>().Initialize(wItem, 1, this as ItemSlotHandler);
        }

        foreach(WeaponItem wItem in subsectionItems.secondary)
        {
            if (wItem == null)
                return;

            var newButton = Instantiate(itemButtonPrefab, contentObj.transform);

            //handlerItems.Add(new ListItem(1, GameManager._instance.availableMechs[hangarManager.currentlySelectedMechIndex].GetSectionItemsByIndex(hangarManager.currentylSelectedSectionIndex)[index]));

            newButton.GetComponent<MechItemButton>().Initialize(wItem, 1, this as ItemSlotHandler);
        }

        foreach(WeaponItem wItem in subsectionItems.tertiary)
        {
            if (wItem == null)
                return;

            var newButton = Instantiate(itemButtonPrefab, contentObj.transform);

            //handlerItems.Add(new ListItem(1, GameManager._instance.availableMechs[hangarManager.currentlySelectedMechIndex].GetSectionItemsByIndex(hangarManager.currentylSelectedSectionIndex)[index]));

            newButton.GetComponent<MechItemButton>().Initialize(wItem, 1, this as ItemSlotHandler);
        }

        foreach(Accessory aItem in subsectionItems.upgrades)
        {
            if (aItem == null)
                return;

            var newButton = Instantiate(itemButtonPrefab, contentObj.transform);

            //handlerItems.Add(new ListItem(1, GameManager._instance.availableMechs[hangarManager.currentlySelectedMechIndex].GetSectionItemsByIndex(hangarManager.currentylSelectedSectionIndex)[index]));

            newButton.GetComponent<MechItemButton>().Initialize(aItem, 1, this as ItemSlotHandler);             
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
