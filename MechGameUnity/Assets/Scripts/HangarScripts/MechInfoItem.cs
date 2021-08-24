using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MechInfoItem : MonoBehaviour
{
    public struct EquipmentPosition
    {
        public int a;
        public int b;
        public int c;
    }

    public bool isSelected = false;

    public MechInfo mechInfoRef;
    public InfoPanel infoPanelRef;

    //public TextMeshProUGUI 
    public Item myItem;
    public Image Icon;
    public Sprite default_Empty_Icon;
    [HideInInspector]
    public EquipmentPosition myPosition;
    [HideInInspector]
    public string typeAsString = "";

    public void Update()
    {
        if (isSelected)
        {

        }
    }

    public void SetNewItem(Item newItem)
    {
        if (newItem != null)
        {
            myItem = newItem;

            Icon.sprite = myItem.icon;

            GetComponent<Button>().onClick.AddListener(delegate { OnPressed(); });

            typeAsString = newItem.GetType().ToString();
        }
        else
        {
            Icon.sprite = default_Empty_Icon;
            typeAsString = "null";
        }

    }

    void OnPressed()
    {
        Debug.Log(typeAsString);
        isSelected = true;
        mechInfoRef.Select(this);
        infoPanelRef.UpdateDescription(this);
    }
}
