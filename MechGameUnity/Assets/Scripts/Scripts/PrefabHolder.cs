using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : MonoBehaviour
{
    public GameObject MechInfoButtonPrefab;
    public GameObject MechInfoButtonEmptyPrefab;
    public GameObject ShopItemButtonPrefab;

    #region Singleton

    public static PrefabHolder instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PrefabHolder>();
            }
            return _instance;
        }
    }
    static PrefabHolder _instance;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
