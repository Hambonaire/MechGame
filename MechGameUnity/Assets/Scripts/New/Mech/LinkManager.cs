using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkManager : MonoBehaviour
{
    [SerializeField]
    Transform[] leftArmLinks;
    [SerializeField]
    Transform[] rightArmLinks;
    [SerializeField]
    Transform[] leftShoulderLinks;
    [SerializeField]
    Transform[] rightShoulderLinks;

    public Transform[] GetSectionLinksByIndex(int index)
    {
        if (index == 3)
            return leftArmLinks;
        else if (index == 4)
            return rightArmLinks;
        else if (index == 5)
            return leftShoulderLinks;
        else if (index == 6)
            return rightShoulderLinks;
        else
            return null;
    }
}
