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
     *  When a button is picked up remove itself preemptively from the handler, and set parent to main canvas and move to front
     */
    public void OnBeginDrag(PointerEventData eventData)
    {
		cGroup.blocksRaycasts = false;

        myHandler.RemoveItemFromList(myItem, 1);

        lastParent = transform.parent;

        transform.parent = canvas.gameObject.transform;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    /* 
     *  For moving the button back if not dropped on valid container
     *
     *  This occurs AFTER onDragDrop so the handler will have already set itself as this button's parent
     *  if the handler has space and is the correct type etc.
     */ 
    public void OnEndDrag(PointerEventData eventData)
    {
		cGroup.blocksRaycasts = true;

        // Defaulting back to old parent
        if (transform.parent == canvas.transform)
        {
            transform.parent = lastParent;

            // Return the item back to the handler how it would like (no dupes for inv)
            myHandler.AddItemToList(myItem, 1);
        }
        
        HangarUI._instance.OnItemButtonChange();       
    }

    public void ChangeHandler(ItemSlotHandler newHandler)
    {
        myHandler = newHandler;
    }
}
