using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Vector3", menuName = "ScriptableVariable/Vector3")]
public class ScriptableVector3 : ScriptableObject
{
    public Vector3 value = Vector3.zero;
}