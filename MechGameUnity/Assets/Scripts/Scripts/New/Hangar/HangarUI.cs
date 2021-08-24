using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarUI : MonoBehaviour
{
    public static HangarUI _instance;

    bool isDirty;
    bool[] dirtyFlags = new bool[4] { false, false, false, false };

    /* List of buttons that select the mech to modify, send signal back to HangarManager */
    public List<GameObject> mechSelectButton = new List<GameObject>();
    public GameObject mechSelectButtonParent;
    public GameObject mechSelectButtonPrefab;

    /* Mech info panel */
    public GameObject mechInfoPanel;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        BuildUI();   
    }

    void Update()
    {

    }

    void LateUpdate()
    {
        if (isDirty)
        {
            BuildUI();
            isDirty = false;
            dirtyFlags = new bool[4] { false, false, false, false };
        }
    }

    /*
     *  1: Select Buttons
     *  2: Mech Info Panel
     *  3: Item Info Panel
     *  4: Shop
     */
    public void MakeDirty(bool selectButtons, bool mechInfoPanel, bool itemInfoPanel, bool shop)
    {
        isDirty = true;

        if (selectButtons)
            dirtyFlags[0] = true;
        if (mechInfoPanel)
            dirtyFlags[1] = true;
        if (itemInfoPanel)
            dirtyFlags[2] = true;
        if (shop)
            dirtyFlags[3] = true;
    }

    void BuildUI()
    {
        /* Build the mech select buttons */
        if (dirtyFlags[0])
        {
            BuildMechSelectButtons();
        }

        /* Build the mech info panel (LEFT) */
        if (dirtyFlags[1])
        {

        }
    }

    void BuildMechSelectButtons()
    {
        foreach (Transform child in mechSelectButtonParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Mech mech in GameManager._instance.availableMechs)
        {
            GameObject button = Instantiate(mechSelectButtonPrefab);
        }
    }

}
