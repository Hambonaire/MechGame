using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechManager : MonoBehaviour {

    public Weapon[] weaponItems;
    public Cockpit[] cockpits;
    public Legs[] legs;

    //public GameObject[] enemyList;
    public List<GameObject> enemyList;

    public GameObject player;

    private float shortestEnemyDistance = 0;
    private GameObject closestTarget;

    public ScriptableFloat targetMaxDistance;
    public ScriptableGameObject playerTarget;
    public ScriptableVector3 playerTargetTransform;

    int i;
    int k;

    // Use this for initialization
    void Start () {
        i = 0;
        k = 0;

        //Variables.MaxTargetDistance = 100f;
        targetMaxDistance.value = 100f;

        InvokeRepeating("GetEnemyToTarget", 0.0f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.F))
        {
            i = Random.Range(0, weaponItems.Length);
            k = Random.Range(0, weaponItems.Length);
            //player.GetComponent<Controller_Player>().SetNewWeapons(weaponItems[i], weaponItems[k]);

            i = Random.Range(0, cockpits.Length);
            //player.GetComponent<Controller_Player>().SetNewCockpit(cockpits[i]);

            i = Random.Range(0, legs.Length);
            //player.GetComponent<Controller_Player>().SetNewLegs(legs[i]);
        }

        if (playerTarget.value != null) //&& Variables.HasTarget)
        {
            //Variables.PlayerTargetTransform = Variables.PlayerTarget.GetComponent<EnemyController>().getCenterMass();
            playerTargetTransform.value = playerTarget.value.GetComponent<EnemyController>().getCenterMass();
        }

	}

    // TODO: Might move to UI_inGame or AutoTargetUI... probably a better fit
    void GetEnemyToTarget()
    {
        //Debug.Log("Repeating");

        //Variables.HasTarget = false;
        playerTarget.value = null;

        foreach (GameObject enemy in enemyList)
        {
            if (enemy != null && enemy.activeSelf)
            {
                //Debug.Log("Enemy !NULL");

                Vector3 screenPoint = Camera.main.WorldToViewportPoint(enemy.transform.position);
                if (screenPoint.z > 0 && screenPoint.x > 0.3 && screenPoint.x < .7 && screenPoint.y > 0.3 && screenPoint.y < .7)
                {
                    //Debug.Log("Enemy in Screen");

                    float tempDist = Vector3.Distance(player.transform.position, enemy.transform.position);

                    if (shortestEnemyDistance == 0 && tempDist <= targetMaxDistance.value)
                    {
                        closestTarget = enemy;
                        shortestEnemyDistance = tempDist;

                        playerTarget.value = closestTarget;
                        playerTargetTransform.value = closestTarget.GetComponent<EnemyController>().getCenterMass();
                        //Variables.HasTarget = true;
                    }
                    else if (tempDist < shortestEnemyDistance && tempDist <= targetMaxDistance.value)
                    {
                        closestTarget = enemy;
                        shortestEnemyDistance = tempDist;

                        playerTarget.value = closestTarget;
                        playerTargetTransform.value = closestTarget.GetComponent<EnemyController>().getCenterMass();
                        //Variables.HasTarget = true;
                    }
                }
            }
            
        }

        shortestEnemyDistance = 0;
    }
}
