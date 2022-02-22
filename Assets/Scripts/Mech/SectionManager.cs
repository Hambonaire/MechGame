using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    //[Header("Section Stats")]

    public float TotalMaxHealth { get; private set; } = 0;
    public float TotalCurrentHealth { get; private set; } = 0;
    public bool CriticallyDamaged { get; private set; } = false;


    Section [] sectionsAsList;

    //[Header("Sections")]
    [HideInInspector]
    public Section torsoSection;
    Section headSection;
    Section legsSection;
    Section leftArmSection;
    Section rightArmSection;
    Section leftShoulderSection;
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

    //public UnityAction OnDamageTaken;
    public Action<MechManager, float> OnDamageTaken;

    void Start()
    {
        sectionsAsList = GetComponentsInChildren<Section>();
        foreach(Section sec in sectionsAsList)
        {
            TotalMaxHealth += sec.MaxHealth;
            // TODO: Is this called many times a frame? otherwise use bool and call invoke once a frame
            sec.OnDamageTaken += TakeDamageInvoke;

            if (sec.sectionType == SectionType.Head)
                headSection = sec;
            else if (sec.sectionType == SectionType.Torso)
                torsoSection = sec;
            else if (sec.sectionType == SectionType.Legs)
                legsSection = sec;
            else if (sec.sectionType == SectionType.LeftArm)
                leftArmSection = sec;
            else if (sec.sectionType == SectionType.RightArm)
                rightArmSection = sec;
            else if (sec.sectionType == SectionType.LeftShoulder)
                leftShoulderSection = sec;
            else if (sec.sectionType == SectionType.RightShoulder)
                rightShoulderSection = sec;
        }
    }

    void Update()
    {

    }

    public Transform[] GetSectionLinksByIndex(int index, int subindex)
    {
        if (index == (int)SectionIndex.leftArm && subindex == 0)
            return leftArmPrimaryLinks;
        else if (index == (int)SectionIndex.leftArm && subindex == 1)
            return leftArmSecondaryLinks;
        else if (index == (int)SectionIndex.leftArm && subindex == 2)
            return leftArmTertiaryLinks;

        else if (index == (int)SectionIndex.rightArm && subindex == 0)
            return rightArmPrimaryLinks;
        else if (index == (int)SectionIndex.rightArm && subindex == 1)
            return rightArmSecondaryLinks;
        else if (index == (int)SectionIndex.rightArm && subindex == 2)
            return rightArmTertiaryLinks;

        else if (index == (int)SectionIndex.leftShoulder && subindex == 0)
            return leftShoulderPrimaryLinks;
        else if (index == (int)SectionIndex.leftShoulder && subindex == 1)
            return leftShoulderSecondaryLinks;
        else if (index == (int)SectionIndex.leftShoulder && subindex == 2)
            return leftShoulderTertiaryLinks;

        else if (index == (int)SectionIndex.rightShoulder && subindex == 0)
            return rightShoulderPrimaryLinks;
        else if (index == (int)SectionIndex.rightShoulder && subindex == 1)
            return rightShoulderSecondaryLinks;
        else if (index == (int)SectionIndex.rightShoulder && subindex == 2)
            return rightShoulderTertiaryLinks;
        else
            return null;
    }
    
    public Section GetSectionByIndex(int index)
    {
        if (index == (int)SectionIndex.torso)
            return torsoSection;
        else if (index == (int)SectionIndex.head)
            return headSection;
        else if (index == (int)SectionIndex.legs)
            return legsSection;
        else if (index == (int)SectionIndex.leftArm)
            return leftArmSection;
        else if (index == (int)SectionIndex.rightArm)
            return rightArmSection;
        else if (index == (int)SectionIndex.leftShoulder)
            return leftShoulderSection;
        else if (index == (int)SectionIndex.rightShoulder)
            return rightShoulderSection;
        else
            return null;
    }

    public void TakeDamageInvoke(MechManager source, float damageTaken)
    {
        int damagedSections = 0;
        TotalCurrentHealth = 0;
        foreach(Section sec in sectionsAsList)
        {
            TotalCurrentHealth += sec.CurrentHealth;

            if (sec.CurrentHealthPercent < 0.333f)
            {
                damagedSections++;
                if (sec.IsCritical)
                    CriticallyDamaged = true;
            }
        }

        if (damagedSections >= 3)
            CriticallyDamaged = true;

        OnDamageTaken(source, damageTaken);
    }

    public float GetHealthPercent()
    {
        return TotalCurrentHealth / TotalMaxHealth;
    }
}
