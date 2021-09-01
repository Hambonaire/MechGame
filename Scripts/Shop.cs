using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop _instance;

    [HideInInspector]
    public ShopItem currentlySelectedShopItem;

    public GameObject buttonPrefab;
    public GameObject contentHolder;
	
	/* Merhcant Alliance */
    List<Item> merchantAllianceItems = new List<Item>();
	
	/* ___ Surplus */
    List<Item> mSurplusItems = new List<Item>();
	
	/* callamx..? Yards */
    List<Item> cYardsItems = new List<Item>();
	
	/* Horizon Ind */
    List<Item> horizonIndItems = new List<Item>();
	
	/* Immostrom */

    void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
		
    }

    void Update()
    {
		
    }

    public void Build(int id)
    {
        
    }
	
	/*
	 *	Generate shop lists for each merchant tab
	 */
	void GenerateItemList()
	{
		itemTable 
		weaponTable 
		mechTemplateTable
		mechPatternTable
	}
	
	public void BuyItem()
	{
		
	}
	
	public void SellItem()
	{
		
	}
}
