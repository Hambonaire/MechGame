using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory _instance;

    public List<ListItem> playerInventory = new List<ListItem>();

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void BuildInventoryFromSave()
    {

    }

}
