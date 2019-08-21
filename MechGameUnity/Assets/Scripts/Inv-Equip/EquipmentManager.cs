using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour {

	// Slot nums (use % to get 1st digit)
	// 0
	// 1
	// 20 - 29 L
	// 30 - 39
	// 40 - 49
	// 50 - 59 R
	// 60 - 69
	// 70 - 79
	// 80 - 89 A
	
    public Controller_Player controller_player;
    public MechHangar hangar_mech;

	Cockpit currentCockpit;
	Legs currentLegs;  
	List<Accessory> currentAccessories = new List<Accessory>();
	List<List<List<Weapon>>> currentWeapons = new List<List<List<Weapon>>>() {
		new List<List<Weapon>>() {	// 0	Left
			new List<Weapon>(),		// 0,0	Left Reg
			new List<Weapon>(),		// 0,1	Left UH
			new List<Weapon>()		// 0,2	Left Shld
		},
		new List<List<Weapon>>() {	// 1	Right
			new List<Weapon>(),		// 1,0	Right Reg
			new List<Weapon>(),		// 1,1	Right UH
			new List<Weapon>()		// 1,2	Right Shld
		}
	};
	
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

	public static EquipmentManager instance {
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
    }

	void Start ()
	{
        inventory = InventoryManager.instance;

        //BuildFromSave();

        BuildDefault();
    }

    public void BuildFromSave()
    {
        SaveData save = SaveService.instance.Load();//= SaveData.Load();

        currentLegs = database.GetActual(save.legs) as Legs;
        currentCockpit = database.GetActual(save.cockpit) as Cockpit;

        foreach (int i in save.weapons)
        {
            Weapon wep = database.GetActual(i) as Weapon;

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

	public Item GetEquipment(InventorySlot slot)
	{
		int num = slot.index % 10;
		
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
		
		return null;
	}

	// Equip a new item
	public void Equip (Item newItem)
	{
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
				inventory.Add(oldItem);
				
				for (int i = 0; i < currentWeapons.Count; i++) {
					for (int j = 0; j < currentWeapons[i].Count; j++) {
						for (int k = 0; k < currentWeapons[i][j].Count; k++) {
							Weapon wep = currentWeapons[i][j][k];
							if (currentCockpit.weaponMap[(int) wep.side][(int) wep.style] > k) {
								currentWeapons[(int) wep.side][(int) wep.style][k] = wep;
								
								if (onEquipmentChanged != null) onEquipmentChanged.Invoke(wep, null);
							} 
							else {
								inventory.Add(wep);
								
								if (onEquipmentChanged != null) onEquipmentChanged.Invoke(null, wep);
							}
						}
					}
				}
				
				for (int i = 0; i < currentAccessories.Count; i++) {
					Accessory it = currentAccessories[i];
					if (currentCockpit.accessoryCount > i) {
						currentAccessories.Add(it);
						if (onEquipmentChanged != null) onEquipmentChanged.Invoke(it, null);
					}
					else {
						inventory.Add(it);
						if (onEquipmentChanged != null) onEquipmentChanged.Invoke(null, it);
					}
				}
			}	
			
			if (onEquipmentChanged != null)
				onEquipmentChanged.Invoke(newItem, oldItem);
			
			currentCockpit = newItem as Cockpit;			
		}
		else if (newItem is Legs)
		{
			if (currentLegs != null) {
				oldItem = currentLegs;
				inventory.Add (oldItem);
			}
			
			if (onEquipmentChanged != null)
				onEquipmentChanged.Invoke(newItem, oldItem);
			
			currentLegs = newItem as Legs;
		}
		else if (newItem is Weapon)
		{
			Weapon wep = newItem as Weapon;
			
			if (currentWeapons[(int) wep.side][(int) wep.style].Count >= currentCockpit.weaponMap[(int) wep.side][(int) wep.style]) {
				oldItem = currentWeapons[(int) wep.side][(int) wep.style][0];
				inventory.Add(oldItem);
				currentWeapons[(int) wep.side][(int) wep.style].RemoveAt(0); // Needed (?), ref prob not removed thru inventory.Add() 
			}
			
			if (onEquipmentChanged != null)
				onEquipmentChanged.Invoke(newItem, oldItem);
			
			currentWeapons[(int) wep.side][(int) wep.style].Add(wep);
		}
		else if (newItem is Accessory)
		{
            Accessory acc = newItem as Accessory;

			if (currentAccessories.Count >= currentCockpit.accessoryCount) {
				oldItem = currentAccessories[0];
				inventory.Add(oldItem);
				currentAccessories.RemoveAt(0);
			}
			
			if (onEquipmentChanged != null)
				onEquipmentChanged.Invoke(newItem, oldItem);
			
			currentAccessories.Add(acc);	
		}
		
		Rebuild();
	}

	void Unequip(int slotIndex) {
		
		int num = slotIndex % 10;
		
		Item oldItem;
		
		if (slotIndex >= 80) {
			oldItem = currentAccessories[num];
			inventory.Add(oldItem);
			currentAccessories.RemoveAt(num);

            if (onEquipmentChanged != null) onEquipmentChanged.Invoke(null, oldItem);
        }
        else if (slotIndex >= 70) {
			oldItem = currentWeapons[1][2][num];
			inventory.Add(oldItem);
			currentWeapons[1][2].RemoveAt(num);

            if (onEquipmentChanged != null) onEquipmentChanged.Invoke(null, oldItem);
        }
        else if (slotIndex >= 60) {
			oldItem = currentWeapons[1][1][num];
			inventory.Add(oldItem);
			currentWeapons[1][1].RemoveAt(num);

            if (onEquipmentChanged != null) onEquipmentChanged.Invoke(null, oldItem);
        }
        else if (slotIndex >= 50) {
			oldItem = currentWeapons[1][0][num];
			inventory.Add(oldItem);
			currentWeapons[1][0].RemoveAt(num);

            if (onEquipmentChanged != null) onEquipmentChanged.Invoke(null, oldItem);
        }
        else if (slotIndex >= 40) {
			oldItem = currentWeapons[0][2][num];
			inventory.Add(oldItem);
			currentWeapons[0][2].RemoveAt(num);

            if (onEquipmentChanged != null) onEquipmentChanged.Invoke(null, oldItem);
        }
        else if (slotIndex >= 30) {
			oldItem = currentWeapons[0][1][num];
			inventory.Add(oldItem);
			currentWeapons[0][1].RemoveAt(num);

            if (onEquipmentChanged != null) onEquipmentChanged.Invoke(null, oldItem);
        }
        else if (slotIndex >= 20) {
			oldItem = currentWeapons[0][0][num];
			inventory.Add(oldItem);
			currentWeapons[0][0].RemoveAt(num);

            if (onEquipmentChanged != null) onEquipmentChanged.Invoke(null, oldItem);
        }
        else if (slotIndex == 1) {
			oldItem = currentLegs;
			inventory.Add(oldItem);
			currentLegs = null;

            if (onEquipmentChanged != null) onEquipmentChanged.Invoke(null, oldItem);
        }
        else if (slotIndex >= 0) {
			oldItem = currentCockpit;
			inventory.Add(oldItem);
			currentCockpit = null;
			
			for (int i = 0; i < currentWeapons.Count; i++) {
				for (int j = 0; j < currentWeapons[i].Count; j++) {
					for (int k = 0; k < currentWeapons[i][j].Count; k++) {
						Weapon wep = currentWeapons[i][j][k];
						inventory.Add(wep);
						currentWeapons[i][j].RemoveAt(k);
						if (onEquipmentChanged != null) onEquipmentChanged.Invoke(null, wep);
					}
				}
			}
			
			for (int i = 0; i < currentAccessories.Count; i++) {
				Item it = currentAccessories[i];
				inventory.Add(it);
				currentWeapons.RemoveAt(i);
				if (onEquipmentChanged != null) onEquipmentChanged.Invoke(null, it);
			}

            if (onEquipmentChanged != null) onEquipmentChanged.Invoke(null, oldItem);
        }
		
		Rebuild();
	}
	
	public void Rebuild() {
        if (controller_player != null)
            controller_player.Rebuild(currentCockpit, currentLegs, currentWeapons, currentAccessories);

        if (hangar_mech != null)
            hangar_mech.Rebuild(currentCockpit, currentLegs, currentWeapons, currentAccessories);
    }

    void BuildDefault()
    {
        currentLegs = database.GetActual(defaultLegs.idNum) as Legs;
        currentCockpit = database.GetActual(defaultCockpit.idNum) as Cockpit;

        currentWeapons[0][0].Add(database.GetActual(defaultLeftWeapon.idNum) as Weapon);
        currentWeapons[1][0].Add(database.GetActual(defaultRightWeapon.idNum) as Weapon);

        Rebuild();      
    }
}
