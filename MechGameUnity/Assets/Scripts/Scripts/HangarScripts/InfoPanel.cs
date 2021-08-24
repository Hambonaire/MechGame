using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    public static InfoPanel _instance;

    // Shop
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemPrice;
    public TextMeshProUGUI itemHealth;
    public TextMeshProUGUI itemShield;
    public TextMeshProUGUI itemShieldRegen;
    public TextMeshProUGUI ballisticArmor;
    public TextMeshProUGUI energyArmor;
    public TextMeshProUGUI ballisticDamage;
    public TextMeshProUGUI energyDamage;
    public TextMeshProUGUI flavorText;
    public TextMeshProUGUI cockpitSize;
    public TextMeshProUGUI weaponCountLeft;
    public TextMeshProUGUI weaponCountRight;
    public TextMeshProUGUI legsSpeed;
    public Image manufacturerIcon;
    public GameObject buyButton;
    public GameObject equipButton;

    // Mech info
    public TextMeshProUGUI itemName2;
    public TextMeshProUGUI itemPrice2;
    public TextMeshProUGUI itemHealth2;
    public TextMeshProUGUI itemShield2;
    public TextMeshProUGUI itemShieldRegen2;
    public TextMeshProUGUI ballisticArmor2;
    public TextMeshProUGUI energyArmor2;
    public TextMeshProUGUI ballisticDamage2;
    public TextMeshProUGUI energyDamage2;
    public TextMeshProUGUI flavorText2;
    public TextMeshProUGUI cockpitSize2;
    public TextMeshProUGUI weaponCountLeft2;
    public TextMeshProUGUI weaponCountRight2;
    public TextMeshProUGUI legsSpeed2;
    public Image manufacturerIcon2;
    public GameObject buyButton2;
    public GameObject unequipButton;

    int currentMoney;
    public TextMeshProUGUI moneyDisplayText;

    public MechInfoItem selectedMechSlot;
    public ShopItem selectedShopSlot;

    public List<TextMeshProUGUI> allTexts = new List<TextMeshProUGUI>();

    //public delegate void OnItemClick(Item newItem);
    //public Event

    void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateMoney", 0, 5);
    }

    // Update is called once per frame
    void UpdateMoney()
    {
        int mo = Random.Range(0, 1000000);
        currentMoney = mo;

        if (mo > 10000000)
        {
            int mon = mo / 1000000;
            moneyDisplayText.text = "$" + mon + "m";
        }
        else if (mo > 10000)
        {
            int mon = mo / 1000;
            moneyDisplayText.text = "$" + mon + "k";
        }
        else
             moneyDisplayText.text = "$" + mo;
    }

    public void UpdateDescription(ShopItem newShopItem)
    {
        //SetAllDisabled();
        selectedShopSlot = newShopItem;

        Item newItem = newShopItem.myItem;

        if (newItem != null)
        {
            itemName.text = newItem.name;
            itemPrice.text = "$" + newItem.price;
            itemHealth.text = newItem.health.ToString();
            itemShield.text = newItem.shield.ToString();
            itemShieldRegen.text = newItem.shieldRegen.ToString();
            ballisticArmor.text = newItem.ballisticArmor.ToString();
            energyArmor.text = newItem.energyArmor.ToString();
            flavorText.text = newItem.description;
            //manufacturerIcon.sprite = newItem.manufacturer;
            manufacturerIcon.enabled = true;

            if (newItem is Cockpit)
            {
                Cockpit newCockpit = newItem as Cockpit;
                //weaponCountLeft.text = "Regular Slots: " + newCockpit.leftRegularCount + "\n" + "Underhand Slots: " + newCockpit.leftUnderhandCount + "\n" + "Shoulder Slots: " + newCockpit.leftShoulderCount;
                //weaponCountRight.text = "Regular Slots: " + newCockpit.rightRegularCount + "\n" + "Underhand Slots: " + newCockpit.rightUnderhandCount + "\n" + "Shoulder Slots: " + newCockpit.rightShoulderCount;
            }
            if (newItem is Legs)
            {
                Legs newLegs = newItem as Legs;
                legsSpeed.text = newLegs.walkSpeed.ToString();
            }
            if (newItem is Weapon)
            {
                Weapon newWeapon = newItem as Weapon;
                ballisticDamage.text = newWeapon.ballisticDamage.ToString();
                energyDamage.text = newWeapon.energyDamage.ToString();

                // More weapon stuff?
                // Ammo?
                // ROF?
                // Do in bars?
            }

        }

    }

    public void UpdateDescription(MechInfoItem newMechInfoItem)
    {
        //SetAllDisabled();

        selectedMechSlot = newMechInfoItem;

        Item newItem = newMechInfoItem.myItem;

        if (newItem != null)
        {
            itemName2.text = newItem.name;
            itemPrice2.text = "$" + newItem.price;
            itemHealth2.text = newItem.health.ToString();
            itemShield2.text = newItem.shield.ToString();
            itemShieldRegen2.text = newItem.shieldRegen.ToString();
            ballisticArmor2.text = newItem.ballisticArmor.ToString();
            energyArmor2.text = newItem.energyArmor.ToString();
            flavorText2.text = newItem.description;
            //manufacturerIcon.sprite = newItem.manufacturer;
            manufacturerIcon2.enabled = true;

            if (newItem is Cockpit)
            {
                Cockpit newCockpit = newItem as Cockpit;
                //weaponCountLeft2.text = "Regular Slots: " + newCockpit.leftRegularCount + "\n" + "Underhand Slots: " + newCockpit.leftUnderhandCount + "\n" + "Shoulder Slots: " + newCockpit.leftShoulderCount;
                //weaponCountRight2.text = "Regular Slots: " + newCockpit.rightRegularCount + "\n" + "Underhand Slots: " + newCockpit.rightUnderhandCount + "\n" + "Shoulder Slots: " + newCockpit.rightShoulderCount;
            }
            if (newItem is Legs)
            {
                Legs newLegs = newItem as Legs;
                legsSpeed2.text = newLegs.walkSpeed.ToString();
            }
            if (newItem is Weapon)
            {
                Weapon newWeapon = newItem as Weapon;
                ballisticDamage2.text = newWeapon.ballisticDamage.ToString();
                energyDamage2.text = newWeapon.energyDamage.ToString();

                // More weapon stuff?
                // Ammo?
                // ROF?
                // Do in bars?
            }

        }

    }

    public void UpdateDescription(Item newItem)
    {
        if (newItem != null)
        {
            itemName.text = newItem.name;
            itemPrice.text = "$" + newItem.price;
            itemHealth.text = newItem.health.ToString();
            itemShield.text = newItem.shield.ToString();
            itemShieldRegen.text = newItem.shieldRegen.ToString();
            ballisticArmor.text = newItem.ballisticArmor.ToString();
            energyArmor.text = newItem.energyArmor.ToString();
            flavorText.text = newItem.description;
            //manufacturerIcon.sprite = newItem.manufacturer;
            manufacturerIcon.enabled = true;

            if (newItem is Cockpit)
            {
                Cockpit newCockpit = newItem as Cockpit;
                //weaponCountLeft2.text = "Regular Slots: " + newCockpit.leftRegularCount + "\n" + "Underhand Slots: " + newCockpit.leftUnderhandCount + "\n" + "Shoulder Slots: " + newCockpit.leftShoulderCount;
                //weaponCountRight2.text = "Regular Slots: " + newCockpit.rightRegularCount + "\n" + "Underhand Slots: " + newCockpit.rightUnderhandCount + "\n" + "Shoulder Slots: " + newCockpit.rightShoulderCount;
            }
            if (newItem is Legs)
            {
                Legs newLegs = newItem as Legs;
                legsSpeed.text = newLegs.walkSpeed.ToString();
            }
            if (newItem is Weapon)
            {
                Weapon newWeapon = newItem as Weapon;
                ballisticDamage.text = newWeapon.ballisticDamage.ToString();
                energyDamage.text = newWeapon.energyDamage.ToString();

                // More weapon stuff?
                // Ammo?
                // ROF?
                // Do in bars?
            }

        }
    }

    // Used for MechInfoPanel buttons, call with int for corresponding item type
    public void MechInfoButtonPress(int idRange)
    {
        if (idRange == 0)
            UpdateDescription(EquipmentManager.Instance.currentCockpit);
        else if (idRange == 1000)
            UpdateDescription(EquipmentManager.Instance.currentLegs);
        else if (idRange == 2000)
            UpdateDescription(EquipmentManager.Instance.currentWeapons[0]);
        else if (idRange == 3000)
            UpdateDescription(EquipmentManager.Instance.currentWeapons[1]);
        else if (idRange == 4000)
            UpdateDescription(EquipmentManager.Instance.currentWeapons[2]);
        else if (idRange == 5000)
            UpdateDescription(EquipmentManager.Instance.currentWeapons[3]);
    }

    void SetAllDisabled()
    {
        //foreach (Text t in allTexts)
        //{
        //    t.enabled = false;
        //}

        manufacturerIcon.enabled = false;
    }

    public void OpenShop()
    {
        Shop._instance.Build(MechInfo._instance.currentlySelectedTypeID);

        Shop._instance.ShowShop();

        /*
        Debug.Log("Buying");

        if (selectedShopSlot != null)
        {
            Debug.Log("Not null");

            if (currentMoney > selectedShopSlot.myItem.price)
            {
                Debug.Log("Have money");

                InventoryManager.Instance.Add(selectedShopSlot.myItem);
                selectedShopSlot.IncreaseAmountInInentory(1);
            }
        }
        */
    }

    public void Unequip()
    {
        print("unequipping " + MechInfo._instance.currentlySelectedTypeID);
        EquipmentManager.Instance.Unequip(MechInfo._instance.currentlySelectedTypeID);
    }

    public void Equip()
    {
        //EquipmentManager.Instance.Equip(selectedShopSlot.myItem, selectedMechSlot);
    }
}
