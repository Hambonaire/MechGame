using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubsectionSlotHandler : ItemSlotHandler, IDropHandler
{
    GameObject upgradeContent;
    GameObject primaryContent;
    GameObject SecondaryContent;
    GameObject tertiaryContent;


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

                SetItemButtonParent(eventData.pointerDrag.GetComponent<MechItemButton>());
            }
            else
            {
                // Do nothing..? Button itself takes care of resetting it's parent
            }
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
        foreach (Transform child in upgradeContent.transform)
            Destroy(child.gameObject);
        foreach (Transform child in primaryContent.transform)
            Destroy(child.gameObject);
        foreach (Transform child in SecondaryContent.transform)
            Destroy(child.gameObject);
        foreach (Transform child in tertiaryContent.transform)
            Destroy(child.gameObject);

        BuildFromStruct();
    }

    public override bool RemoveItemFromList(Item newItem, int count)
    {
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
                    return true;
                }
            }
        }
        else if (newItem is Accessory)
        {
            Accessory newAccessory = newItem as Accessory;

            for (int index = 0; index < subsectionItems.upgrades.Length; index++)
            {
                if (subsectionItems.upgrades[index] == newAccessory)
                {
                    subsectionItems.upgrades[index] = null;
                    return true;
                }
            }
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
        else if (newItem is Accessory)
        {
            Accessory newAccessory = newItem as Accessory;

            if (subsectionItems.upgrades.Length == 0)
                return false;

            for (int index = 0; index < subsectionItems.upgrades.Length; index++)
            {
                if (subsectionItems.upgrades[index] == null)
                {
                    subsectionItems.upgrades[index] = newAccessory;
                    return true;
                }
            }
        }

        return false;
    }

    public ItemStruct GetItemsAsList()
    {
        return subsectionItems;
    }

    private void BuildFromStruct()
    {
        foreach(WeaponItem wItem in subsectionItems.primary)
        {
            if (wItem == null)
                return;
            var newButton = Instantiate(itemButtonPrefab, primaryContent.transform);
            newButton.GetComponent<MechItemButton>().Initialize(wItem, 1, this as ItemSlotHandler);
        }

        foreach(WeaponItem wItem in subsectionItems.secondary)
        {
            if (wItem == null)
                return;
            var newButton = Instantiate(itemButtonPrefab, SecondaryContent.transform);
            newButton.GetComponent<MechItemButton>().Initialize(wItem, 1, this as ItemSlotHandler);
        }

        foreach(WeaponItem wItem in subsectionItems.tertiary)
        {
            if (wItem == null)
                return;
            var newButton = Instantiate(itemButtonPrefab, tertiaryContent.transform);
            newButton.GetComponent<MechItemButton>().Initialize(wItem, 1, this as ItemSlotHandler);
        }

        foreach(Accessory aItem in subsectionItems.upgrades)
        {
            if (aItem == null)
                return;
            var newButton = Instantiate(itemButtonPrefab, upgradeContent.transform);
            newButton.GetComponent<MechItemButton>().Initialize(aItem, 1, this as ItemSlotHandler);             
        }
    }

    private void SetItemButtonParent(MechItemButton button)
    {
        if (button.myItem is WeaponItem)
        {
            WeaponItem newWeapon = button.myItem as WeaponItem;

            if (newWeapon.weaponClass == WeaponClass.Primary)
                button.transform.parent = primaryContent.transform;
            if (newWeapon.weaponClass == WeaponClass.Secondary)
                button.transform.parent = SecondaryContent.transform;
            if (newWeapon.weaponClass == WeaponClass.Tertiary)
                button.transform.parent = tertiaryContent.transform;
        }
        else if (button.myItem is Accessory)
        {
            button.transform.parent = upgradeContent.transform;
        }
    }
}
