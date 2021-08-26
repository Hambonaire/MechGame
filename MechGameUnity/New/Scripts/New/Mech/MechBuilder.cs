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
        /* Reset */

        GameObject mechBase = Object.Instantiate(mech.mechBaseRef.basePrefab);

        var mechManager = mechBase.GetComponent<MechManager>();
        var sectionManager = mechBase.GetComponent<SectionManager>();
        
        /* Loop thru all sections */
        for (int secIndex = (int) sectionIndex.leftArm; secIndex < (int) sectionIndex.rightShoulder; secIndex++)
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
				mechManager.GetExecutableByIndex(secIndex).add(new WeaponExecutable(
					newWeapon, 
					mech.GetSectionItemsByIndex(secIndex)[subIndex] as WeaponItem,
					newWeapon.GetComponent<Weapon>().bulSpwnLoc, 
					newWeapon.GetComponent<Weapon>().wepAnim));
            }
        }

        modifyMechForGameplay(mechBase, modForGame, asPlayer, asEnemy);

        return mechBase;
    }

    /*
     *  Modify the build mech obj to be ready for gameplay
     *      - Attach MechController to Parent
     *      - 
     */
    public void modifyMechForGameplay(GameObject mech, bool modForGame = false, bool asPlayer = false, bool asEnemy = false)
    {
		if (asPlayer)
		{
			mechBase.AddComponent<PlayerController>();
		}
		else if (asEnemy)
		{
			mechBase.AddComponent<EnemyController>();
		}
		else
		{
			mechBase.AddComponent<MechController>();
		}
        	
    }
}
