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
            /* Create a weapon for each subsection */
            for (int subIndex = 0; subIndex < mech.GetSubsectionCountByIndex(secIndex); subIndex++)
            {
                /* No item in this slot */
                if (mech.GetSectionItemsByIndex(secIndex)[subIndex] == null)
                    continue;
                /* Subsection OOB of available links */
                if (!(subIndex < sectionManager.GetSectionLinksByIndex(secIndex).Length))
                    break;

				/* Create the Wep */
                GameObject newWeapon = Object.Instantiate((mech.GetSectionItemsByIndex(secIndex)[subIndex] as WeaponItem).prefab, sectionManager.GetSectionLinksByIndex(secIndex)[subIndex]);
                newWeapon.transform.localPosition = Vector3.zero;

                /* Add the weapons to the MechManager on the mech encapsulating object */
                mechManager.GetWeaponObjByIndex(secIndex).Add(newWeapon);

                /* Add the ref to the parent section to the weapon's script */
                newWeapon.GetComponent<Weapon>().sectionParent = sectionManager.GetSectionByIndex(secIndex);

                // Move into modify???
                /* Create an executable for the weapon and add it to MechManager ref */
                mechManager.GetExecutableByIndex(secIndex).Add(newWeapon.GetComponent<WeaponExecutable>());
            }
        }

        /* Set relevant stats */
        mechManager.baseMoveSpeed = mech.mechBaseRef.baseMoveSpeed;

        ModifyMechForGameplay(mech, mechObj, modForGame, asPlayer, asEnemy);

        return mechObj;
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
