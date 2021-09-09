using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MechItemButton : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField]
    ItemSlotHandler myHandler;

    Canvas canvas;
    RectTransform rectTransform;
	CanvasGroup cGroup;

    Transform lastParent;

	public Item myItem;
    public int count;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI countText;
    public Image iconSprite;
    //public TextMeshProUGUI 

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        cGroup = GetComponent<CanvasGroup>();

        canvas = HangarUI._instance.parentCanvas;
    }

	public void Initialize(Item newItem, int count, ItemSlotHandler handler)
	{
        myHandler = handler;

        if (newItem != null)
        {
            myItem = newItem;
            this.count = count;
        }

        if (myItem != null)
        {
            nameText.text = myItem.name;
            if (count > 1)
                countText.text = "x" + count;
            else
                countText.text = "";
        }
    }

    /* 
     *  When a button is in the inventory section and picked up
     *  - 
     * 
     */
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("On Begin Drag");
		cGroup.blocksRaycasts = false;

        myHandler.RemoveItemFromList(myItem, 1);

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

            // Add the item back in the manner that the handler would like (no dupes for inv)
            myHandler.AddItemToList(myItem, 1);
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

    public void ChangeHandler(ItemSlotHandler newHandler)
    {
        myHandler = newHandler;
    }
}
