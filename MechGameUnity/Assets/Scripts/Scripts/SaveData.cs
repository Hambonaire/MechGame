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
}
