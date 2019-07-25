using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Text;

public class SaveData
{
    #region Defaults
    private const int DEFAULT_MONEY = 0;
    private const int DEFAULT_COCKPIT_ID = 0;
    private const int DEFAULT_WEAPON_LEFT_ID = 0;
    private const int DEFAULT_WEAPON_RIGHT_ID = 0;
    private const int DEFAULT_LEGS_ID = 0;
    private const int DEFAULT_LEVEL = 0;
    #endregion

    #region data
    public int cockpit = DEFAULT_COCKPIT_ID ;
    //private int weaponLeft { get; set; } = DEFAULT_WEAPON_LEFT_ID;
    //private int weaponRight { get; set; } = DEFAULT_WEAPON_RIGHT_ID;
    //public List<int> leftWeapons = new List<int>();
    //public List<int> rightWeapons = new List<int>();
    public List<int> weapons = new List<int>();
    public int legs = DEFAULT_LEGS_ID;
    public int lastLevel = DEFAULT_LEVEL;
    public int money = DEFAULT_MONEY;
    public List<int> accessories = new List<int>();
    public List<int> inventoryItems = new List<int>();
    #endregion

    public void Save()
    {
        string json = JsonUtility.ToJson(this);

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

    public static SaveData Load()
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

    public bool IsDefault()
    {
        return (
            money == DEFAULT_MONEY &&
            cockpit == DEFAULT_COCKPIT_ID &&
            //weaponLeft == DEFAULT_WEAPON_LEFT_ID &&
            //weaponRight == DEFAULT_WEAPON_RIGHT_ID &&
            legs == DEFAULT_LEGS_ID &&
            lastLevel == DEFAULT_LEVEL &&
            accessories.Count == 0 &&
            inventoryItems.Count == 0);
    }


}
