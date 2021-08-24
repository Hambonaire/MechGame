using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour {

    public PlayerController controller_player;

    public MechHangar hangar_mech;

    public Cockpit currentCockpit;

    public Legs currentLegs;

    public List<Accessory> currentAccessories = new List<Accessory>();

    // Left Hand, Right Hand, Left Shoulder, Right Shoulder
    public Weapon[] currentWeapons = new Weapon[4]; 

	// Callback for when an item is equipped
	public delegate void OnEquipmentChanged(Item newItem, Item oldItem);
	public event OnEquipmentChanged onEquipmentChanged;

	public InventoryManager inventory;
    public Database database;

    public Cockpit defaultCockpit;
    public Legs defaultLegs;
    public Weapon defaultRightWeapon;
    public Weapon defaultLeftWeapon;

	#region Singleton

	public static EquipmentManager Instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<EquipmentManager> ();
			}
			return _instance;
		}
	}
	static EquipmentManager _instance;
    
	#endregion
	
	void Awake ()
	{
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }

        currentCockpit = defaultCockpit;
        currentLegs = defaultLegs;
    }

	void Start ()
	{
        inventory = InventoryManager.Instance;

        //BuildFromSave();

        BuildDefault();
    }

    // TODO: fix this
    public void BuildFromSave()
    {
        SaveData save = SaveService.instance.Load();//= SaveData.Load();

        currentLegs = database.GetActual(save.legs) as Legs;
        currentCockpit = database.GetActual(save.cockpit) as Cockpit;

        foreach (int i in save.weapons)
        {
            Weapon wep = database.GetActual(i) as Weapon;

            /*
            // Add if room
            if (currentWeapons[(int)wep.side][(int)wep.style].Count < currentCockpit.weaponMap[(int)wep.side][(int)wep.style])
            {
                currentWeapons[(int)wep.side][(int)wep.style].Add(wep);
            }
            // Add to inventory if no room
            else
            {
                inventory.Add(wep);
            }
            */
        }

        foreach (int i in save.accessories)
        {
            Accessory it = database.GetActual(i) as Accessory;

            // Add if room
            if (currentAccessories.Count < currentCockpit.accessoryCount)
            {
                currentAccessories.Add(it);
            }
            // Add to inventory if no room
            else
            {
                inventory.Add(it);
            }
        }

        Rebuild();
    }

    // TODO: fix this if i use it....
	public Item GetEquipment(InventorySlot slot)
	{
		int num = slot.index % 10;
		
        /*
		if (slot.index >= 80) 
			return currentAccessories[num];
		else if (slot.index >= 70)
			return currentWeapons[1][2][num];
		else if (slot.index >= 60)
			return currentWeapons[1][1][num];
		else if (slot.index >= 50)
			return currentWeapons[1][0][num];
		else if (slot.index >= 40)
			return currentWeapons[0][2][num];
		else if (slot.index >= 30)
			return currentWeapons[0][1][num];
		else if (slot.index >= 20)
			return currentWeapons[0][0][num];
		else if (slot.index == 1)
			return currentLegs;
		else if (slot.index >= 0)
			return currentCockpit;
		*/

		return null;
	}

	// Equip a new item
	public void Equip (Item newItem, MechInfoItem selectedSlot)
	{
        if (!InventoryManager.Instance.Contains(newItem))
            return;

		Item oldItem = null;
		
		// Find out what slot the item fits in
		// and put it there.
		//int slotIndex = (int)newItem.equipSlot;

        if (newItem is Cockpit)
        {
            // Have to recheck weapons and accessories for space... call Equip()(?)

            if (currentCockpit != null)
            {
                oldItem = currentCockpit;
                //inventory.Add(oldItem);
                currentCockpit = newItem as Cockpit;

                if (onEquipmentChanged != null)
                    onEquipmentChanged.Invoke(newItem, oldItem);

                Weapon[] newWeapons = new Weapon[4];

                // Handle weapons on the cockpit right now
                if (currentCockpit.leftHandSlot)
                    newWeapons[0] = currentWeapons[0];
                else
                {
                    //inventory.Add(currentWeapons[0]);
                    if (onEquipmentChanged != null)
                        onEquipmentChanged.Invoke(null, currentWeapons[0]);
                }
                if (currentCockpit.rightHandSlot)
                    newWeapons[1] = currentWeapons[1];
                else
                {
                    //inventory.Add(currentWeapons[1]);
                    if (onEquipmentChanged != null)
                        onEquipmentChanged.Invoke(null, currentWeapons[1]);
                }
                if (currentCockpit.leftShoulderSlot)
                    newWeapons[2] = currentWeapons[2];
                else
                {
                    //inventory.Add(currentWeapons[2]);
                    if (onEquipmentChanged != null)
                        onEquipmentChanged.Invoke(null, currentWeapons[2]);
                }
                if (currentCockpit.rightShoulderSlot)
                    newWeapons[3] = currentWeapons[3];
                else
                {
                    //inventory.Add(currentWeapons[3]);
                    if (onEquipmentChanged != null)
                        onEquipmentChanged.Invoke(null, currentWeapons[3]);
                }
                currentWeapons = newWeapons;

                /**
                for (int i = 0; i < currentWeapons.Length; i++)
                {

                    for (int j = 0; j < currentWeapons[i].Count; j++)
                    {
                        List<Weapon> newWeapons = new List<Weapon>();
                        while (newWeapons.Count < currentCockpit.weaponMap[i][j])
                        {
                            newWeapons.Add(null);
                        }

                        for (int k = 0; k < currentWeapons[i][j].Count; k ++)
                        {
                            if (k < newWeapons.Count)
                            {
                                newWeapons[k] = currentWeapons[i][j][k];
                            }
                            else
                            {
                                inventory.Add(currentWeapons[i][j][k]);
                                if (onEquipmentChanged != null)
                                    onEquipmentChanged.Invoke(null, currentWeapons[i][j][k]);
                            }
                        }

                        currentWeapons[i][j].Clear();
                        currentWeapons[i][j] = newWeapons;
                    }
                }
                **/

                // Handle accessories on the cockpit right now
                List<Accessory> newAccessories = new List<Accessory>();
                while (newAccessories.Count < currentCockpit.accessoryCount)
                {
                    newAccessories.Add(null);
                }
                for (int i = 0; i < currentAccessories.Count; i++)
                {
                    if (i < newAccessories.Count)
                    {
                        newAccessories[i] = currentAccessories[i];
                    }
                    else
                    {
                        //inventory.Add(currentAccessories[i]);
                        if (onEquipmentChanged != null)
                            onEquipmentChanged.Invoke(null, currentAccessories[i]);
                    }
                }
                currentAccessories.Clear();
                currentAccessories = newAccessories;
            }

            if (onEquipmentChanged != null)
                onEquipmentChanged.Invoke(null, null);
        }
        else if (newItem is Legs)
        {
            if (currentLegs != null)
            {
                oldItem = currentLegs;
                //inventory.Add(oldItem);

                onEquipmentChanged.Invoke(null, oldItem);
            }

            currentLegs = newItem as Legs;

            if (onEquipmentChanged != null)
                onEquipmentChanged.Invoke(newItem, null);
        }
        else if (newItem is Weapon)
        {
            Weapon newWeapon = newItem as Weapon;

            if (currentWeapons[(int)newWeapon.slot])
            {
                oldItem = currentWeapons[(int)newWeapon.slot];
                //inventory.Add(oldItem);
            }

            currentWeapons[(int)newWeapon.slot] = newWeapon;

            /**
            // Want to equip in certain slot
            if (selectedSlot != null)
            {
                Weapon newWeapon = newItem as Weapon;

                // Slot is empty
                if (selectedSlot.typeAsString == "Weapon" && selectedSlot.myItem == null && currentWeapons[selectedSlot.myPosition.a][selectedSlot.myPosition.b][selectedSlot.myPosition.c] == null &&
                    selectedSlot.myPosition.a == (int) newWeapon.side && selectedSlot.myPosition.b == (int) newWeapon.style)
                {
                    currentWeapons[selectedSlot.myPosition.a][selectedSlot.myPosition.b][selectedSlot.myPosition.c] = newItem as Weapon;
                    if (onEquipmentChanged != null)
                        onEquipmentChanged.Invoke(newItem, null);
                }
                // Slot isnt empty
                else if (selectedSlot.typeAsString == "Weapon" && selectedSlot.myItem != null && currentWeapons[selectedSlot.myPosition.a][selectedSlot.myPosition.b][selectedSlot.myPosition.c] != null &&
                    selectedSlot.myPosition.a == (int) newWeapon.side && selectedSlot.myPosition.b == (int) newWeapon.style)
                {
                    oldItem = currentWeapons[selectedSlot.myPosition.a][selectedSlot.myPosition.b][selectedSlot.myPosition.c];// = selectedSlot.myItem as Weapon;
                    inventory.Add(oldItem);
                    currentWeapons[selectedSlot.myPosition.a][selectedSlot.myPosition.b][selectedSlot.myPosition.c] = newItem as Weapon;
                    if (onEquipmentChanged != null)
                        onEquipmentChanged.Invoke(newItem, oldItem);
                }
            }
            // Equip in best open or oldest in slot
            else
            {
                Weapon newWeapon = newItem as Weapon;

                // Check for an open slot
                for (int i = 0; i < currentWeapons[(int)newWeapon.side][(int)newWeapon.style].Count; i++)
                {
                    // An open slot is found
                    if (currentWeapons[(int)newWeapon.side][(int)newWeapon.style][i] == null)
                    {
                        currentWeapons[(int)newWeapon.side][(int)newWeapon.style][i] = newWeapon;
                        if (onEquipmentChanged != null)
                            onEquipmentChanged.Invoke(newWeapon, null);
                    }
                    // No open slot, so replace oldest
                    else if(currentWeapons[(int)newWeapon.side][(int)newWeapon.style].Count > 0)
                    {
                        oldItem = currentWeapons[(int)newWeapon.side][(int)newWeapon.style][0];
                        currentWeapons[(int)newWeapon.side][(int)newWeapon.style][0] = newWeapon;
                        if (onEquipmentChanged != null)
                            onEquipmentChanged.Invoke(newWeapon, oldItem);
                    }
                    // No slot at all
                    else
                    {
                        // Do something?
                    }
                }
            }
            **/
        }
        else if (newItem is Accessory)
        {
            // Want to equip in certain slot
            if (selectedSlot != null )
            {
                // Slot is empty
                if (selectedSlot.typeAsString == "Accessory" && selectedSlot.myItem == null && currentAccessories[selectedSlot.myPosition.a] == null)
                {
                    currentAccessories[selectedSlot.myPosition.a] = newItem as Accessory;
                    if (onEquipmentChanged != null)
                        onEquipmentChanged.Invoke(newItem, null);
                }
                // SLot isnt empty
                else if (selectedSlot.typeAsString == "Accessory" && selectedSlot.myItem != null && currentAccessories[selectedSlot.myPosition.a] != null)
                {
                    oldItem = currentAccessories[selectedSlot.myPosition.a];
                    //inventory.Add(oldItem);
                    currentAccessories[selectedSlot.myPosition.a] = newItem as Accessory;
                    if (onEquipmentChanged != null)
                        onEquipmentChanged.Invoke(newItem, oldItem);
                }
            }
            // Equip in best open or oldest in slot
            else
            {
                Accessory newAccessory = newItem as Accessory;

                // Check for an open slot
                for (int i = 0; i < currentAccessories.Count; i++)
                {
                    // An open slot is found
                    if (currentAccessories[i] == null)
                    {
                        currentAccessories[i] = newAccessory;
                        if (onEquipmentChanged != null)
                            onEquipmentChanged.Invoke(newAccessory, null);
                    }
                    // No open slot, so replace oldest
                    else if (currentAccessories.Count > 0)
                    {
                        oldItem = currentAccessories[0];
                        currentAccessories[0] = newAccessory;
                        if (onEquipmentChanged != null)
                            onEquipmentChanged.Invoke(newAccessory, oldItem);
                    }
                    // No slot at all
                    else
                    {
                        // Do something?
                    }
                }

                Accessory acc = newItem as Accessory;

                if (currentAccessories.Count >= currentCockpit.accessoryCount)
                {
                    while (currentAccessories.Count >= currentCockpit.accessoryCount)
                    {
                        oldItem = currentAccessories[0];
                        //inventory.Add(oldItem);
                        currentAccessories.RemoveAt(0);

                        if (onEquipmentChanged != null)
                            onEquipmentChanged.Invoke(null, oldItem);
                    }
                }

                currentAccessories.Add(acc);

                if (onEquipmentChanged != null)
                    onEquipmentChanged.Invoke(newItem, null);
            }
        }

        //InventoryManager.Instance.Remove(newItem);

        Rebuild();
	}

	public void Unequip(MechInfoItem selectedSlot) {

        Item oldItem;
        /**
        if (selectedSlot != null)
        {
            if (selectedSlot.myItem != null)
            {
                if (selectedSlot.typeAsString == "Weapon")
                {
                    oldItem = currentWeapons[selectedSlot.myPosition.a][selectedSlot.myPosition.b][selectedSlot.myPosition.c];
                    currentWeapons[selectedSlot.myPosition.a][selectedSlot.myPosition.b][selectedSlot.myPosition.c] = null;
                    if (onEquipmentChanged != null)
                        onEquipmentChanged.Invoke(null, oldItem);
                }
                else if (selectedSlot.typeAsString == "Accessory")
                {
                    oldItem = currentAccessories[selectedSlot.myPosition.a];
                    currentAccessories[selectedSlot.myPosition.a] = null;
                    if (onEquipmentChanged != null)
                        onEquipmentChanged.Invoke(null, oldItem);
                }
                else if (selectedSlot.typeAsString == "Cockpit")
                {
                    oldItem = currentCockpit;
                    currentCockpit = null;
                    if (onEquipmentChanged != null)
                        onEquipmentChanged.Invoke(null, oldItem);
                }
                else if (selectedSlot.typeAsString == "Legs")
                {
                    oldItem = currentAccessories[selectedSlot.myPosition.a];
                    currentAccessories[selectedSlot.myPosition.a] = null;
                    if (onEquipmentChanged != null)
                        onEquipmentChanged.Invoke(null, oldItem);
                }
            }
        }
        **/
		Rebuild();
	}
	
    public void Unequip(int typeID)
    {
        Item oldItem = null;
        
        if (typeID == 0)
        {
            oldItem = currentCockpit;
            currentCockpit = null;
            if (onEquipmentChanged != null)
                onEquipmentChanged.Invoke(null, oldItem);
        }
        else if (typeID == 1000)
        {
            oldItem = currentLegs;
            currentLegs = null;
            if (onEquipmentChanged != null)
                onEquipmentChanged.Invoke(null, oldItem);
        }
        else if (typeID == 2000)
        {
            oldItem = currentWeapons[0];
            currentWeapons[0] = null;
            if (onEquipmentChanged != null)
                onEquipmentChanged.Invoke(null, oldItem);
        }
        else if (typeID == 3000)
        {
            oldItem = currentWeapons[1];
            currentWeapons[1] = null;
            if (onEquipmentChanged != null)
                onEquipmentChanged.Invoke(null, oldItem);
        }
        else if (typeID == 4000)
        {
            oldItem = currentWeapons[2];
            currentWeapons[2] = null;
            if (onEquipmentChanged != null)
                onEquipmentChanged.Invoke(null, oldItem);
        }
        else if (typeID == 5000)
        {
            oldItem = currentWeapons[3];
            currentWeapons[3] = null;
            if (onEquipmentChanged != null)
                onEquipmentChanged.Invoke(null, oldItem);
        }

        if (oldItem != null)
        {
            //inventory.Add(oldItem);
        }

        Rebuild();

        /**
        if (selectedSlot != null)
        {
            if (selectedSlot.myItem != null)
            {
                if (selectedSlot.typeAsString == "Weapon")
                {
                    oldItem = currentWeapons[selectedSlot.myPosition.a][selectedSlot.myPosition.b][selectedSlot.myPosition.c];
                    currentWeapons[selectedSlot.myPosition.a][selectedSlot.myPosition.b][selectedSlot.myPosition.c] = null;
                    if (onEquipmentChanged != null)
                        onEquipmentChanged.Invoke(null, oldItem);
                }
                else if (selectedSlot.typeAsString == "Accessory")
                {
                    oldItem = currentAccessories[selectedSlot.myPosition.a];
                    currentAccessories[selectedSlot.myPosition.a] = null;
                    if (onEquipmentChanged != null)
                        onEquipmentChanged.Invoke(null, oldItem);
                }
                else if (selectedSlot.typeAsString == "Cockpit")
                {
                    oldItem = currentCockpit;
                    currentCockpit = null;
                    if (onEquipmentChanged != null)
                        onEquipmentChanged.Invoke(null, oldItem);
                }
                else if (selectedSlot.typeAsString == "Legs")
                {
                    oldItem = currentAccessories[selectedSlot.myPosition.a];
                    currentAccessories[selectedSlot.myPosition.a] = null;
                    if (onEquipmentChanged != null)
                        onEquipmentChanged.Invoke(null, oldItem);
                }
            }
        }
        **/
    }

    public void Rebuild() {
        if (controller_player != null)
            controller_player.Rebuild();

        if (hangar_mech != null)
            hangar_mech.Rebuild();
    }

    void BuildDefault()
    {
        //InventoryManager.Instance.Add(database.GetActual(defaultCockpit.idNum) as Cockpit);
        InventoryManager.Instance.Add(database.GetActual(defaultCockpit.idNum) as Cockpit);
        Equip(database.GetActual(defaultCockpit.idNum) as Cockpit, null);
        //currentCockpit = database.GetActual(defaultCockpit.idNum) as Cockpit;

        //InventoryManager.Instance.Add(database.GetActual(defaultLegs.idNum) as Legs);
        InventoryManager.Instance.Add(database.GetActual(defaultLegs.idNum) as Legs);
        Equip(database.GetActual(defaultLegs.idNum) as Legs, null);
        //currentLegs = database.GetActual(defaultLegs.idNum) as Legs;

        //InventoryManager.Instance.Add(database.GetActual(defaultLeftWeapon.idNum) as Weapon);
        InventoryManager.Instance.Add(database.GetActual(defaultLeftWeapon.idNum) as Weapon);
        Equip(database.GetActual(defaultLeftWeapon.idNum) as Weapon, null);
        //currentWeapons[0][0].Add(database.GetActual(defaultLeftWeapon.idNum) as Weapon);

        //InventoryManager.Instance.Add(database.GetActual(defaultRightWeapon.idNum) as Weapon);
        InventoryManager.Instance.Add(database.GetActual(defaultRightWeapon.idNum) as Weapon);
        Equip(database.GetActual(defaultRightWeapon.idNum) as Weapon, null);
        //currentWeapons[1][0].Add(database.GetActual(defaultRightWeapon.idNum) as Weapon);

        Rebuild();
    }
}
