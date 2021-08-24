using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Text;

public class SaveService : MonoBehaviour
{
    #region Singleton
    public static SaveService instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SaveService>();
            }
            return _instance;
        }
    }
    static SaveService _instance;
    #endregion

    void Awake()
    {
        if (!_instance)
        { 
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        
    }

    public void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);

        int key = 129;

        StringBuilder inSb = new StringBuilder(json);
        StringBuilder outSb = new StringBuilder(json.Length);
        char c;
        for (int i = 0; i < json.Length; i++)
        {
            c = inSb[i];

            c = (char)(c ^ key);
            outSb.Append(c);
        }

        File.WriteAllText(Application.persistentDataPath + "/Saves.json", outSb.ToString());
    }

    public SaveData Load()
    {
        if (!File.Exists(Application.persistentDataPath + "/Saves.json"))
        {
            Debug.Log("File not found, returning new object");
            return new SaveData();
        }
        else
        {
            string contents = File.ReadAllText(Application.persistentDataPath + "/Saves.json");

            if (string.IsNullOrEmpty(contents))
            {
                Debug.Log("File is empty. Returning default SaveData");
                return new SaveData();
            }

            int key = 129;

            StringBuilder inSb = new StringBuilder(contents);
            StringBuilder outSb = new StringBuilder(contents.Length);
            char c;
            for (int i = 0; i < contents.Length; i++)
            {
                c = inSb[i];
                c = (char)(c ^ key);
                outSb.Append(c);
            }

            return JsonUtility.FromJson<SaveData>(outSb.ToString());
        }
    }

    /*
    public bool IsDefault(SaveData data)
    {
        return (
            data.money == data.DEFAULT_MONEY &&
            data.cockpit == data.DEFAULT_COCKPIT_ID &&
            //weaponLeft == DEFAULT_WEAPON_LEFT_ID &&
            //weaponRight == DEFAULT_WEAPON_RIGHT_ID &&
            data.legs == data.DEFAULT_LEGS_ID &&
            data.lastLevel == DEFAULT_LEVEL &&
            data.accessories.Count == 0 &&
            data.inventoryItems.Count == 0);
    }
    */
}
