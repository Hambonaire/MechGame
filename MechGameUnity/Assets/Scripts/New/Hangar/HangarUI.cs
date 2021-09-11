using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HangarUI : MonoBehaviour
{
    public static HangarUI _instance;

    public Canvas parentCanvas;

    bool isDirty;
    bool[] dirtyFlags = new bool[6] { true, true, true, true, true, true };

    [Header ("Mech Select Buttons")]
    /* List of buttons that select the mech to modify, send signal back to HangarManager */
    public List<GameObject> mechSelectButtons = new List<GameObject>();

    [Header("Mech Section Select Buttons")]
    /* Subsection buttons */
    public List<GameObject> sectionSelectButtons = new List<GameObject>();

    [Header("Mech Subsection Item Buttons")]
    /* Subsection buttons */
    public GameObject subsectionPanel;
    public SubsectionSlotHandler subsectionHandler;

    [Header("Inv Item Buttons")]
    /* Inv item buttons */
    public GameObject inventoryPanel;
    public InvSlotHandler inventoryHandler;

    [Header("Mech Info Panel")]
    /* Mech info panel */
    public GameObject mechInfoPanel;

    [Header("Item Info Panel")]
    /* Item info panel */
    public GameObject itemInfoPanel;
	public TextMeshProUGUI itemPnlName;
    public Image itemPnlIcon;
	
    [Header("Shop")]
    /* Shop panel */
    public GameObject shopPanel;
    public GameObject shopItemBtnParent;
    public GameObject shopItemBtnPrefab;
    public GameObject shopTabBtnParent;
    public GameObject shopTabBtnPrefab;
    
    public GameObject shopBuyBtn;
    public GameObject shopSellBtn;
    
    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        BuildUI();   
    }

    void LateUpdate()
    {
        if (isDirty)
        {
            BuildUI();
            isDirty = false;
            dirtyFlags = new bool[6] { false, false, false, false, false, false };
        }
    }

    /*
     *  1: Select Buttons
     *  2: Mech Info Panel
     *  3: Item Info Panel
     *  4: Shop
     */
    public void MakeDirty(bool mechSelectButtons, bool subsectionItems, bool mechInfoPanel, bool itemInfoPanel, bool shop, bool inventory)
    {
        isDirty = true;

        if (mechSelectButtons)
            dirtyFlags[0] = true;
        if (subsectionItems)
            dirtyFlags[1] = true; 
        if (mechInfoPanel)
            dirtyFlags[2] = true;
        if (itemInfoPanel)
            dirtyFlags[3] = true;
        if (shop)
            dirtyFlags[4] = true;
        if (inventory)
            dirtyFlags[5] = true;
    }

    /*  Main build function
     *  Check what subsections to rebuild and only build those
     */
    void BuildUI()
    {
        /* Build the mech select buttons */
        if (dirtyFlags[0])
        {
            BuildMechSelectButtons();
        }

        /* Build the subsection items */
        if (dirtyFlags[1])
        {
            BuildSubectionItemUI();
        }

        /* Build the mech info panel (LEFT) */
        if (dirtyFlags[2])
        {
            BuildMechInfoPanel();
        }

        /* Build the item info panel (RIGHT) */
        if (dirtyFlags[3])
        {
            BuildItemInfoPanel();
        }

        /* Build the shop panel */
        if (dirtyFlags[4])
        {
            BuildShopPanel();
        }

        if (dirtyFlags[5])
        {
            BuildInventoryItemUI();
        }
    }

    public void EnableUI(bool subsectionPanelB, bool shopPanelB, bool inventoryPanelB)
    {
        subsectionPanel.SetActive(subsectionPanelB);
        shopPanel.SetActive(shopPanelB);
        inventoryPanel.SetActive(inventoryPanelB);
    }

    /* Build the mech select buttons (top?) of the screen */
    void BuildMechSelectButtons()
    {
        /* Disable buttons based on available mechs */
        for (int index = 0; index < 4; index++)
        {
            mechSelectButtons[index].SetActive(true);

            if (HangarManager._instance.currentlySelectedMechIndex == index)
                mechSelectButtons[index].GetComponent<ButtonGlow>().SetGlowActive(true);
            else
                mechSelectButtons[index].GetComponent<ButtonGlow>().SetGlowActive(false);
        }
    }

    /* Build the mech select buttons (top?) of the screen */
    void BuildSectionSelectButtons()
    {
        
    }

    /* Build the mech select buttons (top?) of the screen */
    void BuildSubectionItemUI()
    {
        subsectionHandler.BuildClean();
    }

    /* Build the inventory list + buttons */
    void BuildInventoryItemUI()
    {
        inventoryHandler.BuildClean();
    }

    /* Build the mech info panel (left?) of the screen */
    void BuildMechInfoPanel()
    {

    }

    /* Build the item info panel (right?) of the screen */
    void BuildItemInfoPanel()
    {
		Item item = HangarManager._instance.currentlySelectedMechItem;

        if (item == null)
            return;

		itemPnlName.text = item.name;
		//itemPnlIcon.sprite = item.icon;
    }

    /* Build the shop panel */
    void BuildShopPanel()
    {

    }

    public void OnItemButtonChange()
    {
        // TODO: Need this until implement remove item from handler on PICKUP
        //inventoryHandler.ParseItemsFromButtons();
        //subsectionHandler.ParseItemsFromButtons();
        inventoryHandler.BuildFromItemList();
        subsectionHandler.BuildFromItemList();

        // Save stuff
        // Modify the mech.cs
        GameManager._instance.availableMechs[HangarManager._instance.currentlySelectedMechIndex].SetSectionItemsByIndex(HangarManager._instance.currentylSelectedSectionIndex, subsectionHandler.GetItemsAsList());

        HangarManager._instance.RebuildMechCurrentIndex();

        // Modify the inventory
        // Save here..?
    }

}
