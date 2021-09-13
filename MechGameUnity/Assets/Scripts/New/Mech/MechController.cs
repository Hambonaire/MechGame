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
[RequireComponent(typeof(MechManager))]
[RequireComponent(typeof(CharacterController))]
public class MechController : MonoBehaviour
{
    protected MechManager mechManager;
    protected SectionManager sectionManager;

    public Animator legsAnimator;
	
	protected Transform torsoRotAxis;
    protected Transform armRotAxis;

    protected Vector3 forward;

    [HideInInspector]
    public Camera mechCamera;

    protected float gravity = -9.8f;
    public float walkSpeed = 5;
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
    protected float turnSmoothTime_Legs = .25f;

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

        legsAnimator = sectionManager.legsAnimator;
    }
    
    void Update()
	{
		
	}

    protected void Fire()
    {
        mechManager.weaponSystem.FireBallisticExe();

        mechManager.weaponSystem.FireMissileExe();

        /**
        for (int secIndex = (int) SectionIndex.leftArm; secIndex < (int) SectionIndex.rightShoulder; secIndex++)
        {
            var execList = mechManager.GetExecutableByIndex(secIndex);

			for (int subIndex = 0; subIndex < execList.Count; subIndex++)
			{
				if (execList[subIndex].Fire(firingSolution))
                {
                    StartCoroutine(ReloadCheckback(execList[subIndex], execList[subIndex].reloadTime));
                }
            }
        }
        */
    }

    protected void Reload()
    {
        //mechManager.weaponSystem.Reload();
    }

    protected void OnCooldown()
    {
        //mechManager.weaponSystem.OnCooldown();
    }

    public virtual Vector3 GetFiringSolutionPoint()
    {
        return Vector3.zero;
    }

}
