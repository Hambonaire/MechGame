using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechController : MonoBehaviour
{
    protected Stats myStats;
    protected MechManager manager;

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
    public GameObject torsoRoot;
    public GameObject legsRoot;
    protected float overallScaleFactor = 1;

    protected Datatype_Weapon[] weapon_data = new Datatype_Weapon[4];

    protected GameObject cockpit;
    protected GameObject legs;

    public GameObject MyTarget
    {
        get { return myTarget; }
        set { myTarget = value; }
    }

    public GameObject TorsoRoot
    {
        get { return torsoRoot; }
    }

    public float OverallScaleFactor
    {
        get { return overallScaleFactor; }
        set { overallScaleFactor = value; }
    }
}
