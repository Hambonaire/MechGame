using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Section Manager holds 
 *  - Refs to a model's weapon link positions
 *  - Refs to a model's section transforms (for getting destroyed?)
 *  - Refs to each section's SectionStats
 */
public class SectionManager : MonoBehaviour
{
    [Header ("Parent Transforms")]
    [SerializeField]
    Transform torsoParent;
    [SerializeField]
    Transform headParent;
    [SerializeField]
    Transform leftLegParent;
    [SerializeField]
    Transform rightLegParent;
    [SerializeField]
    Transform LeftArmParent;
    [SerializeField]
    Transform rightArmParent;
    [SerializeField]
    Transform leftShoulderParent;
    [SerializeField]
    Transform rightShoulderParent;

    [Header("Section Stats")]
    [SerializeField]
    SectionStats torsoStats;
    [SerializeField]
    SectionStats headStats;
    [SerializeField]
    SectionStats leftLegStats;
    [SerializeField]
    SectionStats rightLegStats;
    [SerializeField]
    SectionStats leftArmStats;
    [SerializeField]
    SectionStats rightArmStats;
    [SerializeField]
    SectionStats leftShoulderStats;
    [SerializeField]
    SectionStats rightShoulderStats;

    [Header ("Weapon Link Transforms")]
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
