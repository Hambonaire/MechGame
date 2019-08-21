using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public InfoPanel infoRef;

    public Item myItem;
        
    //public Text itemName;
    //public Text itemDescription;
    //public Text itemPrice;
    public Image itemIcon;
    //public Text amountInInventory;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI itemPrice;
    public TextMeshProUGUI amountInInventory;

    public void SetNewItem(Item newItem, int amountInInventory)
    {
        myItem = newItem;

        itemName.text = newItem.name;
        itemDescription.text = newItem.description;
        itemPrice.text = "$" + newItem.price;
        itemIcon.sprite = newItem.icon;

        this.amountInInventory.text = amountInInventory.ToString();

        GetComponent<Button>().onClick.AddListener(delegate { OnPressed(myItem); });
    }

    void OnPressed(Item myItem)
    {
        infoRef.UpdateDescription(this);
    }

}
