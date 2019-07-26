using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour {

	Cockpit currentCockpit;
    Legs currentLegs;
    /*
	List<List<List<Weapon>>> currentWeapons = new List<List<List<Weapon>>>();
		List<List<Weapon>> leftWeapons = new List<List<Weapon>>(); // 0
			List<Weapon> WepLeftRegular = new List<Weapon>(); // 0,0
			List<Weapon> WepLeftUnderhand = new List<Weapon>(); // 0,1
			List<Weapon> WepLeftShoulder = new List<Weapon>(); // 0,2
		List<List<Weapon>> rightWeapons = new List<List<Weapon>>(); // 1
			List<Weapon> WepRightRegular = new List<Weapon>(); // 1,0
			List<Weapon> WepRightUnderhand = new List<Weapon>(); // 1,1
			List<Weapon> WepRightShoulder = new List<Weapon>(); //1,2
	*/
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
	
	List<Item> currentAccessories = new List<Item>();

	// Callback for when an item is equipped
	public delegate void OnEquipmentChanged(Item newItem, Item oldItem);
	public event OnEquipmentChanged onEquipmentChanged;

	InventoryManager inventory;
    Database database;
	
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

	void Awake ()
	{
		_instance = this;
	}

    #endregion

	void Start ()
	{
		inventory = InventoryManager.instance;
		SaveData save = SaveData.Load(); 

        currentLegs = database.GetActual(save.legs) as Legs;

        currentCockpit = database.GetActual(save.cockpit) as Cockpit;
		
		// Save Wep data as left and right
		// loop thru saved left and rights and emplace here if enough space, which there should be since reading directly from save		
		foreach (int i in save.weapons) {
			Weapon wep = database.GetActual(i) as Weapon;
			
			if (currentWeapons[(int) wep.side][(int) wep.style].Count < currentCockpit.weaponMap[(int) wep.side][(int) wep.style]) {
					currentWeapons[(int) wep.side][(int) wep.style].Add(wep);
			}
			/*	
			if (wep.style == WeaponStyle.Regular && currentWepLeftRegular.Count < currentCockpit.leftRegularCount) {
				currentWepLeftRegular.Add(wep);
			} else if (wep.style == WeaponStyle.Underhand && currentWepLeftUnderhand.Count < currentCockpit.leftUnderhandCount) {
				currentWepLeftUnderhand.Add(wep);
			} else if (wep.style == WeaponStyle.Shoulder && currentWepLeftShoulder.Count < currentCockpit.leftShoulderCount) {
				currentWepLeftShoulder.Add(wep);
			}
			*/
		}
		
		/*
		foreach (int i in save.rightWeapons) {
			
			Weapon wep = database.GetActual(i) as Weapon;
			if (wep.style == WeaponStyle.Regular && currentWepRightRegular.Count < currentCockpit.rightRegularCount) {
				currentWepRightRegular.Add(wep);
			} else if (wep.style == WeaponStyle.Underhand && currentWepRightUnderhand.Count < currentCockpit.rightUnderhandCount) {
				currentWepRightUnderhand.Add(wep);
			} else if (wep.style == WeaponStyle.Shoulder && currentWepRightShoulder.Count < currentCockpit.rightShoulderCount) {
				currentWepRightShoulder.Add(wep);
			}
			
		}
		*/
		
		foreach (int i in save.accessories) {
			Item it = database.GetActual(i);
			if (currentAccessories.Count < currentCockpit.accessoryCount) {
				currentAccessories.Add(it);
			}
		}

	}

	void Update () {
		
	}

	/// Needs to be implemented(?), for param might create enum for each type (?)
	//public Item GetEquipment(EquipmentSlot slot) {
	//	return currentEquipment [(int)slot];
	//}

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
			
			// REBUILD MECH
		}
		else if (newItem is Weapon)
		{
			
		}
		
		// If there was already an item in the slot
		// make sure to put it back in the inventory
		//if (currentEquipment[slotIndex] != null)
		//{
		//	oldItem = currentEquipment [slotIndex];

		//	inventory.Add (oldItem);
		//}

		// An item has been equipped so we trigger the callback
		//if (onEquipmentChanged != null)
		//	onEquipmentChanged.Invoke(newItem, oldItem);

		//currentEquipment [slotIndex] = newItem;
		//Debug.Log(newItem.name + " equipped!");

		//if (newItem.prefab) {
		//	AttachToMesh (newItem.prefab, slotIndex);
		//}
		//equippedItems [itemIndex] = newMesh.gameObject;

	}

	void Unequip(int slotIndex) {
		//if (currentEquipment[slotIndex] != null)
		//{
		//	Equipment oldItem = currentEquipment [slotIndex];
		//	inventory.Add(oldItem);
				
		//	currentEquipment [slotIndex] = null;
			//if (currentMeshes [slotIndex] != null) {
			//	Destroy (currentMeshes [slotIndex].gameObject);
			//}

			// Equipment has been removed so we trigger the callback
			//if (onEquipmentChanged != null)
			//	onEquipmentChanged.Invoke(null, oldItem);
			
		//}

	
	}

}
