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
[RequireComponent(typeof(CharacterController))]
public class MechController : MonoBehaviour
{
    protected MechManager mechManager;
    protected SectionManager sectionManager;

    protected Animator legsAnimator;
	
	protected Transform torsoRotAxis;
    protected Transform armRotAxis;


    protected Vector3 forward;

    protected GameObject myTarget;



    protected float gravity = -9.8f;
    protected float walkSpeed = 5;
    //protected float runSpeed = 7;
	
    protected Vector3 velocity = Vector3.zero;
    protected float currentSpeed = 0;
    protected float targetSpeed = 0;
    protected float movementSmoothVelocity = 0;
    protected float movementSmoothTime = 0.1f;
	
    //protected bool movementEnabled = true;
    //protected bool running;
	
    protected float turnSmoothVelocity_Torso = 0;
    protected float turnSmoothTime_Torso = .25f;
    protected float turnSmoothVelocity_Legs = 0;
    protected float turnSmoothTime_Legs = .02f;

    protected float baseHitBoxRadius = 1f;
    protected float baseHitBoxHeight = 3f;
    protected float baseHitBoxCenter = 1.55f;
	
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
