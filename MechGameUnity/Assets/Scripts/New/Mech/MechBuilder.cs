using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechBuilder
{
    /*  Get Legs & Torso from MechBase
     *      - Attach accessories
     *  Get Weapons instantiate and attach
     */
    public GameObject BuildFromMechObj(Mech mech)
    {
        /* Reset */
        mech.ResetObjLists();

        GameObject mechBase = Object.Instantiate(mech.mechBaseRef.basePrefab);

        var links = mechBase.GetComponent<LinkManager>();
        
        /* Loop thru all sections */
        for (int secIndex = 3; secIndex < 6; secIndex++)
        {
            /* Create a weapon for each subsection */
            for (int subIndex = 0; subIndex < mech.GetSubsectionCountByIndex(secIndex); subIndex++)
            {

                if (mech.GetSectionItemsByIndex(secIndex)[subIndex] == null)
                    continue;

                GameObject newWeapon = Object.Instantiate((mech.GetSectionItemsByIndex(secIndex)[subIndex] as Weapon).prefab, links.GetSectionLinksByIndex(secIndex)[subIndex]);
                newWeapon.transform.localPosition = Vector3.zero;

                /* Add the mech to obj lists */
                mech.GetWeaponObjByIndex(secIndex).Add(newWeapon);
            }
        }

        return mechBase;
    }

}
