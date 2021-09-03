using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MechItemButton : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    Canvas canvas;
    RectTransform rectTransform;
	CanvasGroup cGroup;

    Transform lastParent;

	public Item myItem;

    public TextMeshProUGUI nameText;
    //public TextMeshProUGUI 

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        cGroup = GetComponent<CanvasGroup>();

        canvas = HangarUI._instance.parentCanvas;

        Initialize(null);
    }

	public void Initialize(Item newItem)
	{
        if (newItem != null)
		    myItem = newItem;
        
        if (myItem != null)
            nameText.text = myItem.name;
	}

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("On Begin Drag");
		cGroup.blocksRaycasts = false;

        lastParent = transform.parent;

        transform.parent = canvas.gameObject.transform;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("On Drag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("On End Drag");
		cGroup.blocksRaycasts = true;

        if (transform.parent == canvas.transform)
        {
            //Debug.Log("Not in a handler");
            transform.parent = lastParent;

        }

        HangarUI._instance.OnItemButtonChange();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("On M Down");
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("On Drop");
    }
}
