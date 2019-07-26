using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour {

	Cockpit currentCockpit;
    Legs currentLegs;
   
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
		
		foreach (int i in save.weapons) {
			Weapon wep = database.GetActual(i) as Weapon;
			
			// Add if room
			if (currentWeapons[(int) wep.side][(int) wep.style].Count < currentCockpit.weaponMap[(int) wep.side][(int) wep.style]) {
				currentWeapons[(int) wep.side][(int) wep.style].Add(wep);
			}
			// Add to inventory if no room
			else
			{
				inventory.Add(wep);
			}
		}
		
		foreach (int i in save.accessories) {
			Item it = database.GetActual(i);
			
			// Add if room
			if (currentAccessories.Count < currentCockpit.accessoryCount) {
				currentAccessories.Add(it);
			}
			// Add to inventory if no room
			else
			{
				inventory.Add(it);
			}
		}

	}

	/// Needs to be implemented(?), for param might create enum for each type (?)
	//public Item GetEquipment(EquipmentSlot slot) {
	//	return currentEquipment [(int)slot];
	//}
	
	public Item GetEquipment()
	{
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
							if (currentCockpit.weaponMap[(int) wep.side][(int) wep.style] < k) {
								currentWeapons[(int) wep.side][(int) wep.style] = wep;
							} 
							else {
								inventory.Add(wep);
							}
						}
					}
				}
				
				// DO THE SAME ^^^ FOR ACCESSORY
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
			
			// REBUILD MECH
		}
		else if (newItem is Weapon)
		{
			wep = newItem as Weapon;
			
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
			if (accessories.Count >= currentCockpit.accessoryCount) {
				oldItem = currentAccessories[0];
				inventory.Add(oldItem);
				currentAccessories.RemoveAt(0);
			}
			
			if (onEquipmentChanged != null)
				onEquipmentChanged.Invoke(newItem, oldItem);
			
			currentAccessories.Add(newItem);	
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
