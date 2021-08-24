using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Datatype_Weapon
{
    public GameObject weapon_object;
    public GameObject barrel;
    public WeaponExecutable executable;
    //public Weapon data;
    
    public void Delete () {
        GameObject.Destroy(weapon_object);
        GameObject.Destroy(barrel);
        // GC should handle WeaponExecutable and this object overall...
    }
}
