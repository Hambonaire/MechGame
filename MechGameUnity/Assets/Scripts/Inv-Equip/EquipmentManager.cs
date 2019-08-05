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
			else {
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
			else {
				inventory.Add(it);
			}
		}

	}

	/// Needs to be implemented(?), for param might create enum for each type (?)
	//public Item GetEquipment(EquipmentSlot slot) {
	//	return currentEquipment [(int)slot];
	//}
	
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
					Item it = currentAccessories[i];
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
			if (currentAccessories.Count >= currentCockpit.accessoryCount) {
				oldItem = currentAccessories[0];
				inventory.Add(oldItem);
				currentAccessories.RemoveAt(0);
			}
			
			if (onEquipmentChanged != null)
				onEquipmentChanged.Invoke(newItem, oldItem);
			
			currentAccessories.Add(newItem);	
		}
		
		Rebuild();
		
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

	void Rebuild() {
		// Have this be called from some other script?
		// Maybe not since this should only apply to player character
		// But, this script only does stuff in shop/hangar, so it amybe needs to pass equipment to another script 
		// that exists on each seperate level... meaning this script only exists in the shop/hangar scene
	}

    public void Initialize()
    {
        #region Beforehand Calculations
        //overallScaleFactor = currentCockpit.scaleFactor;
        #endregion

        #region Remove Current Items
        if (controller_player.cockpit != null)
        {
            Destroy(controller_player.cockpit);
        }
        if (controller_player.legs != null)
        {
            Destroy(controller_player.legs);
        }
        if (controller_player.rightArmWeapons.Count != 0)
        {
            foreach(GameObject wep in controller_player.rightArmWeapons) {
                Destroy(wep);
            }
        }
        if (controller_player.leftArmWeapons.Count != 0)
        {
            foreach (GameObject wep in controller_player.leftArmWeapons)
            {
                Destroy(wep);
            }
        }
        #endregion

        // TODO: Dont use Find("")
        #region Part Variables
        controller_player.legs = Instantiate(currentLegs.prefab, controller_player.legsRoot.transform.position, transform.rotation);
        controller_player.legs.transform.parent = controller_player.legsRoot.transform;
        // Scaling
        controller_player.legs.transform.localScale = new Vector3(currentCockpit.scaleFactor, currentCockpit.scaleFactor, currentCockpit.scaleFactor);

        controller_player.torsoRoot.transform.position = controller_player.legs.transform.Find("TorsoConnection").position;

        controller_player.cockpit = Instantiate(currentCockpit.prefab, controller_player.torsoRoot.transform.position, controller_player.torsoRoot.transform.rotation);
        controller_player.cockpit.transform.parent = controller_player.torsoRoot.transform;
        // Scaling
        //cockpit.transform.localScale = new Vector3(torsoScaleFactor, torsoScaleFactor, torsoScaleFactor);

        controller_player.cockpitRotationCenter = controller_player.cockpit.transform.Find("RotationAxis");
        #endregion

        /*
        #region Weapon Variables
        if (rightWeaponItem.rightPrefab != null)
        {
            right_bullet = rightWeaponItem.bullet;
            right_ROF = rightWeaponItem.rateOfFire;
            right_maxAmmo = rightWeaponItem.maxAmmo;
            right_reloadTime = rightWeaponItem.reloadTime;
            right_spread = rightWeaponItem.bulletSpray;
            right_bulletSpeed = rightWeaponItem.bulletSpeed;
            right_bulletLife = rightWeaponItem.bulletLife;

            right_autoTrack = rightWeaponItem.autoTarget;

            right_fireType = rightWeaponItem.fireMode;

            if (right_fireType == FireType.Charge)
            {
                right_chargeTime = rightWeaponItem.chargeTimeIfChargeType;
            }
            else if (right_fireType == FireType.Multi)
            {
                right_projectileCount = rightWeaponItem.projectilesCountIfMultiType;
            }
            else if (right_fireType == FireType.Beam)
            {
                right_beamTime = rightWeaponItem.beamTimeIfBeamType;
            }
            else
            {
                right_beamTime = 0;
                left_chargeTime = 0;
            }

            rightArmWeapon = Instantiate(rightWeaponItem.rightPrefab, cockpit.transform.Find("RightArmConnection").position, cockpit.transform.rotation) as GameObject;
            rightArmWeapon.transform.parent = cockpitRotationCenter.transform;

            rightArmBarrel = rightArmWeapon.transform.Find("Barrel");
        }
        */

        // TODO: Do all the data stuff up there ^^^ in lists 
        for (int i = 0; i < currentWeapons[0].Count; i++)
        {
            for(int j = 0; j < currentWeapons[0][i].Count; j++)
            {
                // TODO: Dont use find(""), set up a list in Cockpit items (prob)
                controller_player.rightArmWeapons[j] = Instantiate(currentWeapons[0][i][j].rightPrefab, controller_player.cockpit.transform.Find("RightArmConnection").position, controller_player.cockpit.transform.rotation) as GameObject;
                controller_player.rightArmWeapons[j].transform.parent = controller_player.cockpitRotationCenter.transform;
                
                // TODO: Make a list of barrels etc...^^^
                //rightArmBarrel = rightArmWeapon.transform.Find("Barrel");
            }
        }


        if (leftWeaponItem.leftPrefab != null)
        {
            left_bullet = leftWeaponItem.bullet;
            left_ROF = leftWeaponItem.rateOfFire;
            left_maxAmmo = leftWeaponItem.maxAmmo;
            left_reloadTime = leftWeaponItem.reloadTime;
            left_Spread = leftWeaponItem.bulletSpray;
            left_bulletSpeed = leftWeaponItem.bulletSpeed;
            left_bulletLife = leftWeaponItem.bulletLife;

            left_FireType = leftWeaponItem.fireMode;

            left_autoTrack = leftWeaponItem.autoTarget;

            if (left_FireType == FireType.Charge)
            {
                left_chargeTime = leftWeaponItem.chargeTimeIfChargeType;
            }
            else if (left_FireType == FireType.Multi)
            {
                left_projectileCount = leftWeaponItem.projectilesCountIfMultiType;
            }
            else if (left_FireType == FireType.Beam)
            {
                left_beamTime = leftWeaponItem.beamTimeIfBeamType;
            }
            else
            {
                left_beamTime = 0;
                left_chargeTime = 0;
            }

            leftArmWeapon = Instantiate(leftWeaponItem.leftPrefab, cockpit.transform.Find("LeftArmConnection").position, cockpit.transform.rotation) as GameObject;
            leftArmWeapon.transform.parent = cockpitRotationCenter.transform;

            leftArmBarrel = leftArmWeapon.transform.Find("Barrel");
        }

        //Variables.PlayerPrimaryAmmo_Max = right_maxAmmo;
        //Variables.PlayerSecondaryAmmo_Max = left_maxAmmo;      
        //Variables.PlayerSecondaryAmmo_Curr = Variables.PlayerSecondaryAmmo_Max;
        //Variables.PlayerPrimaryAmmo_Curr = Variables.PlayerPrimaryAmmo_Max;

        playerRightAmmoCurrent.value = right_ammoCurr;
        playerRightAmmoMax.value = right_maxAmmo;
        playerLeftAmmoCurrent.value = left_ammoCurr;
        playerLeftAmmoMax.value = left_maxAmmo;

        //Variables.PrimaryReloading = false;
        //Variables.SecondaryReloading = false;

        playerRightReloading.value = false;
        playerLeftReloading.value = false;

        right_isReloading = false;
        left_isReloading = false;
        #endregion

        #region Animators
        if (legs.transform.Find("AnimatorHolder") != null)
        {
            legsAnimator = legs.transform.Find("AnimatorHolder").GetComponent<Animator>();
        }

        if (rightArmWeapon.transform.Find("AnimatorHolder") != null && rightArmWeapon != null)
        {
            rightWeaponAnimator = rightArmWeapon.transform.Find("AnimatorHolder").GetComponent<Animator>();
        }

        if (leftArmWeapon.transform.Find("AnimatorHolder") != null && leftArmWeapon != null)
        {
            leftWeaponAnimator = leftArmWeapon.transform.Find("AnimatorHolder").GetComponent<Animator>();
        }
        #endregion

        /// NOW HANDLED IN EQUIPMENT MANAGER
        #region Stats
        //playerStats.SetBallisticArmor(cockpitItem.ballisticArmor + legsItem.ballisticArmor);
        //playerStats.SetEnergyArmor(cockpitItem.energyArmor + legsItem.energyArmor);
        //playerStats.SetHealthMax(cockpitItem.health + legsItem.health);

        //Variables.PlayerHealth_Max = playerStats.GetMaxHealth();
        //Variables.PlayerHealth_Curr = playerStats.GetCurrentHealth();
        //Variables.PlayerBallisticArmor = playerStats.GetBallisticArmor();
        //Variables.PlayerEnergyArmor = playerStats.GetEnergyArmor();

        //playerHealthMax.value = playerStats.GetMaxHealth();
        //playerHealthCurrent.value = playerStats.GetCurrentHealth();
        #endregion

        #region Misc.
        walkSpeed = legsItem.walkSpeed * overallScaleFactor;
        runSpeed = legsItem.runSpeed * overallScaleFactor;
        hitBox.radius = baseHitBoxRadius * overallScaleFactor;
        hitBox.height = baseHitBoxHeight * overallScaleFactor;
        hitBox.center = new Vector3(0, hitBox.height / 2 + .05f, hitBox.center.z);
        #endregion

        cockpit.transform.localScale = new Vector3(overallScaleFactor, overallScaleFactor, overallScaleFactor);
    }
}
