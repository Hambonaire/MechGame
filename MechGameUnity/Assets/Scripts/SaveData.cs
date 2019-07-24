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
    private int cockpit { get; set; } = DEFAULT_COCKPIT_ID ;
    //private int weaponLeft { get; set; } = DEFAULT_WEAPON_LEFT_ID;
    //private int weaponRight { get; set; } = DEFAULT_WEAPON_RIGHT_ID;
    private List<int> leftWeapons { get; set; } = new List<int>();
    private List<int> rightWeapons { get; set; } = new List<int>();
    private int legs { get; set; } = DEFAULT_LEGS_ID;
    private int lastLevel { get; set; } = DEFAULT_LEVEL;
    private int money { get; set; } = DEFAULT_MONEY;
    private List<int> accessories { get; set; } = new List<int>();
    private List<int> inventoryItems { get; set; } = new List<int>();
    #endregion

    public static void Save()
    {
        string json = JsonUtility.ToJson(this, true);

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
