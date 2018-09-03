using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Mecha/Weapon")]
public class Weapon : ScriptableObject {

    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;

    public GameObject prefab;
    public Transform barrel;
    public int maxAmmo;
    public GameObject bullet;
    public float rateOfFire;
    public float reloadTime;
    [Range(0, 500)]
    public float bulletSpary;
    public float bulletSpeed;
    public float bulletLife;
}