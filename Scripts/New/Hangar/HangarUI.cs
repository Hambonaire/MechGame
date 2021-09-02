using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HangarUI : MonoBehaviour
{
    public static HangarUI _instance;

    bool isDirty;
    bool[] dirtyFlags = new bool[5] { true, true, true, true, true };

    [Header ("Mech Select Buttons")]
    /* List of buttons that select the mech to modify, send signal back to HangarManager */
    public List<GameObject> mechSelectButtons = new List<GameObject>();

    [Header("Mech Section Select Buttons")]
    /* Subsection buttons */
    public List<GameObject> sectionSelectButtons = new List<GameObject>();

    [Header("Mech Subsection Select Buttons")]
    /* Subsection buttons */
    public GameObject subsectionButtonContent;
    public List<GameObject> subsectionSelectButtons = new List<GameObject>();
	public GameObject mechItemButtonPrefab;

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

    void Update()
    {

    }

    void LateUpdate()
    {
        if (isDirty)
        {
            BuildUI();
            isDirty = false;
            dirtyFlags = new bool[5] { false, false, false, false, false };
        }
    }

    /*
     *  1: Select Buttons
     *  2: Mech Info Panel
     *  3: Item Info Panel
     *  4: Shop
     */
    public void MakeDirty(bool mechSelectButtons, bool subsectionSelectButtons, bool mechInfoPanel, bool itemInfoPanel, bool shop)
    {
        isDirty = true;

        if (mechSelectButtons)
            dirtyFlags[0] = true;
        if (subsectionSelectButtons)
            dirtyFlags[1] = true; 
        if (mechInfoPanel)
            dirtyFlags[2] = true;
        if (itemInfoPanel)
            dirtyFlags[3] = true;
        if (shop)
            dirtyFlags[4] = true;
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

        /* Build the subsection buttons */
        if (dirtyFlags[1])
        {
            BuildSubsectionSelectButtons();
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
    void BuildSubsectionSelectButtons()
    {
		/* Destroy old ItemButtons first */
		
		
		for (int index = 0; index < GameManager._instance.availableMechs[HangarManager._instance.currentlySelectedMechIndex].GetSubsectionCountByIndex(index) ; index++)
		{
			var newButton = Instantiate(mechItemButtonPrefab);
			newButton = GetComponent<
		}
		
        /* TOD: Fix for drag and drop and instantiate pref..?
        for (int index = 0; index < 5; index++)
        {


            subsectionSelectButtons[index].SetActive(false);

            if (HangarManager._instance.currentylSelectedSectionIndex == -1)
                continue;

            if (index < GameManager._instance.availableMechs[HangarManager._instance.currentlySelectedMechIndex].GetSubsectionCountByIndex(index))
            {
                subsectionSelectButtons[index].SetActive(true);

                // Set the stuff here
                //subsectionSelectButtons[index].
            }

        }
        */
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

}
