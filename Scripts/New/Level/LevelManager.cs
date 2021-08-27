using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *	Level Manager
 *	- Handles spawning entities after load
 *	- Handles objectives
 */
public class LevelManager : MonoBehaviour
{
	public Transform playerSpawn;
	
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void SpawnPlayer()
	{
		MechBuilder builder = new MechBuilder();

        GameObject playerMech = builder.BuildFromMechObj(GameManager._instance.availableMechs[0], Vector3.zero, true, true, false);
	}
}
