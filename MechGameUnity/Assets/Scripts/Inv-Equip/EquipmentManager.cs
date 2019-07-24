using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour {

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

	Item currentCockpit;
	Item currentLegs;
	List<Item> currentWepLeftRegular = new List<Item>;
	List<Item> currentWepLeftUnderhand = new List<Item>;
	List<Item> currentWepLeftShoulder = new List<Item>;
	List<Item> currentWepRightRegular = new List<Item>;
	List<Item> currentWepRightUnderhand = new List<Item>;
	List<Item> currentWepRightShoulder = new List<Item>;
	List<Item> currentAccessories = new List<Item>;

	// Callback for when an item is equipped
	public delegate void OnEquipmentChanged(Item newItem, Item oldItem);
	public event OnEquipmentChanged onEquipmentChanged;

	InventoryManager inventory;

	void Start ()
	{
		inventory = InventoryManager.instance;
		SaveData save = Load();
				
		currentLegs = Database.Instance.GetActual(save.legs);
		
		currentCockpit = Database.Instance.GetActual(save.cockpit);
		
		// Save Wep data as left and right
		// loop thru saved left and rights and emplace here if enough space, which there should be since reading directly from save		
		foreach (int i in save.leftWeapons) {
			Weapon wep = Database.Instance.GetActual(i) as Weapon;
			if (wep.style == WeaponStyle.Regular && currentWepLeftRegular.Count < currentCockpit.leftRegularCount) {
				currentWepLeftRegular.Add(wep);
			} else if (wep.style == WeaponStyle.Underhand && currentWepLeftUnderhand.Count < currentCockpit.leftUnderhandCount) {
				currentWepLeftUnderhand.Add(wep);
			} else if (wep.style == WeaponStyle.Shoulder && currentWepLeftShoulder.Count < currentCockpit.leftShoulderCount) {
				currentWepLeftShoulder.Add(wep);
			}
		}
		
		foreach (int i in save.rightWeapons) {
			Weapon wep = Database.Instance.GetActual(i) as Weapon;
			if (wep.style == WeaponStyle.Regular && currentWepRightRegular.Count < currentCockpit.rightRegularCount) {
				currentWepRightRegular.Add(wep);
			} else if (wep.style == WeaponStyle.Underhand && currentWepRightUnderhand.Count < currentCockpit.rightUnderhandCount) {
				currentWepRightUnderhand.Add(wep);
			} else if (wep.style == WeaponStyle.Shoulder && currentWepRightShoulder.Count < currentCockpit.rightShoulderCount) {
				currentWepRightShoulder.Add(wep);
			}
		}
		
		foreach (int i in save.accessories) {
			Item it = Database.Instance.GetActual(i);
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
		Equipment oldItem = null;

		// Find out what slot the item fits in
		// and put it there.
		int slotIndex = (int)newItem.equipSlot;

		// If there was already an item in the slot
		// make sure to put it back in the inventory
		if (currentEquipment[slotIndex] != null)
		{
			oldItem = currentEquipment [slotIndex];

			inventory.Add (oldItem);
		}

		// An item has been equipped so we trigger the callback
		if (onEquipmentChanged != null)
			onEquipmentChanged.Invoke(newItem, oldItem);

		currentEquipment [slotIndex] = newItem;
		Debug.Log(newItem.name + " equipped!");

		//if (newItem.prefab) {
		//	AttachToMesh (newItem.prefab, slotIndex);
		//}
		//equippedItems [itemIndex] = newMesh.gameObject;

	}

	void Unequip(int slotIndex) {
		if (currentEquipment[slotIndex] != null)
		{
			Equipment oldItem = currentEquipment [slotIndex];
			inventory.Add(oldItem);
				
			currentEquipment [slotIndex] = null;
			//if (currentMeshes [slotIndex] != null) {
			//	Destroy (currentMeshes [slotIndex].gameObject);
			//}

			// Equipment has been removed so we trigger the callback
			if (onEquipmentChanged != null)
				onEquipmentChanged.Invoke(null, oldItem);
			
		}

	
	}

}
