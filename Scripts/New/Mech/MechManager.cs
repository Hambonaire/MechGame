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
	
    [SerializeField]
	List<WeaponExecutable> executableLeftArm = new List<WeaponExecutable>();
	List<WeaponExecutable> executableRightArm = new List<WeaponExecutable>();
	List<WeaponExecutable> executableLeftShoulder = new List<WeaponExecutable>();
	List<WeaponExecutable> executableRightShoulder = new List<WeaponExecutable>();

    void Start()
    {
        sectionManager = GetComponent<SectionManager>();   
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //sectionManager.GetSectionByIndex((int) sectionIndex.leftArm).TakeDamage(10);
        }
    }

    public List<GameObject> GetWeaponObjByIndex(int index)
    {
        if (index == (int) sectionIndex.leftArm)
            return equippedLeftArmObj;
        else if (index == (int) sectionIndex.rightArm)
            return equippedRightArmObj;
        else if (index == (int) sectionIndex.leftShoulder)
            return equippedLeftShoulderObj;
        else if (index == (int) sectionIndex.rightShoulder)
            return equippedRightShoulderObj;
        else
            return null;
    }
	
	public List<WeaponExecutable> GetExecutableByIndex(int index)
    {
        if (index == (int) sectionIndex.leftArm)
            return executableLeftArm;
        else if (index == (int) sectionIndex.rightArm)
            return executableRightArm;
        else if (index == (int) sectionIndex.leftShoulder)
            return executableLeftShoulder;
        else if (index == (int) sectionIndex.rightShoulder)
            return executableRightShoulder;
        else
            return null;
    }
    
    /* Use this to check for lethal damage etc */
    public void CheckForDamage()
    {
        if (sectionManager.GetSectionByIndex((int)sectionIndex.torso) == null ||
            sectionManager.GetSectionByIndex((int)sectionIndex.torso).isDestroyed)
        {
            
        }
        
        if (sectionManager.GetSectionByIndex((int)sectionIndex.head) == null ||
            sectionManager.GetSectionByIndex((int)sectionIndex.head).isDestroyed)
        {
            
        }
        
        if (sectionManager.GetSectionByIndex((int)sectionIndex.leftLeg) == null ||
            sectionManager.GetSectionByIndex((int)sectionIndex.leftLeg).isDestroyed)
        {
            
        }
        if (sectionManager.GetSectionByIndex((int)sectionIndex.rightLeg) == null ||
            sectionManager.GetSectionByIndex((int)sectionIndex.rightLeg).isDestroyed)
        {
            
        }
    }
	
}
