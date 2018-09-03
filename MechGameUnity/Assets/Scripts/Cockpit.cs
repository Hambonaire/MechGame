using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cockpit", menuName = "Mecha/Cockpit")]
public class Cockpit : ScriptableObject {

    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;

    public GameObject prefab;
    public Transform rightArmConnection;
    public Transform leftArmConnection;
    public Transform rotationCenter;
}