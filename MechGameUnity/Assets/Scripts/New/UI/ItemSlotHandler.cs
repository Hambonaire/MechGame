using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListItem
{
    public int count = 0;
    public Item item;

    public ListItem(int _count, Item _item)
    {
        count = _count;
        item = _item;
    }
}

public class ItemSlotHandler : MonoBehaviour
{
    [SerializeField]
    public List<ListItem> handlerItems = new List<ListItem>();

    public GameObject contentObj;

    public GameObject itemButtonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildClean()
    {
        handlerItems.Clear();

        foreach (Transform child in contentObj.transform)
            Destroy(child.gameObject);
    }

    public void BuildFromItemList()
    {
        foreach (Transform child in contentObj.transform)
            Destroy(child.gameObject);

        foreach (ListItem item in handlerItems)
        {
            var newButton = Instantiate(itemButtonPrefab, contentObj.transform);

            newButton.GetComponent<MechItemButton>().Initialize(item.item, item.count, this as ItemSlotHandler);
        }
    }

    public void ParseItemsFromButtons()
    {
        handlerItems.Clear();

        foreach (Transform child in contentObj.transform)
        {
            handlerItems.Add(
                new ListItem(child.GetComponent<MechItemButton>().count,
                child.GetComponent<MechItemButton>().myItem));
        }

    }

    public void AddItemToList(Item newItem, int count)
    {
        var existingItem = handlerItems.Find(x => x.item.Equals(newItem));

        if (existingItem != null)
        {
            existingItem.count += count;
        }
        else
        {
            handlerItems.Add(new ListItem(count, newItem));
        }
    }

    public void RemoveItemFromList(Item newItem, int count)
    {
        var existingItem = handlerItems.Find(x => x.item.Equals(newItem));

        if (existingItem != null)
        {
            print("Found -> Removing");

            existingItem.count -= count;

            if (existingItem.count <= 0)
                handlerItems.Remove(existingItem);
        }

    }

}
