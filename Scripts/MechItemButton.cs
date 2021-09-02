using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public bool isSelected = false;

    //public InfoPanel infoRef;
    public Shop shopRef;

    public Item myItem;
    Button myButton;

    public Image itemIcon;
    public Image selectorImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI itemPrice;
    public TextMeshProUGUI amountInInventoryText;

    public TextMeshProUGUI health;
    public TextMeshProUGUI shield;
    public TextMeshProUGUI ballisticArmor;
    public TextMeshProUGUI energyArmor;
    public TextMeshProUGUI ballisticDamage;
    public TextMeshProUGUI energyDamage;

    public int amountInInventory;

    public Button buyButton;
    public Button equipButton;

    void Start()
    {
        myButton = GetComponent<Button>();

        shopRef = Shop._instance;
    }

    void Update()
    {
        if (isSelected)
        {
            //myButton.OnSelect(null);

            Color colTemp = selectorImage.color;
            colTemp.a = (1 + Mathf.Sin(2 * Mathf.PI * .5f * Time.time));
            selectorImage.color = colTemp; ;
        }
        else
        {
            Color colTemp = selectorImage.color;
            colTemp.a = 0;
            selectorImage.color = colTemp;
        }

    }

    public void SetNewItem(Item newItem, int amountInInventory)
    {
        myItem = newItem;

        itemName.text = newItem.name;
        itemDescription.text = newItem.description;
        itemPrice.text = "$" + newItem.price;
        itemIcon.sprite = newItem.icon;

        this.amountInInventory = amountInInventory;
        this.amountInInventoryText.text = amountInInventory.ToString();

        if (amountInInventory > 0)
        {
            buyButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(true);
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(false);
        }

        GetComponent<Button>().onClick.AddListener(delegate { OnPressed(myItem); });

        buyButton.onClick.AddListener(delegate { OnBuyPressed(); });
        equipButton.onClick.AddListener(delegate { OnEquipPressed(); });
    }

    void OnPressed(Item myItem)
    {
        //isSelected = true;
        //shopRef.Select(this);
        //infoRef.UpdateDescription(this);
    }

    public void IncreaseAmountInInventory(int i)
    {
        amountInInventory += i;
        amountInInventoryText.text = amountInInventory.ToString();
    }

    void OnBuyPressed()
    {
        //IncreaseAmountInInventory(1);
       // InventoryManager.Instance.Add(myItem);
       // buyButton.gameObject.SetActive(false);
       // equipButton.gameObject.SetActive(true);
    }

    void OnEquipPressed()
    {
        //InventoryManager.Instance.Add(myItem);
        //InventoryManager.Instance.Add(myItem);
       // EquipmentManager.Instance.Equip(myItem, null);
    }
}
