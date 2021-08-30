using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum objectiveType { Target, DefendTime, DefendDestroy, Travel, Escort }

[System.Serializable]
public class Objective
{
    [HideInInspector]
    public GameObject player;

    public objectiveType thisObjectiveType;

    [Range(1, 5)]
    public int difficulty;

    [Header("Destroy")]
    /* If a pattern ref is null in the list a random mechs will be built based on the list size*/
    public List<MechPattern> targetPatterns;
    [HideInInspector]
    public List<GameObject> targetObjects;

    [Header("Escort & Protect")]
    public List<GameObject> allies;
    public Vector3 location;
    public float duration;
    float startTime;
    float endTime;

    public string objectiveText;

    void Start()
    {
        
    }

    public void Initialize()
    {
        //startTime = Time.time;

        //endTime = startTime + duration;
    }

    public bool ObjectiveCompleted()
    {
        if (thisObjectiveType == objectiveType.Target)
        {
            for (int index = 0; index < targetObjects.Count; index++)
            {
                if (targetObjects[index] != null)
                {
                    return false;
                }
            }

            return true;
        }
        else if (thisObjectiveType == objectiveType.Travel)
        {
            if (Vector3.Distance(player.transform.position, location) < 4)
            {
                return false;
            }

            return true;
        }
        else if (thisObjectiveType == objectiveType.Escort)
        {
            for (int index = 0; index < targetObjects.Count; index++)
            {
                if (targetObjects[index] == null || Vector3.Distance(location, allies[index].transform.position) > 4)
                {
                    return false;
                }
            }

            return true;
        }
        else if (thisObjectiveType == objectiveType.DefendDestroy)
        {
            for (int index = 0; index < targetObjects.Count; index++)
            {
                if (targetObjects[index] != null)
                {
                    return false;
                }
            }

            for (int index = 0; index < allies.Count; index++)
            {
                if (allies[index] == null)
                {
                    return false;
                }
            }

            return true;
        }
        else if (thisObjectiveType == objectiveType.DefendTime)
        {
            if (endTime > Time.time) return false;

            for (int index = 0; index < targetObjects.Count; index++)
            {
                if (targetObjects[index] == null)
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

}
