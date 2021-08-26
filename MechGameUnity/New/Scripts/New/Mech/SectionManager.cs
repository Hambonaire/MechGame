using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Section Manager holds 
 *  - Refs to a model's weapon link positions
 *  - Refs to a model's section transforms (for getting destroyed?)
 *  - Refs to each section's SectionStats
 */
[RequiresComponent(typeof(MechManager))]
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
    Section torsoSection;
    [SerializeField]
    Section headSection;
    [SerializeField]
    Section leftLegSection;
    [SerializeField]
    Section rightLegSection;
    [SerializeField]
    Section leftArmSection;
    [SerializeField]
    Section rightArmSection;
    [SerializeField]
    Section leftShoulderSection;
    [SerializeField]
    Section rightShoulderSection;

    [Header ("Weapon Link Transforms")]
    [SerializeField]
    Transform[] leftArmLinks;
    [SerializeField]
    Transform[] rightArmLinks;
    [SerializeField]
    Transform[] leftShoulderLinks;
    [SerializeField]
    Transform[] rightShoulderLinks;

    void Start()
    {
        if (torsoSection != null)
            torsoSection.isDestructible = false;

        if (headSection != null)
            headSection.isDestructible = true;

        if (leftLegSection != null)
            leftLegSection.isDestructible = false;

        if (rightLegSection != null)
            rightLegSection.isDestructible = false;

        if (leftArmSection != null)
            leftArmSection.isDestructible = true;

        if (rightArmSection != null)
            rightArmSection.isDestructible = true;

        if (leftShoulderSection != null)
            leftShoulderSection.isDestructible = true;

        if (rightShoulderSection != null)
            rightShoulderSection.isDestructible = true;

    }

    public Transform[] GetSectionLinksByIndex(int index)
    {
        if (index == 4)
            return leftArmLinks;
        else if (index == 5)
            return rightArmLinks;
        else if (index == 6)
            return leftShoulderLinks;
        else if (index == 7)
            return rightShoulderLinks;
        else
            return null;
    }
    
    public Section GetSectionByIndex(int index)
    {
        if (index == 0)
            return torsoSection;
        else if (index == 1)
            return headSection;
        else if (index == 2)
            return leftLegSection;
        else if (index == 3)
            return rightLegSection;
        else if (index == 4)
            return leftArmSection;
        else if (index == 5)
            return rightArmSection;
        else if (index == 6)
            return leftShoulderSection;
        else if (index == 7)
            return rightShoulderSection;
        else
            return null;
    }
}
