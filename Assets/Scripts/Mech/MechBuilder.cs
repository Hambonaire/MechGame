using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechBuilder
{
    /*  Get Legs & Torso & Arms from MechBase
     *      - Attach accessories
     *  Get Weapons instantiate and attach
     */
    public GameObject BuildFromMechObj(Mech mech, Vector3 location, Quaternion rotation, bool modForGame, bool asPlayer, Faction faction, BehaviorType behaviorType, IntelligenceLevel aiLevel)
    {
        GameObject mechObj = Object.Instantiate(mech.mechBaseRef.basePrefab, location, rotation);

        var mechManager = mechObj.GetComponent<MechManager>();
        var sectionManager = mechObj.GetComponent<SectionManager>();

        mechManager.Initialize();
        
        
        /* Attach weapons for each sections */
        for (int secIndex = (int) SectionIndex.leftArm; secIndex < (int) SectionIndex.rightShoulder; secIndex++)
        {
            /* Get the ItemStruct for each section */
            ItemStruct refStruct = mech.GetSectionItemsByIndex(secIndex);

            /* Build from WeaponStruct */
            int subIndex = 0;
            foreach (WeaponItem wItem in refStruct.primary)
            {

                if (subIndex >= sectionManager.GetSectionLinksByIndex(secIndex, 0).Length)
                {
                    Debug.Log("Too many weapons to links in sec " + secIndex + " primary weapons ");
                    break;
                }

                BuildClassWeapon(wItem, mechManager, sectionManager, secIndex, subIndex, 0);
                subIndex++;
            }

            subIndex = 0;
            foreach (WeaponItem wItem in refStruct.secondary)
            {
                if (subIndex >= sectionManager.GetSectionLinksByIndex(secIndex, 1).Length)
                    break;

                BuildClassWeapon(wItem, mechManager, sectionManager, secIndex, subIndex, 1);
                subIndex++;
            }

            subIndex = 0;
            foreach (WeaponItem wItem in refStruct.tertiary)
            {
                if (subIndex >= sectionManager.GetSectionLinksByIndex(secIndex, 2).Length)
                    break;

                BuildClassWeapon(wItem, mechManager, sectionManager, secIndex, subIndex, 2);
                subIndex++;
            }
        }

        /* Set relevant stats */
        mechManager.baseMoveSpeed = mech.mechBaseRef.baseMoveSpeed;
        mechManager.mechFaction = faction;

        if (modForGame)
            ModifyMechForGameplay(mech, mechObj, asPlayer, faction, behaviorType, aiLevel);

        // TODO: Outlines

        return mechObj;
    }

    private void BuildClassWeapon(WeaponItem wItem, MechManager mechManager, SectionManager sectionManager, int secIndex, int subIndex, int classIndex)
    {
        if (wItem == null)
            return;

        /* Create the Wep */
        GameObject newWeapon = Object.Instantiate(wItem.prefab);
        newWeapon.transform.parent = sectionManager.GetSectionLinksByIndex(secIndex, classIndex)[subIndex];
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localEulerAngles = Vector3.zero;

        /* Add the weapons to the MechManager on the mech encapsulating object */
        mechManager.GetWeaponObjByIndex(secIndex).Add(newWeapon);

        /* Add the ref to the parent section to the weapon's script */
        newWeapon.GetComponent<WeaponExecutable>().sectionParent = sectionManager.GetSectionByIndex(secIndex);
        newWeapon.GetComponent<WeaponExecutable>().mechManager = mechManager;

        // Move into modify???
        /* Create an executable for the weapon and add it to MechManager ref */
        mechManager.GetExecutableByIndex(secIndex).Add(newWeapon.GetComponent<WeaponExecutable>());
    }

    /*
     *  Modify the build mech obj to be ready for gameplay
     *      - Attach:
     *          - Mech, Player, Enemy Controllers scripts
     *      - Enable
     *          - CharacterController component
     */
    public void ModifyMechForGameplay(Mech mech, GameObject mechObj, bool asPlayer, Faction faction, BehaviorType behaviorType, IntelligenceLevel aiLevel)
    {
        if (asPlayer)
        {
            var PC = mechObj.AddComponent<PlayerController>();
            PC.walkSpeed = mech.mechBaseRef.baseMoveSpeed;

            mechObj.GetComponent<CharacterController>().enabled = true;
            PC.Initialize();
        }
        else
        {
            var ai = mechObj.AddComponent<AIController>();
            ai.behaviorType = behaviorType;
            ai.aiLevel = aiLevel;
            ai.Initialize();
        }

        mechObj.GetComponent<WeaponSystem>().enabled = true;
    }

    /* 
     *  Build randomized mech from parameters
     *  - Mech Tier
     *  - Weapon Type Bias
     *  - Range Bias
     *  - Weapon Tier Bias
     */
    public void BuildRandomMech(MechTables table, int mechTier, int AmmoTypeBias, int tierBias, bool modForGame, bool asPlayer, bool asEnemy)
    {

    }
}
