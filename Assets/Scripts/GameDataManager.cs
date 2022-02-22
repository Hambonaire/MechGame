using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public List<Mech> saveDataMechs = new List<Mech>();

    public List<ListItem> playerInventory = new List<ListItem>();

    /* Other stufff like rep, money, etc */

}

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager _instance;

    // Create a field for the save file.
    string saveFile;

    public SaveData saveData = new SaveData();

    void Awake()
    {
        _instance = this;

        // Update the path once the persistent path exists.
        saveFile = Application.persistentDataPath + "/gamedata.json";
    }

    /*
     *  Read the save file and store the data in a local class?
     *  - Takes a lot of space? Holding same data in 2 places at once?
     *  - Should other class refer to this data or have their own copies and reconcile b4 saving?
     *  
     */
    public void ReadSaveFile()
    {
        // Does the file exist?
        if (File.Exists(saveFile))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(saveFile);

            // Deserialize the JSON data 
            //  into a pattern matching the GameData class.
            saveData = JsonUtility.FromJson<SaveData>(fileContents);

            // Set manually for now
            // Use Event Listeners?
            GameManager._instance.availableMechs = saveData.saveDataMechs;
            Inventory._instance.playerInventory = saveData.playerInventory;
        }
    }

    public void WriteSaveFile()
    {
        ConsolidateDataForSave();

        // Serialize the object into JSON and save string.
        string jsonString = JsonUtility.ToJson(saveData);

        // Write JSON to file.
        File.WriteAllText(saveFile, jsonString);
    }

    void ConsolidateDataForSave()
    {
        saveData.saveDataMechs = GameManager._instance.availableMechs;

        saveData.playerInventory = Inventory._instance.playerInventory;
    }
}