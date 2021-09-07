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
	
	[SerializeField]
	GameObject target;

	public bool lockOnCapable;

	public List<WeaponExecutable> ballisticExecutables = new List<WeaponExecutable>();
	public List<WeaponExecutable> missileExecutables = new List<WeaponExecutable>();

	[Range(0, 1)]
	float lockOnProgress = 0;
	float lockOnTime = 3;
	
    // Start is called before the first frame update
    void Start()
    {
		mechManager = GetComponent<MechManager>();

		for (int secIndex = (int)SectionIndex.leftArm; secIndex < (int)SectionIndex.rightShoulder; secIndex++)
        {
			var execList = mechManager.GetExecutableByIndex(secIndex);

			for (int subIndex = 0; subIndex < execList.Count; subIndex++)
			{
				if (execList[subIndex].weaponItemRef.weaponType == WeaponType.Ballistic)
					ballisticExecutables.Add(execList[subIndex]);
				else if (execList[subIndex].weaponItemRef.weaponType == WeaponType.Missile)
					missileExecutables.Add(execList[subIndex]);
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (lockOnCapable)
			LockOnTarget();
	}

	public void FireBallisticExe(Vector3 firingSolution)
    {
		for (int execIndex = 0; execIndex < ballisticExecutables.Count; execIndex++)
        {
			ballisticExecutables[execIndex].Fire(firingSolution, default);

			//if ( == 1)
			//{
			//	StartCoroutine(ReloadCheckback(ballisticExecutables[execIndex], ballisticExecutables[execIndex].reloadTime));
			//}
		}
	}

	public void FireMissileExe(GameObject targetObject)
    {
		for (int execIndex = 0; execIndex < missileExecutables.Count; execIndex++)
		{
			missileExecutables[execIndex].Fire(default, targetObject);

			//if ( == 1)
			//{
			//	StartCoroutine(ReloadCheckback(missileExecutables[execIndex], missileExecutables[execIndex].reloadTime));
			//}
		}
	}

	/**
	public void OnCooldown()
	{
		for (int secIndex = (int)SectionIndex.leftArm; secIndex < (int)SectionIndex.rightShoulder; secIndex++)
		{
			var execList = mechManager.GetExecutableByIndex(secIndex);

			for (int subIndex = 0; subIndex < execList.Count; subIndex++)
			{
				execList[subIndex].OnCooldown();
			}
		}
	}

	public void Reload()
	{
		for (int secIndex = (int)SectionIndex.leftArm; secIndex < (int)SectionIndex.rightShoulder; secIndex++)
		{
			var execList = mechManager.GetExecutableByIndex(secIndex);

			for (int subIndex = 0; secIndex < execList.Count; secIndex++)
			{


				//if (execList[subIndex].Reload() == 1)
				//{
				//	StartCoroutine(ReloadCheckback(execList[subIndex], execList[subIndex].reloadTime));
				//}
			}
		}
	}
	*/

	/* Camera from this mech */
	bool HasTargetInView(Camera mechCam)
	{
		var hostiles = LevelManager._instance.GetAllEntitiesInRange(gameObject, 20, Faction.enemy);

		/* If hostile in view, then get the closest out of remaining */
		float closestDist = Mathf.Infinity;
		GameObject closestHostile = null;
		foreach (GameObject hostile in hostiles)
        {
			var wtvp = mechCam.WorldToViewportPoint(hostile.transform.position);

			if (!(0 <= wtvp.x && wtvp.x <= 1 && 0 <= wtvp.y && wtvp.y <= 1 && wtvp.z >= 1))
				continue;
			else if (Vector3.Distance(transform.position, hostile.transform.position) < closestDist)
				closestHostile = hostile;
		}

		target = closestHostile;
		return target != null;
	}
	
	void LockOnTarget()
	{
		if (HasTargetInView(null))
			lockOnProgress += lockOnTime / Time.deltaTime;
		else
			lockOnProgress -= lockOnTime / Time.deltaTime;
	}

	public GameObject GetMyTarget()
    {
		return target;
    }
	
	public void SetMyTarget(GameObject newTarget)
    {
		target = newTarget;
	}
}
