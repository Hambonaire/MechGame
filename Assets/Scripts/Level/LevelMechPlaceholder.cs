using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMechPlaceholder : MonoBehaviour
{
    [HideInInspector]
    public MechManager initializedMechManager;

    public Faction faction;
    public BehaviorType behaviorType;
    public IntelligenceLevel aiLevel;

    public int enableOnObjective = 0;

    public MechPattern basePattern;

    [Header("Weapon Override")]
    public bool overridePattern;

    public void InitializeMech(MechBuilder builder)
    {
        initializedMechManager = builder.BuildFromMechObj(basePattern.mech, transform.position, transform.rotation, true, false, faction, behaviorType, aiLevel).GetComponent<MechManager>();
        initializedMechManager.gameObject.SetActive(false);
    }

}
