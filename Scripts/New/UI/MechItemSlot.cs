using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubsectionSlotHandler : MonoBehaviour, IDropHandler
{
	public GameObject itemButtonPrefab;
	
	public List<GameObject> subsectionSlots = new List<GameObject>();
	
    public void OnDrop(PointerEventData eventData)
    {
		if (eventData.pointerDrag != null)
		{
			eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
			
			OnItemDrop();
		}
        Debug.Log("Dropped On");
    }

	public void Build()
	{
		for (int index = 0; index < subsectionSlots.Count; index++)
        {
            subsectionSlots[index].SetActive(false);

            if (HangarManager._instance.currentylSelectedSectionIndex == -1)
                continue;

            if (index < GameManager._instance.availableMechs[HangarManager._instance.currentlySelectedMechIndex].GetSubsectionCountByIndex(index))
            {
                subsectionSlots[index].SetActive(true);

                // Set the stuff here
				var newButton = Instantiate(itemButtonPrefab, subsectionSlots[index].GetComponent<RectTransform>().anchoredPosition);
				newButton.GetComponent<MechItemButton>().Intialize(GameManager._instance.availableMechs[HangarManager._instance.currentlySelectedMechIndex].GetSectionItemsByIndex(HangarManager._instance.currentylSelectedSectionIndex)[index]);
            }

        }
	}

	public void OnItemDrop()
	{
		
	}
}
