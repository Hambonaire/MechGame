using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechBuilder
{
    /*  Get Legs & Torso & Arms from MechBase
     *      - Attach accessories
     *  Get Weapons instantiate and attach
     */
    public GameObject BuildFromMechObj(Mech mech)
    {
        /* Reset */

        GameObject mechBase = Object.Instantiate(mech.mechBaseRef.basePrefab);

        var links = mechBase.GetComponent<SectionManager>();
        var mechManager = mechBase.GetComponent<MechManager>();
        
        /* Loop thru all sections */
        for (int secIndex = 3; secIndex < 6; secIndex++)
        {
            /* Create a weapon for each subsection */
            for (int subIndex = 0; subIndex < mech.GetSubsectionCountByIndex(secIndex); subIndex++)
            {
                if (mech.GetSectionItemsByIndex(secIndex)[subIndex] == null)
                    continue;

                if (!(subIndex < links.GetSectionLinksByIndex(secIndex).Length))
                    break;

                GameObject newWeapon = Object.Instantiate((mech.GetSectionItemsByIndex(secIndex)[subIndex] as Weapon).prefab, links.GetSectionLinksByIndex(secIndex)[subIndex]);
                newWeapon.transform.localPosition = Vector3.zero;

                /* Add the weapons to the MechManager on the mech encapsulating object */
                mechManager.GetWeaponObjByIndex(secIndex).Add(newWeapon);
            }
        }

        modifyMechForGameplay(mechBase);

        return mechBase;
    }

    /*
     *  Modify the build mech obj to be ready for gameplay
     *      - Attach MechController to Parent
     *      - Attach Stats to each piece
     */
    public void modifyMechForGameplay(GameObject mech)
    {


        
    }
}
