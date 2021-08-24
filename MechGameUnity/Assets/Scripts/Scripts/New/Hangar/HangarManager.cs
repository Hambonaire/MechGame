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
     * 1: Legs
     * 2: Left Arm
     * 3: Right Arm
     * 4: Left Shoulder
     * 5: Right Shoulder 
     */
    public int currentylSelectedSectionIndex = -1;

    /* -1: Nothing
     * x: Subsection index
     */
    public int currentlySelectedkSubsectionIndex = -1;

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
        currentlySelectedkSubsectionIndex = index;

        HangarUI._instance.MakeDirty(false, true, true, false);
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
        if (currentlySelectedMechIndex == -1 || currentylSelectedSectionIndex == -1 || currentlySelectedkSubsectionIndex == -1)
            return;


        HangarUI._instance.MakeDirty(false, true, false, false);
    }

    /* Update the Mech.cs obj in the GameManager with the modified loadout */
    public void SaveMechLoadout()
    {

    }
}
