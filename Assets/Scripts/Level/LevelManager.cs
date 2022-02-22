using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;

/*
 *	Level Manager
 *	- Handles spawning entities after load
 *	- Handles objectives
 */
public class LevelManager : MonoBehaviour
{
    private bool firstPass = false;

    public static LevelManager _instance;

    LevelMechSpawner levelMechSpawner;

	public Transform playerSpawn;

    [HideInInspector]
    public MechManager playerMechManager;

    LevelMechPlaceholder[] placeHolders;

    [HideInInspector]
    public List<MechManager> neutralMechs = new List<MechManager>();
    [HideInInspector]
    public List<MechManager> playerMechs = new List<MechManager>();
    [HideInInspector]
    public List<MechManager> hostileMechs = new List<MechManager>();

    public List<Objective> levelObjectives = new List<Objective>();
    int objectiveIndex = 0;

    float timer = 0;

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

    }

    // Update is called once per frame
    void Update()
    {
        if (!firstPass)
        {
            InitializeLevel();
            firstPass = true;
        }

        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            timer = 0;
            CheckCurrentObjective();
        }
    }

    void InitializeLevel()
    {
        placeHolders = FindObjectsOfType<LevelMechPlaceholder>();
        MechBuilder builder = new MechBuilder();
        foreach (LevelMechPlaceholder mech in placeHolders)
        {
            mech.InitializeMech(builder);
        }

        levelMechSpawner = GetComponent<LevelMechSpawner>();
        levelMechSpawner.levelManager = this;

        playerMechManager = SpawnPlayer();

        for (int index = 0; index < levelObjectives.Count; index++)
        {
            levelObjectives[index].player = playerMechManager;
            if (index == 0)
                InitializeObjective(levelObjectives[0]);
        }
    }

    void CompleteCurrentObjective()
    {
        objectiveIndex++;

        if (objectiveIndex >= levelObjectives.Count)
        {
            print("Level Completed");
            // Level Over
        }
        else
            InitializeObjective(levelObjectives[objectiveIndex]);
    }

    void CheckCurrentObjective()
    {
        if (objectiveIndex < levelObjectives.Count)
        {
            switch (levelObjectives[objectiveIndex].ObjectiveCompleted())
            {
                case 1:
                    print("Objective Complete");
                    CompleteCurrentObjective();
                    break;
                case 0:
                    break;
                case -1:
                    print("Objective Failed");
                    CompleteCurrentObjective();
                    break;
            }
        }
    }

    void InitializeObjective(Objective objective)
    {
        foreach (LevelMechPlaceholder mech in placeHolders)
        {
            if (mech.enableOnObjective == objectiveIndex)
                mech.initializedMechManager.gameObject.SetActive(true);
        }

        foreach (LevelMechPlaceholder objMech in objective.targetPatterns)
        {
            hostileMechs.Add(objMech.initializedMechManager);
            objective.targetMechs.Add(objMech.initializedMechManager);
            objMech.initializedMechManager.gameObject.SetActive(true);
        }

        foreach(LevelMechPlaceholder objMech in objective.defendPatterns)
        {
            playerMechs.Add(objMech.initializedMechManager);
            objective.defendMechs.Add(objMech.initializedMechManager);
            objMech.initializedMechManager.gameObject.SetActive(true);
        }

        switch (objective.thisObjectiveType)
        {
            case ObjectiveType.Target:

                break;
            case ObjectiveType.DefendDestroy:

                break;
            case ObjectiveType.DefendTime:

                objective.InitializeTimers();
                break;
            case ObjectiveType.DefendLocation:

                objective.InitializeTimers();
                break;
            case ObjectiveType.Escort:

                break;
            case ObjectiveType.Travel:

                break;
        }

        SpawnLevelMechs();

        //foreach (HUDManager manager in playerHUDs)
        //    manager.InitObjective(objective);

        LevelUIManager._instance.InitObjectiveHUDs(objective);
    }

    MechManager SpawnPlayer()
	{
		MechBuilder builder = new MechBuilder();

        var playerMech = builder.BuildFromMechObj(GameManager._instance.availableMechs[GameManager._instance.selectedMechIndex], playerSpawn.position, Quaternion.identity,true, true, Faction.Player, BehaviorType.Neutral, IntelligenceLevel.Recruit);
        var playerMechManager = playerMech.GetComponent<MechManager>();
        var playerController = playerMech.GetComponent<PlayerController>();

        LevelUIManager._instance.AttachUIElementsToPlayer(playerController);

        /*
        // Spawn a HUD Canvas & Camera
        //Camera playerCamera = Instantiate(playerCamPrefab).GetComponent<Camera>();
        CinemachineVirtualCamera vCam = Instantiate(playerCamPrefab).GetComponent<CinemachineVirtualCamera>();
        HUDManager playerHUD = Instantiate(playerHUDPrefab).GetComponent<HUDManager>();
        playerController.AttachCamera(Camera.main, vCam, playerHUD);

        playerHUD.globalVolume = globalPPVolume;
        playerHUD.vCam = vCam;

        //playerHUD.Initialize();
        */
        playerMechs.Add(playerMechManager);
        //playerHUDs.Add(playerHUD);

        return playerMechManager;
    }

    // Spawn non-objective mechs
    void SpawnLevelMechs()
    {
        levelMechSpawner.SpawnFromLevelMech(objectiveIndex);
    }

    public void AddMechToLists(MechManager mech)
    {
        switch(mech.mechFaction)
        {
            case Faction.Player:
                playerMechs.Add(mech);
                break;
            case Faction.Hostile:
                hostileMechs.Add(mech);
                break;
            case Faction.Neutral:
                neutralMechs.Add(mech);
                break;
        }
    }

    // Faction is YOUR faction, so it is exclusive
    public List<GameObject> GetAllEntitiesInRange(GameObject target, float radius, Faction faction)
    {
        List<GameObject> entitiesInRange = new List<GameObject>();

        if (faction == Faction.Hostile)
        {
            for (int index = 0; index < playerMechs.Count; index++)
            {
                if (Vector3.Distance(target.transform.position, playerMechs[index].transform.position) < radius)
                    entitiesInRange.Add(playerMechs[index].gameObject);
            }
            for (int index = 0; index < neutralMechs.Count; index++)
            {
                if (Vector3.Distance(target.transform.position, neutralMechs[index].transform.position) < radius)
                    entitiesInRange.Add(neutralMechs[index].gameObject);
            }
        }
        else if (faction == Faction.Player)
        {
            for (int index = 0; index < hostileMechs.Count; index++)
            {
                if (Vector3.Distance(target.transform.position, hostileMechs[index].transform.position) < radius)
                    entitiesInRange.Add(hostileMechs[index].gameObject);
            }
        }
        else if (faction == Faction.Neutral)
        {
            for (int index = 0; index < hostileMechs.Count; index++)
            {
                if (Vector3.Distance(target.transform.position, hostileMechs[index].transform.position) < radius)
                    entitiesInRange.Add(hostileMechs[index].gameObject);
            }
        }

        return entitiesInRange;
    }

    public List<MechManager> GetFactionMechsInRange(Vector3 position, float radius, Faction faction)
    {
        List<MechManager> entitiesInRange = new List<MechManager>();

        if (faction == Faction.Hostile)
        {
            for (int index = 0; index < hostileMechs.Count; index++)
            {
                if (Vector3.Distance(position, hostileMechs[index].transform.position) < radius)
                    entitiesInRange.Add(hostileMechs[index]);
            }
        }
        else if (faction == Faction.Player)
        {
            for (int index = 0; index < playerMechs.Count; index++)
            {
                if (Vector3.Distance(position, playerMechs[index].transform.position) < radius)
                    entitiesInRange.Add(playerMechs[index]);
            }
        }
        else if (faction == Faction.Neutral)
        {
            for (int index = 0; index < neutralMechs.Count; index++)
            {
                if (Vector3.Distance(position, neutralMechs[index].transform.position) < radius)
                    entitiesInRange.Add(neutralMechs[index]);
            }
        }

        return entitiesInRange;
    }

    public GameObject GetClosestEntityInRange(GameObject target, float radius, Faction faction)
    {
        GameObject entity = null;

        if (faction == Faction.Hostile)
        {
            float closestDist = radius;
            for (int index = 0; index < playerMechs.Count; index++)
            {
                float dist = Vector3.Distance(target.transform.position, playerMechs[index].transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    entity = playerMechs[index].gameObject;
                }
            }
            for (int index = 0; index < neutralMechs.Count; index++)
            {
                float dist = Vector3.Distance(target.transform.position, neutralMechs[index].transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    entity = neutralMechs[index].gameObject;
                }
            }
        }
        if (faction == Faction.Player)
        {
            float closestDist = radius;
            for (int index = 0; index < hostileMechs.Count; index++)
            {
                float dist = Vector3.Distance(target.transform.position, hostileMechs[index].transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    entity = hostileMechs[index].gameObject;
                }
            }
        }
        if (faction == Faction.Neutral)
        {
            float closestDist = radius;
            for (int index = 0; index < hostileMechs.Count; index++)
            {
                float dist = Vector3.Distance(target.transform.position, hostileMechs[index].transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    entity = hostileMechs[index].gameObject;
                }
            }
        }

        return entity;
    }

}
