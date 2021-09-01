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
[RequireComponent(typeof(SectionManager))]
[RequireComponent(typeof(WeaponSystem))]
public class MechManager : MonoBehaviour {

    public SectionManager sectionManager;
    public WeaponSystem weaponSystem;

    bool torsoDestroyed = false;
    bool headDestroyed = false;
    bool leftLegDestroyed = false;
    bool rightLegDestroyed = false;

    List<GameObject> equippedLeftArmObj = new List<GameObject>();
    List<GameObject> equippedRightArmObj = new List<GameObject>();
    List<GameObject> equippedLeftShoulderObj = new List<GameObject>();
    List<GameObject> equippedRightShoulderObj = new List<GameObject>();

	List<WeaponExecutable> executableLeftArm = new List<WeaponExecutable>();
	List<WeaponExecutable> executableRightArm = new List<WeaponExecutable>();
	List<WeaponExecutable> executableLeftShoulder = new List<WeaponExecutable>();
	List<WeaponExecutable> executableRightShoulder = new List<WeaponExecutable>();
	
    [HideInInspector]
    public float baseMoveSpeed = 5;

    void Start()
    {
        sectionManager = GetComponent<SectionManager>();
        weaponSystem = GetComponent<WeaponSystem>();
    }

    void Update()
    {

    }

    public List<GameObject> GetWeaponObjByIndex(int index)
    {
        if (index == (int) SectionIndex.leftArm)
            return equippedLeftArmObj;
        else if (index == (int) SectionIndex.rightArm)
            return equippedRightArmObj;
        else if (index == (int) SectionIndex.leftShoulder)
            return equippedLeftShoulderObj;
        else if (index == (int) SectionIndex.rightShoulder)
            return equippedRightShoulderObj;
        else
            return null;
    }
	
	public List<WeaponExecutable> GetExecutableByIndex(int index)
    {
        if (index == (int) SectionIndex.leftArm)
            return executableLeftArm;
        else if (index == (int) SectionIndex.rightArm)
            return executableRightArm;
        else if (index == (int) SectionIndex.leftShoulder)
            return executableLeftShoulder;
        else if (index == (int) SectionIndex.rightShoulder)
            return executableRightShoulder;
        else
            return null;
    }
    
    /* Use this to check for lethal damage etc */
    public void CheckForDamage()
    {
        if (sectionManager.GetSectionByIndex((int)SectionIndex.torso) == null ||
            sectionManager.GetSectionByIndex((int)SectionIndex.torso).isDestroyed)
        {
            
        }
        
        if (sectionManager.GetSectionByIndex((int)SectionIndex.head) == null ||
            sectionManager.GetSectionByIndex((int)SectionIndex.head).isDestroyed)
        {
            
        }
        
        if (sectionManager.GetSectionByIndex((int)SectionIndex.leftLeg) == null ||
            sectionManager.GetSectionByIndex((int)SectionIndex.leftLeg).isDestroyed)
        {
            
        }
        if (sectionManager.GetSectionByIndex((int)SectionIndex.rightLeg) == null ||
            sectionManager.GetSectionByIndex((int)SectionIndex.rightLeg).isDestroyed)
        {
            
        }
    }
	
}
