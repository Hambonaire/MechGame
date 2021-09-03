using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  On hangar scene load build each mech from each Mech.cs script in the saved list (Put in GameManager...?)
 *
 *
 */
public class HangarManager : MonoBehaviour
{
    public static HangarManager _instance;

    public GameObject[] mechObjects = { null, null, null, null };

    public int currentlySelectedMechIndex = 0;

    /* -1: Nothing
     * 0: Torso
     * 1: Head
     * 2: L Leg
     * 3: R Leg
     * 4: Left Arm
     * 5: Right Arm
     * 6: Left Shoulder
     * 7: Right Shoulder 
     */
    public int currentylSelectedSectionIndex = -1;

    /* -1: Nothing
     * x: Subsection index
     */
    public int currentlySelectedSubsectionIndex = -1;

	public Item currentlySelectedMechItem;

    [Header("Camera")]
    Camera mainHangarCamera;
    public GameObject lookAtTarget;
    Vector3 cameraStartPos;

	int mechSpacing = 6;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        //mainHangarCamera = FindObjectOfType<Camera>();
        //cameraStartPos = mainHangarCamera.transform.position;
        lookAtTarget.transform.position = Vector3.up * 1.3f;

        MechBuilder builder = new MechBuilder();
		
		for (int mechIndex = 0; mechIndex < GameManager._instance.availableMechs.Count; mechIndex++)
		{
            if (mechIndex > 3)
                break;

			GameObject mech = builder.BuildFromMechObj(GameManager._instance.availableMechs[mechIndex], Vector3.right * mechIndex * mechSpacing, false, false, false);
            mech.transform.rotation = Quaternion.Euler(0, 180, 0);

            mechObjects[mechIndex] = mech;
		}
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

        currentylSelectedSectionIndex = -1;
        currentlySelectedSubsectionIndex = -1;

        // TODO: Remove/modify this if buttons use built-in color updates
        HangarUI._instance.MakeDirty(true, true ,true, true, false, false);

        SwapMechView(index);

        HangarUI._instance.EnableUI(false, false, false);
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

        HangarUI._instance.MakeDirty(false, true, true, true, false, false);

        HangarUI._instance.EnableUI(true, false, true);
    }

    /*
    *  Player presses a mech subsection button
    *  1. Rebuild the UI
    *      - Update and open the info item panel
    *      - Reset and close the shop UI
    */
    public void SelectMechSubsection(int index)
    {
        Debug.LogError("SelectMechSubsection DONT USE (probably)");

        //currentlySelectedSubsectionIndex = index;

        //HangarUI._instance.MakeDirty(false, true, true, true, false, false);
    }

    /* Move the hangar view from current mech to the selected mech */
    public void SwapMechView(int index)
    {

        lookAtTarget.transform.position = (Vector3.right * currentlySelectedMechIndex * mechSpacing) + Vector3.up * 1.3f;
        
    }

	/*
	 *	Build item info pnl
	 */
	public void SelectMechItem(Item item)
	{
		currentlySelectedMechItem = item;
		
		HangarUI._instance.MakeDirty(false, false, false, true, false, false);
	}

    // TODO
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
        builder.BuildFromMechObj(GameManager._instance.availableMechs[currentlySelectedMechIndex], Vector3.right * currentlySelectedMechIndex * mechSpacing, false, false, false);

        HangarUI._instance.MakeDirty(false, true, true, false, false, false);
    }

    public void RebuildMechCurrentIndex()
    {
        MechBuilder builder = new MechBuilder();

        GameObject mech = builder.BuildFromMechObj(GameManager._instance.availableMechs[currentlySelectedMechIndex], Vector3.right * currentlySelectedMechIndex * mechSpacing, false, false, false);
        mech.transform.rotation = Quaternion.Euler(0, 180, 0);

        Destroy(mechObjects[currentlySelectedMechIndex]);
        mechObjects[currentlySelectedMechIndex] = mech;
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

    public void ConfirmSection()
    {
        HangarUI._instance.EnableUI(false, false, false);
    }

}
