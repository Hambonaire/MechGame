using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Faction { player, enemy};

/*
 *	Level Manager
 *	- Handles spawning entities after load
 *	- Handles objectives
 */
public class LevelManager : MonoBehaviour
{
    public static LevelManager _instance;

    public Camera playerCam;

	public Transform playerSpawn;

    public GameObject playerMech;

    public List<GameObject> hostileMechs = new List<GameObject>();

    public List<Objective> levelObjectives = new List<Objective>();
    int objectiveIndex = 0;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
    
        for (int index = 0; index < levelObjectives.Count; index++)
        {
            levelObjectives[index].player = playerMech;
            if (index == 0)
                InitializeObjective(levelObjectives[0]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void SpawnPlayer()
	{
		MechBuilder builder = new MechBuilder();

        playerMech = builder.BuildFromMechObj(GameManager._instance.availableMechs[0], Vector3.zero, true, true, false);

        //playerMech.GetComponent<PlayerController>().AttachCamera(playerCam);
    }

    void SpawnHostiles()
    {

    }

    public void CompleteCurrentObjective()
    {

    }

    public void InitializeObjective(Objective objective)
    {
        objective.Initialize();

        MechBuilder builder = new MechBuilder();

        if (objective.thisObjectiveType == objectiveType.Target)
        {
            for(int index = 0; index < objective.targetPatterns.Count; index++)
            {
                if (objective.targetPatterns[index] != null)
                {
                    builder.BuildFromMechObj(objective.targetPatterns[index].mech, Vector3.forward * 10, true);
                }
                else
                {

                }
            }
        }
    }

    public List<GameObject> GetAllEntitiesInRange(GameObject target, float radius, Faction faction)
    {
        List<GameObject> entitiesInRange = new List<GameObject>();

        if (faction == Faction.enemy)
        {
            for (int index = 0; index < hostileMechs.Count; index++)
            {
                if (Vector3.Distance(target.transform.position, hostileMechs[index].transform.position) < radius)
                    entitiesInRange.Add(hostileMechs[index]);
            }

        }

        return entitiesInRange;
    }

    public GameObject GetClosestEntityInRange(GameObject target, float radius, Faction faction)
    {
        GameObject entity = null;

        if (faction == Faction.enemy)
        {
            float closestDist = radius;
            for (int index = 0; index < hostileMechs.Count; index++)
            {
                float dist = Vector3.Distance(target.transform.position, hostileMechs[index].transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    entity = hostileMechs[index];
                }
            }

        }

        return entity;
    }

}
