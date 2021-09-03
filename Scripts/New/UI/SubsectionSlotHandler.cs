using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubsectionSlotHandler : MonoBehaviour, IDropHandler
{
	[SerializeField]
	GameObject contentObj;

	[SerializeField]
	GameObject itemButtonPrefab;

	//[HideInInspector]
	public List<Item> subsectionItems = new List<Item>();

	[SerializeField]
	List<GameObject> subsectionSlotIcon = new List<GameObject>();
	
    public void OnDrop(PointerEventData eventData)
    {
		if (eventData.pointerDrag != null)
		{	
			OnItemDrop(eventData);
		}
    }

	public void Build()
	{
		subsectionItems.Clear();

		foreach (Transform child in contentObj.transform)
			Destroy(child.gameObject);

		for (int index = 0; index < subsectionSlotIcon.Count; index++)
        {
			subsectionSlotIcon[index].SetActive(false);

            if (HangarManager._instance.currentylSelectedSectionIndex == -1)
                continue;

            if (index < GameManager._instance.availableMechs[HangarManager._instance.currentlySelectedMechIndex].GetSubsectionCountByIndex(index))
            {
				subsectionSlotIcon[index].SetActive(true);

				if (GameManager._instance.availableMechs[HangarManager._instance.currentlySelectedMechIndex].GetSectionItemsByIndex(HangarManager._instance.currentylSelectedSectionIndex)[index] != null)
				{
					var newButton = Instantiate(itemButtonPrefab, contentObj.transform);

					subsectionItems.Add(GameManager._instance.availableMechs[HangarManager._instance.currentlySelectedMechIndex].GetSectionItemsByIndex(HangarManager._instance.currentylSelectedSectionIndex)[index]);

					newButton.GetComponent<MechItemButton>().Initialize(GameManager._instance.availableMechs[HangarManager._instance.currentlySelectedMechIndex].GetSectionItemsByIndex(HangarManager._instance.currentylSelectedSectionIndex)[index]);
				}
			}

        }
	}

	public void ParseItemsFromButtons()
    {
		subsectionItems.Clear();

		foreach (Transform child in contentObj.transform)
		{
			subsectionItems.Add(child.GetComponent<MechItemButton>().myItem);
		}

	}

	public void OnItemDrop(PointerEventData eventData)
	{
		if (subsectionItems.Count < GameManager._instance.availableMechs[HangarManager._instance.currentlySelectedMechIndex].GetSubsectionCountByIndex(HangarManager._instance.currentylSelectedSectionIndex) && eventData.pointerDrag.GetComponent<MechItemButton>() != null)
        {
			subsectionItems.Add(eventData.pointerDrag.GetComponent<MechItemButton>().myItem);

			eventData.pointerDrag.transform.parent = contentObj.transform;
			eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
		}

	}
}
