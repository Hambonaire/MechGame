using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelMechDirective { None, Patrol, Target, Defend };
[System.Serializable]
public struct LevelMech
{
    public Faction faction;

    public BehaviorType behaviorType;

    public IntelligenceLevel aiLevel;

    public MechPattern pattern;

    public LevelMechDirective mechDirective;

    public int onObjective;

    public Transform[] spawnLocations;

    [Header("Patrol")]
    public Transform[] route;
}

/*
 *  Handles spawning non-objective mechs in a level 
 *  (instead of having them also in objective or floating in levelManager
 */
public class LevelMechSpawner : MonoBehaviour
{
    // TODO: FIX THIS
    [HideInInspector]
    public LevelManager levelManager;

    public List<LevelMech> levelMechs = new List<LevelMech>();

    // Start is called before the first frame update
    void Start()
    {
        levelManager = LevelManager._instance;
    }

    public void SpawnFromLevelMech(int objectiveIndex)
    {
        MechBuilder builder = new MechBuilder();

        foreach (LevelMech mech in levelMechs)
        {
            if (mech.onObjective != objectiveIndex)
                continue;

            MechManager newMech = builder.BuildFromMechObj(mech.pattern.mech, Vector3.zero, Quaternion.identity, true, false, mech.faction, mech.behaviorType, mech.aiLevel).GetComponent<MechManager>();
            levelManager.AddMechToLists(newMech);

            var currObjective = levelManager.levelObjectives[objectiveIndex];

            switch (mech.mechDirective)
            {
                case LevelMechDirective.Patrol:
                    if (mech.route.Length > 0)
                    {
                        newMech.transform.position = mech.route[0].position;
                        newMech.GetComponent<AIController>().patrolRoute = mech.route;
                    }
                    break;
                case LevelMechDirective.Target:
                    newMech.transform.position = mech.spawnLocations[0].position;
                    newMech.transform.rotation = mech.spawnLocations[0].rotation;
                    if (mech.faction == Faction.Hostile)
                    {
                        if (currObjective.thisObjectiveType == ObjectiveType.DefendDestroy || currObjective.thisObjectiveType == ObjectiveType.DefendTime || currObjective.thisObjectiveType == ObjectiveType.Escort)
                        {
                            newMech.GetComponent<AIController>().destroyTarget = currObjective.defendMechs[0];
                        }
                        if (currObjective.thisObjectiveType == ObjectiveType.DefendLocation)
                        {
                            newMech.GetComponent<AIController>().targetLocation = currObjective.location.transform;
                        }
                    }
                    else if (mech.faction == Faction.Player)
                    {
                        if (currObjective.thisObjectiveType == ObjectiveType.Target || currObjective.thisObjectiveType == ObjectiveType.DefendDestroy)
                        {
                            newMech.GetComponent<AIController>().destroyTarget = currObjective.targetMechs[0];
                        }
                    }
                    break;
                case LevelMechDirective.Defend:
                    newMech.transform.position = mech.spawnLocations[0].position;
                    newMech.transform.rotation = mech.spawnLocations[0].rotation;
                    if (mech.faction == Faction.Hostile)
                    {
                        if (currObjective.thisObjectiveType == ObjectiveType.Target || currObjective.thisObjectiveType == ObjectiveType.DefendDestroy)
                        {
                            newMech.GetComponent<AIController>().destroyTarget = currObjective.targetMechs[0];
                        }
                    }
                    else if (mech.faction == Faction.Player)
                    {
                        if (currObjective.thisObjectiveType == ObjectiveType.DefendDestroy || currObjective.thisObjectiveType == ObjectiveType.DefendTime || currObjective.thisObjectiveType == ObjectiveType.Escort)
                        {
                            newMech.GetComponent<AIController>().defendTarget = currObjective.defendMechs[0];
                        }
                        if  (currObjective.thisObjectiveType == ObjectiveType.DefendLocation)
                        {
                            // TODO
                            newMech.GetComponent<AIController>().defendLocation = currObjective.location.transform;

                        }
                    }
                    break;
                case LevelMechDirective.None:
                    newMech.transform.position = mech.spawnLocations[0].position;
                    newMech.transform.rotation = mech.spawnLocations[0].rotation;
                    break;
            }
        }
    }
}
