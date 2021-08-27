using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *	
 *	
 *	
 */
public class WeaponSystem : MonoBehaviour
{
	MechManager mechManager;
	
	GameObject target;
	
	[Range(0, 100)]
	int lockOnProgress = 0;
	
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (FindTarget())
		{
			LockOnTarget();
		}
    }
	
	bool FindTarget()
	{
		Collider[] Physics.Spherecast();
	}
	
	void LockOnTarget()
	{
		
	}
}
