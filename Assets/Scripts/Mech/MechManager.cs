using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Faction { Neutral, Player, Hostile };

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

    [HideInInspector]
    public bool totallyDestroyed = false;

    public Faction mechFaction = Faction.Neutral;

    [HideInInspector]
    public SectionManager sectionManager;
    [HideInInspector]
    public WeaponSystem weaponSystem;

    [HideInInspector]
    public HUDManager controllerHUD;

    [HideInInspector]
    public OutlineHandler outlineHandler;

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

    public void Initialize()
    {
        sectionManager = GetComponent<SectionManager>();
        weaponSystem = GetComponent<WeaponSystem>();

        outlineHandler = GetComponent<OutlineHandler>();
    }

    void Start()
    {
        Initialize();

        outlineHandler.Initialize(mechFaction);
    }

    void Update()
    {
        CheckForDamage();
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
        if (sectionManager.torsoSection.IsDestroyed)
        {
            totallyDestroyed = true;
            gameObject.SetActive(false);
        }


        if (sectionManager.GetSectionByIndex((int)SectionIndex.torso) == null ||
            sectionManager.GetSectionByIndex((int)SectionIndex.torso).IsDestroyed)
        {
            
        }
        
        if (sectionManager.GetSectionByIndex((int)SectionIndex.head) == null ||
            sectionManager.GetSectionByIndex((int)SectionIndex.head).IsDestroyed)
        {
            
        }
        
        if (sectionManager.GetSectionByIndex((int)SectionIndex.legs) == null ||
            sectionManager.GetSectionByIndex((int)SectionIndex.legs).IsDestroyed)
        {
            
        }

    }
	
    public Transform GetTargetCenter()
    {
        return sectionManager.torsoRotAxis;
    }

}
