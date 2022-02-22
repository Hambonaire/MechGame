using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Pattern", menuName = "Mech/Pattern")]
public class MechPattern : ScriptableObject
{
    public string patternName;

    [Range(1, 5)]
    public int tier;

    public int price = 100000;

    public Mech mech = new Mech();

}
