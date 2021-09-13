using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MechItemButton : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    [SerializeField]
    GameObject selectGlow;

    private ItemSlotHandler myHandler;

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup cGroup;

    private Transform lastParent;

	public Item myItem;
    public int count;

    [SerializeField]
    TextMeshProUGUI nameText;
    [SerializeField]
    TextMeshProUGUI countText;
    [SerializeField]
    Image iconSprite;

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

            
            if (myItem is WeaponItem)
            {
                WeaponItem item = myItem as WeaponItem;

                if (item.weaponClass == WeaponClass.Primary)
                    GetComponent<Image>().color = Utilities.PRIMARY_ITEM_COLOR;
                if (item.weaponClass == WeaponClass.Secondary)
                    GetComponent<Image>().color = Utilities.SECONDARY_ITEM_COLOR;
                if (item.weaponClass == WeaponClass.Tertiary)
                    GetComponent<Image>().color = Utilities.TERTIARY_ITEM_COLOR;
            }
            else if (myItem is Accessory)
            {
                Accessory item = myItem as Accessory;

                GetComponent<Image>().color = Utilities.UPGRADE_ITEM_COLOR;
            }
            
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

    public void OnPointerClick(PointerEventData eventData)
    {
        Select(true);

        HangarManager._instance.SelectMechItem(this);
    }

    public void Select(bool val)
    {
        selectGlow.SetActive(val);
    }
}
