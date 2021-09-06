using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop _instance;

    public GameObject buttonPrefab;
    public GameObject contentHolder;
	
	/* Player items from save data */
    List<Item> inventoryItems = new List<Item>();
	
	void Start()
	{
		
	}
	
	void BuildInventoryFromSave()
	{
		
	}
}
