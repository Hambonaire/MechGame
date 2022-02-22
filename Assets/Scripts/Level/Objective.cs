using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ObjectiveType { Target, DefendTime, DefendLocation, DefendDestroy, Travel, Escort }

[System.Serializable]
public class Objective
{
    [HideInInspector]
    public MechManager player;

    public ObjectiveType thisObjectiveType;

    [Range(1, 5)]
    public int difficulty;

    [Header("Target & Defend")]
    /* If a pattern ref is null in the list a random mechs will be built based on the list size*/
    public List<LevelMechPlaceholder> targetPatterns;
    public List<LevelMechPlaceholder> defendPatterns;
    [HideInInspector]
    public List<MechManager> targetMechs;
    [HideInInspector]
    public List<MechManager> defendMechs;

    //[Header("Escort & Protect")]
    public Collider location;
    public float duration;
    float startTime;
    float endTime;

    public string objectiveText;

    public void InitializeTimers()
    {
        startTime = Time.time;

        endTime = startTime + duration;
    }

    /*
     * 1    - Objective complete
     * 0    - Objective in-progress
     * -1   - Objective failed
     */
    public int ObjectiveCompleted()
    {
        switch (thisObjectiveType)
        {
            case ObjectiveType.Target:
                return targetMechs.All(mech => mech.totallyDestroyed) ? 1 : 0;

            case ObjectiveType.DefendDestroy:

                if (defendMechs.All(mech => mech.totallyDestroyed))
                    return -1;

                return targetMechs.All(mech => mech.totallyDestroyed) ? 1 : 0;

            case ObjectiveType.DefendTime:
                if (defendMechs.All(mech => mech.totallyDestroyed))
                    return -1;

                return (endTime > Time.time) ? 1: 0;

            case ObjectiveType.DefendLocation:
                // TODO: Return if enemy in location for too long

                return (endTime > Time.time) ? 1 : 0;

            case ObjectiveType.Escort:
                if (defendMechs.All(mech => mech.totallyDestroyed))
                    return -1;

                return defendMechs.All(mech => Vector3.Distance(location.transform.position, mech.transform.position) < 10) ? 1 : 0;

            case ObjectiveType.Travel:
                return (Vector3.Distance(player.transform.position, location.transform.position) < 5) ? 1 : 0;

        }

        return 0;
    }

}
