using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemPrice;
    public TextMeshProUGUI itemHealth;
    public TextMeshProUGUI itemShield;
    public TextMeshProUGUI itemShieldRegen;
    public TextMeshProUGUI ballisticArmor;
    public TextMeshProUGUI energyArmor;
    public TextMeshProUGUI flavorText;
    public Image manufacturerIcon;

    public TextMeshProUGUI cockpitSize;
    public TextMeshProUGUI weaponCountLeft;
    public TextMeshProUGUI weaponCountRight;

    public TextMeshProUGUI legsSpeed;

    public TextMeshProUGUI ballisticDamage;
    public TextMeshProUGUI energyDamage;

    public List<TextMeshProUGUI> allTexts = new List<TextMeshProUGUI>();

    //public delegate void OnItemClick(Item newItem);
    //public Event

    // Start is called before the first frame update
    void Start()
    {
        allTexts.Add(itemName);
        allTexts.Add(itemPrice);
        allTexts.Add(itemHealth);
        allTexts.Add(itemShield);
        allTexts.Add(itemShieldRegen);
        allTexts.Add(ballisticArmor);
        allTexts.Add(energyArmor);
        allTexts.Add(flavorText);
        allTexts.Add(cockpitSize);
        allTexts.Add(weaponCountLeft);
        allTexts.Add(weaponCountRight);
        allTexts.Add(legsSpeed);
        allTexts.Add(ballisticDamage);
        allTexts.Add(energyDamage);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateDescription(ShopItem newShopItem)
    {
        //SetAllDisabled();

        Item newItem = newShopItem.myItem;

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
            weaponCountLeft.text = 
                "Regular Slots: " + newCockpit.leftRegularCount + "\n" + 
                "Underhand Slots: " + newCockpit.leftUnderhandCount + "\n" + 
                "Shoulder Slots: " + newCockpit.leftShoulderCount;
            weaponCountRight.text =
                "Regular Slots: " + newCockpit.rightRegularCount + "\n" +
                "Underhand Slots: " + newCockpit.rightUnderhandCount + "\n" +
                "Shoulder Slots: " + newCockpit.rightShoulderCount;
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

    void SetAllDisabled()
    {
        //foreach (Text t in allTexts)
        //{
        //    t.enabled = false;
        //}

        manufacturerIcon.enabled = false;
    }
}
