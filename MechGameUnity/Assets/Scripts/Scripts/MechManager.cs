using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechManager : MonoBehaviour {

    EquipmentManager equipRef;
    InventoryManager invRef;

    public Weapon[] weaponItems;
    public Cockpit[] cockpits;
    public Legs[] legs;

    public GameObject[] enemyArray;
    public List<GameObject> enemyList;

    public GameObject player;

    private float shortestEnemyDistance = 0;
    private GameObject closestTarget;

    public ScriptableBool autoTarget;
    public ScriptableFloat targetMaxDistance;
    public GameObject playerTarget;
    public ScriptableFloat playerTargetDistance;
    public ScriptableVector3 playerTargetTransform;

    int i;
    int k;

    // Use this for initialization
    void Start ()
    {
        equipRef = EquipmentManager.Instance;
        invRef = InventoryManager.Instance;
        //player.GetComponent<PlayerController>().Rebuild();

        i = 0;
        k = 0;

        //Variables.MaxTargetDistance = 100f;
        //targetMaxDistance.value = 100f;

        enemyArray = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in enemyArray)
        {
            enemy.GetComponent<MechController>().MyTarget = player;
        }

        InvokeRepeating("GetEnemyToTarget", 0.0f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.F))
        {
            i = Random.Range(0, weaponItems.Length);
            k = Random.Range(0, weaponItems.Length);
            //player.GetComponent<PlayerController>().SetNewWeapons(weaponItems[i], weaponItems[k]);

            i = Random.Range(0, cockpits.Length);
            //player.GetComponent<PlayerController>().SetNewCockpit(cockpits[i]);

            i = Random.Range(0, legs.Length);
            //player.GetComponent<PlayerController>().SetNewLegs(legs[i]);
        }

	}

    // TODO: Might move to UI_inGame or AutoTargetUI... probably a better fit
    void GetEnemyToTarget()
    {
        playerTarget = null;
        player.GetComponent<PlayerController>().MyTarget = playerTarget;

        if (autoTarget.value)
        {
            foreach (GameObject enemy in enemyArray)
            {
                if (enemy != null && enemy.activeSelf)
                {
                    float tempDist = Vector3.Distance(player.transform.position, enemy.transform.position);
                    enemy.GetComponent<EnemyController>().playerObjectDistance.value = tempDist;

                    Vector3 screenPoint = Camera.main.WorldToViewportPoint(enemy.transform.position);
                    if (screenPoint.z > 0 && screenPoint.x > 0.3 && screenPoint.x < .7 && screenPoint.y > 0.3 && screenPoint.y < .7)
                    {

                        if (shortestEnemyDistance == 0 && tempDist <= targetMaxDistance.value)
                        {
                            closestTarget = enemy;
                            shortestEnemyDistance = tempDist;

                            playerTarget = closestTarget;
                            playerTargetDistance.value = tempDist;
                            player.GetComponent<PlayerController>().MyTarget = playerTarget;
                            //playerTargetTransform.value = closestTarget.GetComponent<EnemyController>().getCenterMass();
                            //Variables.HasTarget = true;
                        }
                        else if (tempDist < shortestEnemyDistance && tempDist <= targetMaxDistance.value)
                        {
                            closestTarget = enemy;
                            shortestEnemyDistance = tempDist;

                            playerTarget = closestTarget;
                            playerTargetDistance.value = tempDist;
                            player.GetComponent<PlayerController>().MyTarget = playerTarget;
                            //playerTargetTransform.value = closestTarget.GetComponent<EnemyController>().getCenterMass();
                            //Variables.HasTarget = true;
                        }
                    }
                }

            }
        }

        shortestEnemyDistance = 0;
    }
}
