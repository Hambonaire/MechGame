using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  The MechManager is attached to the actual mech's encapsulating GameObject
 *  
 *  Holds
 *  - Each section's SectionManager reference (assign manually?)
 *  - EquippedWeapon references (GameObject & WeaponExe script)
 */
public class MechManager : MonoBehaviour {

    SectionManager sectionManager;

    public bool torsoDestroyed = false;
    public bool headDestroyed = false;
    public bool leftLegDestroyed = false;
    public bool rightLegDestroyed = false;

    List<GameObject> equippedLeftArmObj = new List<GameObject>();
    List<GameObject> equippedRightArmObj = new List<GameObject>();
    List<GameObject> equippedLeftShoulderObj = new List<GameObject>();
    List<GameObject> equippedRightShoulderObj = new List<GameObject>();

    void Start()
    {
        
    }

    public List<GameObject> GetWeaponObjByIndex(int index)
    {
        if (index == 3)
            return equippedLeftArmObj;
        else if (index == 4)
            return equippedRightArmObj;
        else if (index == 5)
            return equippedLeftShoulderObj;
        else if (index == 6)
            return equippedRightShoulderObj;
        else
            return null;
    }
    
    /* Use this to check for lethal damage etc */
    public void CheckForDamage()
    {
        if (sectionManager.torsoStats == null || sectionManager.torsoStats.isDestroyed)
        {
            
        }
        
        if (sectionManager.headStats == null || sectionManager.headStats.isDestroyed)
        {
            
        }
        
        if (sectionManager.leftLegStats == null || sectionManager.leftLegStats.isDestroyed)
        {
            
        }
        if (sectionManager.rightLegStats == null || sectionManager.rightLegStats.isDestroyed)
        {
            
        }
    }
}
