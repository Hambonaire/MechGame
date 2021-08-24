using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop _instance;

    public Database database;
    InventoryManager inventory;
    EquipmentManager equipment;

    [HideInInspector]
    public ShopItem currentlySelectedShopItem;

    public GameObject buttonPrefab;
    public GameObject contentHolder;
    public InfoPanel InfoPanel;

    List<Item> allItems = new List<Item>();

    void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        inventory = InventoryManager.Instance;
        equipment = EquipmentManager.Instance;

        HideShop();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GetComponent<CanvasGroup>().alpha == 1)
        {
            HideShop();
        }
    }

    public void Build(int id)
    {
        foreach (Transform child in contentHolder.transform)
        {
            Destroy(child.gameObject);
        }

        int r = id / 1000;
        int rt = r * 1000;
        int rtt = rt + 999;

        List<int> shopTempIds = new List<int>();
        for(int i = 0; i < database.database.Count; i++)
        {
            if (database.database[i].idNum >= rt && database.database[i].idNum <= rtt)
            {
                shopTempIds.Add(database.database[i].idNum);
            }
        }
        shopTempIds.Sort();

        List<int> invTempIds = new List<int>();
        for (int j = 0; j < inventory.items.Count; j++)
        {
            invTempIds.Add(inventory.items[j].idNum);
        }
        invTempIds.Sort();

        // Build UI and check incIds for items in inventory...
        for (int k = 0; k < shopTempIds.Count; k++)
        {
            GameObject button = Instantiate(buttonPrefab) as GameObject;
            ShopItem itemScript = button.GetComponent<ShopItem>();
            Item currentItem = database.GetActual(shopTempIds[k]);

            button.SetActive(true);
            //button.transform.SetParent(buttonPrefab.transform.parent, false);
            button.transform.SetParent(contentHolder.transform, false);

            itemScript.infoRef = InfoPanel;

            int amountInInventory = 0;

            for (int m = 0; m < invTempIds.Count; m++)
            {
                if (invTempIds[m] == shopTempIds[k])
                    amountInInventory++;
            }

            itemScript.SetNewItem(currentItem , amountInInventory);
            itemScript.shopRef = this;
        }
    }

    public void Select(ShopItem shopInfo)
    {
        if (currentlySelectedShopItem != null)
            currentlySelectedShopItem.isSelected = false;
        currentlySelectedShopItem = shopInfo;
    }

    public void ShowShop()
    {
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void HideShop()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}
