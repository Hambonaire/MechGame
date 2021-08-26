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
    protected float walkSpeed;
    protected float runSpeed;
    protected Vector3 velocity = Vector3.zero;
    protected float currentSpeed = 0;
    protected float targetSpeed = 0;
    protected float movementSmoothVelocity = 0;
    protected float movementSmoothTime = 0.1f;
    protected bool movementEnabled = true;
    protected bool running;
    protected float turnSmoothVelocity_Torso = 0;
    protected float turnSmoothTime_Torso = .025f;
    protected float turnSmoothVelocity_Legs = 0;
    protected float turnSmoothTime_Legs = .2f;

    protected CapsuleCollider hitBox;
    protected float baseHitBoxRadius = 1f;
    protected float baseHitBoxHeight = 3f;
    protected float baseHitBoxCenter = 1.55f;
    
	public Transform cockpitRotationCenter;


    //protected Datatype_Weapon[] weapon_data = new Datatype_Weapon[4];

    void Awake()
    {
    
    }

    void Start()
    {
    
    }
    
    void Update()
	{
		
	}

    void Fire()
    {	
        for (int secIndex = (int) sectionIndex.leftArm; secIndex < (int) sectionIndex.rightShoulder; secIndex++)
        {
			var execList = mechManager.GetExecutableByIndex(secIndex)

			for (int subIndex = 0; secIndex < execList.Count; secIndex++)
			{
				execList[subIndex].Fire();
			}
        }
    }
	
	Reload()
	{
		for (int secIndex = (int) sectionIndex.leftArm; secIndex < (int) sectionIndex.rightShoulder; secIndex++)
        {
			var execList = mechManager.GetExecutableByIndex(secIndex)

			for (int subIndex = 0; secIndex < execList.Count; secIndex++)
			{
				execList[subIndex].Reload();
			}
        }
	}
	
	// REMOVE???
	void OnCooldown()
    {
        for (int i = 0; i < weapon_data.Length; i++)
        {
            if (weapon_data[i] != null)
            {
                weapon_data[i].executable.OnCooldown();
            }
        }
    }
}
