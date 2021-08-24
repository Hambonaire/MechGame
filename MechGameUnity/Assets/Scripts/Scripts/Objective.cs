using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum objectiveType { Target, Defend_Time, Defend_Destroy, Travel, Escort }

//[CreateAssetMenu(fileName = "Objective")]
public class Objective : MonoBehaviour
{
    public GameObject player;

    public objectiveType thisObjectiveType;

    public List<GameObject> targets;

    public List<GameObject> allies;

    public Vector3 location;

    public float duration;

    float startTime;
    float endTime;

    public string objectiveText;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Initialize()
    {
        startTime = Time.time;

        endTime = startTime + duration;
    }

    public bool ObjectiveCompleted()
    {
        if (thisObjectiveType == objectiveType.Target)
        {
            for (int index = 0; index < targets.Count; index++)
            {
                if (targets[index] != null)
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
            for (int index = 0; index < targets.Count; index++)
            {
                if (targets[index] == null || Vector3.Distance(location, allies[index].transform.position) > 4)
                {
                    return false;
                }
            }

            return true;
        }
        else if (thisObjectiveType == objectiveType.Defend_Destroy)
        {
            for (int index = 0; index < targets.Count; index++)
            {
                if (targets[index] != null)
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
        else if (thisObjectiveType == objectiveType.Defend_Time)
        {
            if (endTime > Time.time) return false;

            for (int index = 0; index < targets.Count; index++)
            {
                if (targets[index] == null)
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

}
