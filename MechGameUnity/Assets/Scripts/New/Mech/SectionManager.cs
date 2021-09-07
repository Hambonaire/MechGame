using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Section Manager holds 
 *  - Refs to a model's weapon link positions
 *  - Refs to a model's section transforms (for getting destroyed?)
 *  - Refs to each section's SectionStats
 */
[RequireComponent(typeof(MechManager))]
public class SectionManager : MonoBehaviour
{
    public Transform torsoRotAxis;
    public Transform armRotAxis;

    public Animator legsAnimator;

    public Vector3 cameraOffset;

    [Header ("Parent Transforms")]
    Transform torsoParent;
    Transform headParent;
    Transform leftLegParent;
    Transform rightLegParent;
    Transform LeftArmParent;
    Transform rightArmParent;
    Transform leftShoulderParent;
    Transform rightShoulderParent;

    [Header("Section Stats")]
    public Section torsoSection;
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
    Transform[] leftArmPrimaryLinks;
    [SerializeField]
    Transform[] leftArmSecondaryLinks;
    [SerializeField]
    Transform[] leftArmTertiaryLinks;

    [SerializeField]
    Transform[] rightArmPrimaryLinks;
    [SerializeField]
    Transform[] rightArmSecondaryLinks;
    [SerializeField]
    Transform[] rightArmTertiaryLinks;

    [SerializeField]
    Transform[] leftShoulderPrimaryLinks;
    [SerializeField]
    Transform[] leftShoulderSecondaryLinks;
    [SerializeField]
    Transform[] leftShoulderTertiaryLinks;

    [SerializeField]
    Transform[] rightShoulderPrimaryLinks;
    [SerializeField]
    Transform[] rightShoulderSecondaryLinks;
    [SerializeField]
    Transform[] rightShoulderTertiaryLinks;

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

    void Update()
    {
        if (torsoSection.isDestroyed)
        {
            Debug.Log(gameObject.name + " torso was destroyed!");

            gameObject.SetActive(false);
        }
    }

    public Transform[] GetSectionLinksByIndex(int index, int subindex)
    {
        if (index == 4 && subindex == 0)
            return leftArmPrimaryLinks;
        else if (index == 4 && subindex == 1)
            return leftArmSecondaryLinks;
        else if (index == 4 && subindex == 2)
            return leftArmTertiaryLinks;

        else if (index == 5 && subindex == 0)
            return rightArmPrimaryLinks;
        else if (index == 5 && subindex == 1)
            return rightArmSecondaryLinks;
        else if (index == 5 && subindex == 2)
            return rightArmTertiaryLinks;

        else if (index == 6 && subindex == 0)
            return leftShoulderPrimaryLinks;
        else if (index == 6 && subindex == 1)
            return leftShoulderSecondaryLinks;
        else if (index == 6 && subindex == 2)
            return leftShoulderTertiaryLinks;

        else if (index == 7 && subindex == 0)
            return rightShoulderPrimaryLinks;
        else if (index == 7 && subindex == 1)
            return rightShoulderSecondaryLinks;
        else if (index == 7 && subindex == 2)
            return rightShoulderTertiaryLinks;
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
