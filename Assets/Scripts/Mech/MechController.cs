using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
    protected WeaponSystem weaponSystem;

    public GameObject cameraPlaceholder;
    public Camera mechCamera;
    public CinemachineVirtualCamera virtualCamera;

    protected Animator legsAnimator;
	
	protected Transform torsoRotAxis;
    protected Transform armRotAxis;

    protected float gravity = -9.8f;
    public float walkSpeed = 5;         // TODO: Fix this?
    //protected float runSpeed = 7;
	
    protected Vector3 velocity = Vector3.zero;
    protected float currentSpeed = 0;
    protected float targetSpeed = 0;
    protected float movementSmoothVelocity = 0;
    protected float movementSmoothTime = 0.1f;

    //protected bool movementEnabled = true;
    //protected bool running;

    protected Vector3 lastForward;

    protected float turnSmoothVelocity_Torso = 0;
    protected float turnSmoothTime_Torso = .25f;
    protected float turnSmoothVelocity_Legs = 0;
    protected float turnSmoothTime_Legs = .25f;

    protected float baseHitBoxRadius = 1f;
    protected float baseHitBoxHeight = 3f;
    protected float baseHitBoxCenter = 1.55f;
	
    public void Initialize()
    {
        mechManager = GetComponent<MechManager>();
        sectionManager = GetComponent<SectionManager>();
        weaponSystem = GetComponent<WeaponSystem>();
    }

    protected void Start()
    {
        Initialize();

        torsoRotAxis = sectionManager.torsoRotAxis;
        armRotAxis = sectionManager.armRotAxis;

        legsAnimator = sectionManager.legsAnimator;

        sectionManager.OnDamageTaken += SectionTookDamage;

        lastForward = transform.forward;
    }

    void Update()
	{
		
	}

    protected virtual void Move()
    {

    }

    protected virtual void Fire()
    {
        //weaponSystem.FireBallisticExe(GetFiringSolutionPoint());

        //weaponSystem.FireMissileExe();

        weaponSystem.FireExecutables(GetFiringSolutionPoint());
    }

    protected virtual void Reload()
    {
        mechManager.weaponSystem.Reload();
    }

    protected virtual void Animate() 
    {

    }

    protected void OffsetTorsoLegTurn(ref Vector3 lastForward)
    {
        // Torso Turning (y only) for cockpit object itself
        // Torso rotation -> Encapsulatin object rotation offset
        var curFwd = transform.forward;
        // measure the angle rotated since last frame:
        var angDiff = Vector3.Angle(curFwd, lastForward);
        if (angDiff > 0.01)
        {
            // if rotated a significant angle...
            // fix angle sign...
            if (Vector3.Cross(curFwd, lastForward).y < 0) angDiff = -angDiff;
            lastForward = curFwd; // and update lastFwd
        }
        torsoRotAxis.transform.eulerAngles = new Vector3(torsoRotAxis.transform.eulerAngles.x, torsoRotAxis.transform.eulerAngles.y + angDiff, torsoRotAxis.transform.eulerAngles.z);
    }

    public virtual Vector3 GetFiringSolutionPoint()
    {
        return Vector3.zero;
    }

    public virtual void OnWeaponFire(float intensity)
    {

    }

    protected virtual void SectionTookDamage(MechManager source, float damageTaken)
    {
        BecomeAlerted(source.transform.position);
    }

    public virtual void BecomeAlerted(Vector3 lastKnownPos)
    {

    }
}
