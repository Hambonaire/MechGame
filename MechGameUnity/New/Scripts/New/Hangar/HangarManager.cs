using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarManager : MonoBehaviour
{
    /*
     *  On hangar scene load build each mech from each Mech.cs script in the saved list (Put in GameManager...?)
     *
     *
     */

    public static HangarManager _instance;

    public int currentlySelectedMechIndex = 0;

    /* -1: Nothing
     * 0: Torso
     * 1: Head
     * 2: Legs
     * 3: Left Arm
     * 4: Right Arm
     * 5: Left Shoulder
     * 6: Right Shoulder 
     */
    public int currentylSelectedSectionIndex = -1;

    /* -1: Nothing
     * x: Subsection index
     */
    public int currentlySelectedSubsectionIndex = -1;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {

        MechBuilder builder = new MechBuilder();

        builder.BuildFromMechObj(GameManager._instance.availableMechs[0]);

    }

    void Update()
    {
        
    }

    /*
     *  Player presses a mech select button
     *  1. Rebuild the UI
     *      - Reset and close the shop UI
     *      - Reset and close(?) the item info panel
     *  2. Move the view to the selected mech
     */
    public void SelectMech(int index)
    {
        currentlySelectedMechIndex = index;

        // TODO: Remove/modify this if buttons use built-in color updates
        HangarUI._instance.MakeDirty(true, true, true, false);

        SwapMechView(index);
    }

    /*
     *  Player presses a mech section button
     *  1. Rebuild the UI
     *      - Update and open the info item panel
     *      - Reset and close the shop UI
     */
    public void SelectMechSection(int index)
    {
        currentylSelectedSectionIndex = index;

        HangarUI._instance.MakeDirty(false, true, true, false);
    }

    /*
    *  Player presses a mech subsection button
    *  1. Rebuild the UI
    *      - Update and open the info item panel
    *      - Reset and close the shop UI
    */
    public void SelectMechSubsection(int index)
    {
        currentlySelectedSubsectionIndex = index;

        HangarUI._instance.MakeDirty(false, true, true, true);
    }

    /* Move the hangar view from current mech to the selected mech */
    public void SwapMechView(int index)
    {

    }

    /*
     *  Equip/Swap the equipped item on the mech
     *  1. modify the Mech.cs for the selected mech
     *  2. Rebuild the selected Mech
     *  3. Rebuild the Hangar UI
     */
    public void EquipMechItem(Item item)
    {
        /* Any selection type is invalid */
        if (currentlySelectedMechIndex == -1 || currentylSelectedSectionIndex == -1 || currentlySelectedSubsectionIndex == -1)
            return;

        /* Not enough slots in the selected subsection */
        if (currentlySelectedSubsectionIndex < GameManager._instance.availableMechs[currentlySelectedMechIndex].GetSubsectionCountByIndex(currentlySelectedSubsectionIndex))
            return;

        /* Selected Head, Torso, or Legs but item is not an accessory */
        if (currentylSelectedSectionIndex == 0 || currentylSelectedSectionIndex == 1 || currentylSelectedSectionIndex == 2 && !(item is Accessory))
            return;

        /* Remove the item in the selected section -> subsection */
        GameManager._instance.availableMechs[currentlySelectedMechIndex].GetSectionItemsByIndex(currentylSelectedSectionIndex)[currentlySelectedSubsectionIndex] = null;

        /* Put the item in the selected section -> subsection */
        GameManager._instance.availableMechs[currentlySelectedMechIndex].GetSectionItemsByIndex(currentylSelectedSectionIndex)[currentlySelectedSubsectionIndex] = item;

        /* Build */
        MechBuilder builder = new MechBuilder();
        builder.BuildFromMechObj(GameManager._instance.availableMechs[currentlySelectedMechIndex]);

        HangarUI._instance.MakeDirty(false, true, false, false);
    }

    /*
     *  Remove the item at selected mech -> section -> subsection
     */
    public void RemoveMechItem()
    {

    }

    /* Update the Mech.cs obj in the GameManager with the modified loadout */
    public void SaveMechLoadout()
    {

    }
}
