using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechBuilder
{
    /*  Get Legs & Torso & Arms from MechBase
     *      - Attach accessories
     *  Get Weapons instantiate and attach
     */
    public GameObject BuildFromMechObj(Mech mech, Vector3 location, bool modForGame = false, bool asPlayer = false, bool asEnemy = false)
    {
        GameObject mechObj = Object.Instantiate(mech.mechBaseRef.basePrefab, location, Quaternion.identity);

        var mechManager = mechObj.GetComponent<MechManager>();
        var sectionManager = mechObj.GetComponent<SectionManager>();
        
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

        ModifyMechForGameplay(mech, mechObj, modForGame, asPlayer, asEnemy);

        return mechObj;
    }

    private void BuildClassWeapon(WeaponItem wItem, MechManager mechManager, SectionManager sectionManager, int secIndex, int subIndex, int classIndex)
    {
        if (wItem == null)
            return;

        /* Create the Wep */
        GameObject newWeapon = Object.Instantiate(wItem.prefab, sectionManager.GetSectionLinksByIndex(secIndex, classIndex)[subIndex]);
        newWeapon.transform.localPosition = Vector3.zero;

        /* Add the weapons to the MechManager on the mech encapsulating object */
        mechManager.GetWeaponObjByIndex(secIndex).Add(newWeapon);

        /* Add the ref to the parent section to the weapon's script */
        newWeapon.GetComponent<Weapon>().sectionParent = sectionManager.GetSectionByIndex(secIndex);

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
    public void ModifyMechForGameplay(Mech mech, GameObject mechObj, bool modForGame, bool asPlayer, bool asEnemy)
    {
        if (!modForGame)
            return;

		if (asPlayer)
		{
            var PC = mechObj.AddComponent<PlayerController>();
            PC.walkSpeed = mech.mechBaseRef.baseMoveSpeed;

            mechObj.GetComponent<CharacterController>().enabled = true;

            mechObj.GetComponent<WeaponSystem>().enabled = true;
		}
		else if (asEnemy)
		{
            //mech.AddComponent<EnemyController>();

            mechObj.GetComponent<CharacterController>().enabled = true;
            mechObj.GetComponent<WeaponSystem>().enabled = true;
        }
        else
		{
            mechObj.AddComponent<MechController>();

            mechObj.GetComponent<CharacterController>().enabled = true;
            mechObj.GetComponent<WeaponSystem>().enabled = true;
        }
        	
    }

    /* 
     *  Build randomized mech from parameters
     *  - Mech Tier
     *  - Weapon Type Bias
     *  - Range Bias
     *  - Weapon Tier Bias
     */
    public void BuildRandomMech(MechTables table, int mechTier, int weaponTypeBias, int tierBias, bool modForGame, bool asPlayer, bool asEnemy)
    {

    }
}
