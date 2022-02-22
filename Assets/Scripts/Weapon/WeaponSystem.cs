using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponGroup
{
	public List<WeaponExecutable> mappedWeapons = new List<WeaponExecutable>();

	public WeaponGroup()
    {

    }
}

/*
 *	
 *	
 *	
 */
public class WeaponSystem : MonoBehaviour
{
	MechManager mechManager;
	MechController mechController;

	[HideInInspector]
	public HUDManager controllerHUD;
	[HideInInspector]
	public MechManager target;
	[HideInInspector]
	public Transform targetCenter;

	[HideInInspector]
	public int selectedGroup;
	public List<WeaponGroup> weaponGroups = new List<WeaponGroup>();// = new WeaponGroup[]{ new WeaponGroup(), new WeaponGroup(), new WeaponGroup() };

	public List<WeaponExecutable> weaponExecutables = new List<WeaponExecutable>();
	//public List<WeaponExecutable> missileExecutables = new List<WeaponExecutable>();

	public bool lockOnCapable;
	public Vector3 optimalRanges = new Vector3(10, 20, 40);

	public float heat;

	[Range(0, 1)]
	float lockOnProgress = 0;
	float lockOnTime = 3;
	
    // Start is called before the first frame update
    void Start()
    {
		mechManager = GetComponent<MechManager>();
		mechController = GetComponent<MechController>();

		weaponGroups.Add(new WeaponGroup());
		weaponGroups.Add(new WeaponGroup());
		weaponGroups.Add(new WeaponGroup());

		for (int secIndex = (int)SectionIndex.leftArm; secIndex < (int)SectionIndex.rightShoulder; secIndex++)
        {
			weaponExecutables.AddRange(mechManager.GetExecutableByIndex(secIndex));
		}

		for (int i = 0; i < weaponExecutables.Count; i++)
        {
			weaponExecutables[i].refSystem = this;
			weaponGroups[(int)weaponExecutables[i].weaponItemRef.weaponClass].mappedWeapons.Add(weaponExecutables[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
		//if (lockOnCapable)
		//	LockOnTarget();
	}

	public void FireExecutables(Vector3 firingSolution)
    {
		if (target == null || !target.gameObject.activeSelf)
			FindTargetInView();

		for (int execIndex = 0; execIndex < weaponExecutables.Count; execIndex++)
		{
			if (weaponExecutables[execIndex].gameObject.activeInHierarchy)
				weaponExecutables[execIndex].Fire(targetCenter, firingSolution);
		}
	}

	public void FireByGroup(int group, Vector3 firingSolution)
    {
		if (0 <= group && group < 3)
		{
			for (int i = 0; i < weaponGroups[group].mappedWeapons.Count; i++)
            {
				//if ((int)weaponExecutables[i].weaponItemRef.AmmoType == group)
				weaponGroups[group].mappedWeapons[i].Fire(targetCenter, firingSolution);
			}
		}
		else
			print("Group out of range");
    }

	/*
	public void FireBallisticExe(Vector3 firingSolution)
    {
		//Vector3 firingSolution = mechController.GetFiringSolutionPoint();

		for (int execIndex = 0; execIndex < ballisticExecutables.Count; execIndex++)
        {
			if (ballisticExecutables[execIndex].gameObject.activeInHierarchy)
				ballisticExecutables[execIndex].Fire(default, firingSolution);
		}
	}

	public void FireMissileExe()
    {
		if (target == null || !target.gameObject.activeSelf)
			FindTargetInView();
			
		for (int execIndex = 0; execIndex < missileExecutables.Count; execIndex++)
		{
			if (missileExecutables[execIndex].gameObject.activeInHierarchy)
				missileExecutables[execIndex].Fire(targetCenter, default);
		}
	}
	*/

	public void Reload()
    {

    }

	/* Camera from this mech */
	public MechManager FindTargetInView(float viewAngle = 50, float maxDist = 100)
	{
		var hostiles = LevelManager._instance.GetAllEntitiesInRange(gameObject, maxDist, mechManager.mechFaction);
		target = null;
		targetCenter = null;

		/* If hostile in view, then get the closest out of remaining */
		float smallestAngle = 360;
		GameObject closestHostile = null;
		foreach (GameObject hostile in hostiles)
        {
			if (!hostile.activeInHierarchy || hostile.GetComponent<MechManager>().totallyDestroyed)
				continue;

			//var dist = Vector3.Distance(transform.position, hostile.transform.position);
			var angleBetween = Vector3.Angle(mechManager.sectionManager.torsoRotAxis.transform.forward, (hostile.transform.position - transform.position));

			if (angleBetween < viewAngle && angleBetween < smallestAngle)
            {
				closestHostile = hostile;
				smallestAngle = angleBetween;
			}
		}

		if (closestHostile != null)
        {
			target = closestHostile.GetComponent<MechManager>();
			targetCenter = target.GetTargetCenter();
		}

		return target;
	}
	
	public bool CheckTargetInView(MechManager targetMech ,float viewAngle = 50, float maxDist = 100)
	{
		var dist = Vector3.Distance(transform.position, targetMech.transform.position);
		var angleBetween = Vector3.Angle(mechManager.sectionManager.torsoRotAxis.transform.forward, (targetMech.transform.position - transform.position));

		bool ret = (angleBetween < viewAngle && dist < maxDist);

		if (ret) target = targetMech;

		return ret;
	}

	void LockOnTarget()
	{
		if (FindTargetInView())
			lockOnProgress += lockOnTime / Time.deltaTime;
		else
			lockOnProgress -= lockOnTime / Time.deltaTime;
	}

	public void OnSuccessFire(float intensity)
    {
		mechController.OnWeaponFire(intensity);
    }
}
