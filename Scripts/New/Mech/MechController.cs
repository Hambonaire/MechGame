using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  -----Mech Controller-----
 *  The Mech controller is added to a mech model by the mechBuilder for gameplay
 *  
 *  Mech controller handles movement & weapon firing
 *  - Attaches to a mech's encapsulating GameObject
 *  - Should require MechManager & SectionManager to get their references
 *  - Act like an interface so Enemy Controller & Player Controller can both control mechs through this script
 */
public class MechController : MonoBehaviour
{
    protected MechManager mechManager;
    protected SectionManager sectionManager;

    protected Vector3 forward;

    protected GameObject myTarget;

    protected float gravity = -9.8f;
    [SerializeField]
    protected float walkSpeed = 5;
    [SerializeField]
    protected float runSpeed = 7;
    protected Vector3 velocity = Vector3.zero;
    protected float currentSpeed = 0;
    protected float targetSpeed = 0;
    protected float movementSmoothVelocity = 0;
    [SerializeField]
    protected float movementSmoothTime = 0.1f;
    protected bool movementEnabled = true;
    protected bool running;
    protected float turnSmoothVelocity_Torso = 0;
    [SerializeField]
    protected float turnSmoothTime_Torso = .25f;
    protected float turnSmoothVelocity_Legs = 0;
    [SerializeField]
    protected float turnSmoothTime_Legs = .02f;

    protected CapsuleCollider hitBox;
    protected float baseHitBoxRadius = 1f;
    protected float baseHitBoxHeight = 3f;
    protected float baseHitBoxCenter = 1.55f;
    
	protected Transform torsoRotAxis;
    protected Transform armRotAxis;

    //protected Datatype_Weapon[] weapon_data = new Datatype_Weapon[4];

    void Awake()
    {
    
    }

    protected void Start()
    {
        mechManager = GetComponent<MechManager>();
        sectionManager = GetComponent<SectionManager>();

        torsoRotAxis = sectionManager.torsoRotAxis;
        armRotAxis = sectionManager.armRotAxis;
    }
    
    void Update()
	{
		
	}

    protected void Fire()
    {
        for (int secIndex = (int) sectionIndex.leftArm; secIndex < (int) sectionIndex.rightShoulder; secIndex++)
        {
            var execList = mechManager.GetExecutableByIndex(secIndex);

			for (int subIndex = 0; subIndex < execList.Count; subIndex++)
			{
				if (execList[subIndex].Fire())
                {
                    StartCoroutine(ReloadCheckback(execList[subIndex], execList[subIndex].reloadTime));
                }
            }
        }
    }

    protected void Reload()
	{
		for (int secIndex = (int) sectionIndex.leftArm; secIndex < (int) sectionIndex.rightShoulder; secIndex++)
        {
            var execList = mechManager.GetExecutableByIndex(secIndex);

			for (int subIndex = 0; secIndex < execList.Count; secIndex++)
			{
				if (execList[subIndex].Reload());
                {
                    StartCoroutine(ReloadCheckback(execList[subIndex], execList[subIndex].reloadTime));
                }
			}
        }
	}

    IEnumerator ReloadCheckback(WeaponExecutable exec, float time)
    {
        yield return new WaitForSeconds(time + .01f);

        exec.Reload();
    }
}
