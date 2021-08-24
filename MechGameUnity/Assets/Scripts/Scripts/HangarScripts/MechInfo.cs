using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MechInfo : MonoBehaviour
{
    public static MechInfo _instance;

    InventoryManager invRef;
    EquipmentManager equipRef;
    PrefabHolder prefRef;

    public InfoPanel infoPanelRef;

    //public TextMeshProUGUI 
    //public GameObject buttonPrefab;

    public GameObject LSContentObject;
        [HideInInspector]
        public List<GameObject> LSContents;
    public GameObject LRContentObject;
        [HideInInspector]
        public List<GameObject> LRContents;
    public GameObject RSContentObject;
        [HideInInspector]
        public List<GameObject> RSContents;
    public GameObject RRContentObject;
        [HideInInspector]
        public List<GameObject> RRContents;
    public GameObject AccContentObject;
        [HideInInspector]
        public List<GameObject> ACCContents;

    public GameObject CockpitButton;
    public GameObject LegsButton;
    public GameObject LeftArmButton;
    public GameObject RightArmButton;
    public GameObject LeftShoulderButton;
    public GameObject RightShoulderButton;

    public MechInfoItem currentlySelectedEquipment;

    public int currentlySelectedTypeID = -1;

    void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        invRef = InventoryManager.Instance;
        equipRef = EquipmentManager.Instance;
        prefRef = PrefabHolder.instance;

        //equipRef.onEquipmentChanged += Build;

        //Build(null, null);   

        //EquipmentManager.Instance.onEquipmentChanged += EnableSlotButtons;

        currentlySelectedTypeID = 0;
    }

    public void Select(MechInfoItem mechInfo)
    {
        if (currentlySelectedEquipment != null)
            currentlySelectedEquipment.isSelected = false;
        currentlySelectedEquipment = mechInfo;
    }

    // Used by Mech Slot buttons
    public void SlotSelect(int typeID)
    {
        if (typeID == 0)
        {
            InfoPanel._instance.UpdateDescription(EquipmentManager.Instance.currentCockpit);
            currentlySelectedTypeID = 0;
        }
        if (typeID == 1000) 
        { 
            InfoPanel._instance.UpdateDescription(EquipmentManager.Instance.currentLegs);
            currentlySelectedTypeID = 1000;
        }
        if (typeID == 2000) 
        { 
            InfoPanel._instance.UpdateDescription(EquipmentManager.Instance.currentWeapons[0]);
            currentlySelectedTypeID = 2000;
        }
        if (typeID == 3000) 
        { 
            InfoPanel._instance.UpdateDescription(EquipmentManager.Instance.currentWeapons[1]);
            currentlySelectedTypeID = 3000;
        }
        if (typeID == 4000)
        { 
            InfoPanel._instance.UpdateDescription(EquipmentManager.Instance.currentWeapons[2]);
            currentlySelectedTypeID = 4000;
        }
        if (typeID == 5000) 
        { 
            InfoPanel._instance.UpdateDescription(EquipmentManager.Instance.currentWeapons[3]);
            currentlySelectedTypeID = 5000;
        }

        Shop._instance.Build(currentlySelectedTypeID);
    }

    /*
    public void Build(Item newItem, Item oldItem)
    {
        // Clear and Delete current MechInfos and GameObjects
        LSContents.Clear();
        LRContents.Clear();
        RSContents.Clear();
        RRContents.Clear();
        ACCContents.Clear();

        foreach (Transform child in LSContentObject.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in LRContentObject.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in RSContentObject.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in RRContentObject.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in AccContentObject.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in CockpitButton.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in LegsButton.transform)
        {
            Destroy(child.gameObject);
        }

        #region Create new Buttons

        // Handle Cockpit
        Cockpit cp = equipRef.currentCockpit;
        GameObject newButtonC = Instantiate(prefRef.MechInfoButtonPrefab) as GameObject;
        MechInfoItem mechInfoItemScriptC = newButtonC.GetComponent<MechInfoItem>();
        newButtonC.SetActive(true);
        newButtonC.transform.SetParent(CockpitButton.transform, false);
        mechInfoItemScriptC.SetNewItem(cp);
        mechInfoItemScriptC.mechInfoRef = this;
        mechInfoItemScriptC.infoPanelRef = infoPanelRef;

        // Handle Legs
        Legs lg = equipRef.currentLegs;
        GameObject newButtonL = Instantiate(prefRef.MechInfoButtonPrefab) as GameObject;
        MechInfoItem mechInfoItemScriptL = newButtonL.GetComponent<MechInfoItem>();
        newButtonL.SetActive(true);
        newButtonL.transform.SetParent(LegsButton.transform, false);
        mechInfoItemScriptL.SetNewItem(lg);
        mechInfoItemScriptL.mechInfoRef = this;
        mechInfoItemScriptL.infoPanelRef = infoPanelRef;

        // Handle Weapons
        for (int i = 0; i < equipRef.currentWeapons.Count; i++)
        {
            for (int j = 0; j < equipRef.currentWeapons[i].Count; j++)
            {
                // Buttons that have items
                for (int k = 0; k < equipRef.currentWeapons[i][j].Count; k++)
                {
                    Weapon wep = equipRef.currentWeapons[i][j][k];

                    GameObject newButton = Instantiate(prefRef.MechInfoButtonPrefab) as GameObject;
                    MechInfoItem mechInfoItemScript = newButton.GetComponent<MechInfoItem>();

                    newButton.SetActive(true);

                    mechInfoItemScript.SetNewItem(wep);
                    mechInfoItemScript.mechInfoRef = this;
                    mechInfoItemScript.infoPanelRef = infoPanelRef;
                    mechInfoItemScript.myPosition = new MechInfoItem.EquipmentPosition { a = i, b = j, c = k };

                    if (i == 0)
                    {
                        if (j == 0)
                        {
                            // Add to Left Regulars
                            newButton.transform.SetParent(LRContentObject.transform, false);
                            LRContents.Add(newButton);
                        }
                        else if (j == 1)
                        {
                            // Add to Left UH's
                        }
                        else if (j == 2)
                        {
                            // Add to Left Shoulders
                            newButton.transform.SetParent(LSContentObject.transform, false);
                            LSContents.Add(newButton);
                        }
                    }
                    else if (i == 1)
                    {
                        if (j == 0)
                        {
                            // Add to Right Regulars
                            newButton.transform.SetParent(RRContentObject.transform, false);
                            RRContents.Add(newButton);
                        }
                        else if (j == 1)
                        {
                            // Add to Right Uh's
                        }
                        else if (j == 2)
                        {
                            // Add to Right Shoulders
                            newButton.transform.SetParent(RSContentObject.transform, false);
                            RSContents.Add(newButton);
                        }
                    }
                }             
                // BUttons with no items
                if (equipRef.currentWeapons[i][j].Count < equipRef.currentCockpit.weaponMap[i][j])
                {
                    for (int d = 0; d < equipRef.currentCockpit.weaponMap[i][j] - equipRef.currentWeapons[i][j].Count; d++)
                    {
                        GameObject newButton = Instantiate(prefRef.MechInfoButtonPrefab) as GameObject;
                        MechInfoItem mechInfoItemScript = newButton.GetComponent<MechInfoItem>();

                        newButton.SetActive(true);

                        mechInfoItemScript.SetNewItem(null);
                        mechInfoItemScript.mechInfoRef = this;
                        mechInfoItemScript.infoPanelRef = infoPanelRef;
                        mechInfoItemScript.myPosition = new MechInfoItem.EquipmentPosition { a = i, b = j, c = equipRef.currentWeapons[i][j].Count + d };

                        if (i == 0 && j == 0)
                        {
                            newButton.transform.SetParent(LRContentObject.transform, false);
                            LRContents.Add(newButton);
                        }
                        if (i == 0 && j == 2)
                        {
                            newButton.transform.SetParent(LSContentObject.transform, false);
                            LSContents.Add(newButton);
                        }

                        if (i == 1 && j == 0)
                        {
                            newButton.transform.SetParent(RRContentObject.transform, false);                       
                            RRContents.Add(newButton);
                        }
                        if (i == 1 && j == 2)
                        {
                            newButton.transform.SetParent(RSContentObject.transform, false);
                            RSContents.Add(newButton);
                        }
                    }
                }            
            }
        }

        // Handle Accessories
        // Buttons that have items
        for (int q = 0; q < equipRef.currentAccessories.Count; q++)
        {
            Accessory acc = equipRef.currentAccessories[q];

            GameObject newButton = Instantiate(prefRef.MechInfoButtonPrefab) as GameObject;
            MechInfoItem mechInfoItemScript = newButton.GetComponent<MechInfoItem>();

            newButton.SetActive(true);
            newButton.transform.SetParent(AccContentObject.transform, false);

            mechInfoItemScript.SetNewItem(acc);
            mechInfoItemScript.mechInfoRef = this;
            mechInfoItemScript.infoPanelRef = infoPanelRef;
            mechInfoItemScript.myPosition = new MechInfoItem.EquipmentPosition { a = q, b = 0, c = 0 };

            ACCContents.Add(newButton);
        }
        // Buttons with no items
        if (equipRef.currentAccessories.Count < equipRef.currentCockpit.accessoryCount)
        {
            for (int d = 0; d < equipRef.currentCockpit.accessoryCount - equipRef.currentAccessories.Count; d++)
            {
                GameObject newButton = Instantiate(prefRef.MechInfoButtonPrefab) as GameObject;
                MechInfoItem mechInfoItemScript = newButton.GetComponent<MechInfoItem>();

                newButton.SetActive(true);
                newButton.transform.SetParent(AccContentObject.transform, false);

                mechInfoItemScript.SetNewItem(null);
                mechInfoItemScript.mechInfoRef = this;
                mechInfoItemScript.infoPanelRef = infoPanelRef;
                mechInfoItemScript.myPosition = new MechInfoItem.EquipmentPosition { a = equipRef.currentAccessories.Count + d, b = 0, c = 0 };

                ACCContents.Add(newButton);
            }
        }

        #endregion

    }
    */

}
