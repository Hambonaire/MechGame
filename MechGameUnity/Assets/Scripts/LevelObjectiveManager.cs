using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelObjectiveManager : MonoBehaviour        
{
    public GameObject manager;

    public GameObject player;

    public GameObject[] objectives;
    private GameObject currentObjectiveObject;
    private Objective currentObjective;

    private int objectiveLength;
    private int currentObjectiveIndex = 0;

    public Text objectiveTextObject;

    private bool targetsCheck = false;

    public UnityEvent onEndLevel;// = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        objectiveLength = objectives.Length;
        currentObjectiveObject = objectives[0];
        currentObjectiveIndex = -1;

        AddMechsToManager();

        StartNextObjective();

        InvokeRepeating("ObjectiveUpdate", 0.0f, 0.5f);
    }

    void OnGUI()
    {
        objectiveTextObject.text = currentObjective.objectiveText;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (currentObjectiveIndex < objectiveLength)
        {
            currentObjective = objectives[currentObjectiveIndex];

            if (currentObjective.GetComponent<Objective>().thisObjectiveType == objectiveType.Target) // If completed go next if has next
            {
                if (currentObjective.GetComponent<Objective>().target == null)
                {
                    currentObjectiveIndex++;
                }
            }
            else if (currentObjective.GetComponent<Objective>().thisObjectiveType == objectiveType.Travel)
            {
                if (Vector3.Distance(player.transform.position, currentObjective.GetComponent<Objective>().location) < 3)
                {
                    currentObjectiveIndex++;
                }
            }
            
            else if ()
            {

            }
            
        }
        else
        {
            // Stop input first, wait, then set timescale

            Time.timeScale = 0;
        }
        */
    }

    void ObjectiveUpdate()
    {
        if (currentObjectiveIndex < objectiveLength)
        {
            if (currentObjective.thisObjectiveType == objectiveType.Target) // If completed go next if has next
            {
                targetsCheck = true;

                foreach(GameObject target in currentObjective.targets)
                {
                    if (target != null) targetsCheck = false;
                }

                if (targetsCheck)
                {
                    //currentObjectiveIndex++;
                    StartNextObjective();
                }
            }
            else if (currentObjective.thisObjectiveType == objectiveType.Travel)
            {
                if (Vector3.Distance(player.transform.position, currentObjective.location) < 3)
                {
                    //currentObjectiveIndex++;
                    StartNextObjective();
                }
            }
            /*
            else if (currentObjective.GetComponent<Objective>().thisObjectiveType == objectiveType.Protect)
            {

            }
            */
        }
        else
        {
            // Stop input first, wait, then set timescale
            EndLevel();

            Time.timeScale = 0;
        }
    }

    void StartNextObjective()
    {
        currentObjectiveIndex++;

        if (currentObjectiveIndex < objectiveLength)
        {
            currentObjectiveObject = objectives[currentObjectiveIndex];
            currentObjective = currentObjectiveObject.GetComponent<Objective>();

            if (currentObjective.thisObjectiveType == objectiveType.Target) // If completed go next if has next
            {
                /*
                foreach (GameObject target in currentObjective.targets)
                {
                    if (!target.activeSelf) target.SetActive(true);
                }
                */

                for (int index = 0; index < currentObjective.targets.Count; index++)
                {
                    currentObjective.targets[index].SetActive(true);
                }
            }
            else if (currentObjective.thisObjectiveType == objectiveType.Travel)
            {

            }
        }
        else
        {
            // End Level
            EndLevel(); 
        }
    }

    void EndLevel()
    {
        onEndLevel.Invoke();

        Time.timeScale = (0.1f);
    }

    void AddMechsToManager()
    {
        foreach (GameObject objective in objectives)
        {
            List<GameObject> tempTargets = objective.GetComponent<Objective>().targets;

            for (int index = 0; index < objective.GetComponent<Objective>().targets.Count; index++ )
            {
                manager.GetComponent<MechManager>().enemyList.Add(objective.GetComponent<Objective>().targets[index]);
            }
        }
    }
}
