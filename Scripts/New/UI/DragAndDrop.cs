using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MechItemButton : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public Canvas canvas;
    RectTransform rectTransform;
	CanvasGroup cGroup;
	
	Item myItem;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        cGroup = GetComponent<CanvasGroup>();
    }

	public void Initialize(Item newItem)
	{
		myItem = newItem;
	}

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("On Begin Drag");
		cGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("On Drag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("On End Drag");
		cGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("On M Down");
    }

}
