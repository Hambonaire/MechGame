using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameObject", menuName = "ScriptableVariable/GameObject")]
public class ScriptableGameObject : ScriptableObject
{
    public GameObject value = null;
}