using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Size { Small, Medium, Large}

[CreateAssetMenu(fileName = "New Cockpit", menuName = "Mecha/Cockpit")]
public class Cockpit : Item {
    
    public int leftRegularCount = 1;
    public int leftUnderhandCount = 0;
    public int leftShoulderCount = 0;
    
    public int rightRegularCount = 1;
    public int rightUnderhandCount = 0;
    public int rightShoulderCount = 0;

    public Dictionary<int, Dictionary<int, int>> weaponMap = new Dictionary<int, Dictionary<int, int>>() {
        { 0, new Dictionary<int, int> { { 0, 1 }, { 1, 0 }, { 2, 0 } } },
        { 1, new Dictionary<int, int> { { 0, 1 }, { 1, 0 }, { 2, 0 } } }
    };

    public int accessoryCount = 0;
    
    public GameObject prefab;

    public float armDistanceX = 0;
    public float scaleFactor = 1;

    private void Awake()
    {
        weaponMap[0][0] = leftRegularCount;
        weaponMap[0][1] = leftUnderhandCount;
        weaponMap[0][2] = leftShoulderCount;
        weaponMap[1][0] = rightRegularCount;
        weaponMap[1][1] = rightUnderhandCount;
        weaponMap[1][2] = rightShoulderCount;
    }
}
