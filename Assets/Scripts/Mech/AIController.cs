using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 *  Neutral     - Watch for target, engage if close enough, retreat on low health, chase target when its weak
 *  Aggressive  - Search for target, engage and chase when detected, do not retreat
 *  Defensive   - Watch for target, engage but keep distance, retreat on low health, does not chase
 */
public enum BehaviorType { Neutral, Aggressive, Defensive };

public enum BehaviorState { Idle, Patrol, Advance, Engage, Defend, Chase, Retreat};

public enum IntelligenceLevel { Recruit , Veteran, Elite };

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MechController
{
    NavMeshAgent navAgent;

    MechManager playerMech;

    // Behavior & State
    public IntelligenceLevel aiLevel = IntelligenceLevel.Recruit;
    public BehaviorType behaviorType = BehaviorType.Neutral;
    [SerializeField]
    BehaviorState currentState = BehaviorState.Idle;
    BehaviorState lastState = BehaviorState.Idle;
    float stateTimer = 0;

    bool alerted = false;
    [SerializeField]
    MechManager lastCombatTarget;
    public Vector3 lastKnownPosition = Vector3.zero;

    // in testing TODO
    bool disengage = false;
    bool isDisengaging = false;

    // Descending order of priority
    [HideInInspector]
    public MechManager destroyTarget;
    [HideInInspector]
    public MechManager defendTarget;
    [HideInInspector]
    public Transform targetLocation;
    [HideInInspector]
    public Transform defendLocation;
    [HideInInspector]
    public Transform[] patrolRoute = { };
    int patrolIndex = 0;

    // Firing 
    bool fireReset;
    bool isFiring;
    float firingTimer = 0;
    float minFiringAngle;

    new void Start()
    {
        base.Start();

        navAgent = GetComponent<NavMeshAgent>();

        playerMech = LevelManager._instance.playerMechManager;

        navAgent.speed = 3;
        navAgent.angularSpeed = 180;
    }

    void Update()
    {
        MechLogic();

        Move();

        Look();

        Fire();

        Animate();

        if (Input.GetKey(KeyCode.Space))
        {
            weaponSystem.FireExecutables(GetFiringSolutionPoint());
        }
    }

    void MechLogic()
    {
        FindTarget();

        // Reset timer if switched states last frame
        if (currentState != lastState)
            stateTimer = 0;
        else
            stateTimer += Time.deltaTime;

        switch (currentState)
        {
            case BehaviorState.Idle:
                lastState = BehaviorState.Idle;

                // Change priority on aggressiveness
                if (lastCombatTarget != null && !disengage)
                    currentState = BehaviorState.Engage;
                else if (lastCombatTarget == null && lastKnownPosition != Vector3.zero)
                {
                    currentState = BehaviorState.Advance;
                }
                else if (destroyTarget != null)
                {
                    currentState = BehaviorState.Advance;
                }
                else if (defendTarget != null)
                    currentState = BehaviorState.Defend;
                else if (targetLocation != null)        // TODO: Do maybe an APPROACH state? engage seems wrong here
                    currentState = BehaviorState.Advance;
                else if (defendLocation != null)
                    currentState = BehaviorState.Defend;
                else if (patrolRoute.Length > 0)
                    currentState = BehaviorState.Patrol;
                // No Objective but disengaging
                else if (disengage)
                {
                    disengage = false;
                    isDisengaging = false;
                }

                break;
            case BehaviorState.Patrol:  // Traverse patrol route
                lastState = BehaviorState.Patrol;

                if (lastCombatTarget != null)
                    currentState = BehaviorState.Idle;

                break;
            case BehaviorState.Advance:  // Move towards various targets
                lastState = BehaviorState.Advance;

                if (lastCombatTarget != null)
                    currentState = BehaviorState.Idle; // We have local target
                else if (lastKnownPosition == Vector3.zero && (targetLocation.position == Vector3.zero || targetLocation == null) && destroyTarget == null)
                    currentState = BehaviorState.Idle;

                break;
            case BehaviorState.Engage:  // Engage in combat with target, attempt to win
                lastState = BehaviorState.Engage;

                // TODO
                if (lastCombatTarget == null)
                {
                    lastState = BehaviorState.Idle;
                    break;
                }

                if (behaviorType == BehaviorType.Aggressive)
                    currentState = BehaviorState.Chase;

                /*
                 *  1. Check my health
                 *  2. Check target health
                 *  3. Check distance to target
                 */
                if (sectionManager.CriticallyDamaged)
                {
                    if (behaviorType == BehaviorType.Neutral || behaviorType == BehaviorType.Defensive)
                        currentState = BehaviorState.Retreat;
                }
                else if (lastCombatTarget.sectionManager.CriticallyDamaged)
                {
                    if (behaviorType == BehaviorType.Neutral)
                        currentState = BehaviorState.Chase;
                }
                else if (Vector3.Distance(transform.position, lastCombatTarget.transform.position) > weaponSystem.optimalRanges.z) // TODO: Some dist? Weapons optimal ranges?
                {
                    //TODO
                    //currentState = BehaviorState.Advance;
                    /*
                    if (behaviorType == BehaviorType.Neutral || behaviorType == BehaviorType.Defensive)
                    {
                        currentState = BehaviorState.Idle;
                        disengage = true;
                        isDisengaging = true;
                    }
                    */
                }

                break;
            case BehaviorState.Defend:
                lastState = BehaviorState.Defend;

                // TODO: chase, dont chase, retreat?
                if (lastCombatTarget != null || defendLocation == null || defendTarget == null)
                    currentState = BehaviorState.Idle;

                break;
            case BehaviorState.Chase:
                lastState = BehaviorState.Chase;

                // If neutral, break out depending on my health or target distance 
                // TODO: Lose sight of target?

                // TODO
                if (lastCombatTarget == null)
                {
                    currentState = BehaviorState.Idle;
                    break;
                }

                if (behaviorType == BehaviorType.Defensive)
                    currentState = BehaviorState.Engage;
                if (behaviorType == BehaviorType.Neutral)
                {
                    if (sectionManager.CriticallyDamaged)
                        currentState = BehaviorState.Retreat;
                    else if (Vector3.Distance(transform.position, lastCombatTarget.transform.position) > weaponSystem.optimalRanges.z)
                        currentState = BehaviorState.Idle;
                    // TODO: Disengage??
                }

                break;
            case BehaviorState.Retreat:
                lastState = BehaviorState.Retreat;

                // Break out based on target health or distance

                if (behaviorType == BehaviorType.Neutral)
                {
                    
                }

                break;
            default:
                currentState = BehaviorState.Idle;
                break;
        }
            
    }

    protected override void Move()
    {
        switch (currentState)
        {
            case BehaviorState.Idle:
                if (lastState != BehaviorState.Idle)
                {
                    navAgent.isStopped = true;
                }

                // Stand around? Some point in time move to random position?
                if (stateTimer > 6)
                {
                    MoveRandomPointInAngle(transform.position, 10, 360);
                    stateTimer = 0;
                }

                break;
            case BehaviorState.Patrol:
                if (lastState != BehaviorState.Patrol)
                {
                    navAgent.isStopped = false;
                    navAgent.SetDestination(patrolRoute[patrolIndex].position);
                }

                // If at destination next index
                if (Vector3.Distance(transform.position, patrolRoute[patrolIndex].position) < 1)
                {
                    patrolIndex++;

                    if (patrolIndex >= patrolRoute.Length)
                        patrolIndex = 0;

                    navAgent.SetDestination(patrolRoute[patrolIndex].position);
                }

                break;
            case BehaviorState.Advance:
                if (lastState != BehaviorState.Advance)
                {
                    navAgent.isStopped = false;
                    if (lastKnownPosition != Vector3.zero)
                    {
                        MoveRandomPointInAngle(lastKnownPosition, weaponSystem.optimalRanges.x, 180);
                    }
                    else if (destroyTarget != null)
                    {
                        MoveRandomPointInAngle(destroyTarget.transform.position, weaponSystem.optimalRanges.y, 60);
                    }
                    else if (targetLocation != null)
                    {
                        MoveRandomPointInAngle(targetLocation.position, weaponSystem.optimalRanges.y, 90);
                    }
                }

                // TODO
                break;
            case BehaviorState.Engage:
                // Try to keep target in optimal firing range
                // Move around while firing>
                if (lastState != BehaviorState.Engage)
                {
                    navAgent.isStopped = false;
                    MoveRandomPointInAngle(lastCombatTarget.transform.position, weaponSystem.optimalRanges.y, 60);
                }

                if (Vector3.Distance(transform.position, navAgent.destination) < 2 && (Vector3.Distance(transform.position, lastCombatTarget.transform.position) > weaponSystem.optimalRanges.y + 10 || stateTimer > 4))
                {
                    MoveRandomPointInAngle(lastCombatTarget.transform.position, weaponSystem.optimalRanges.y, 210);
                    stateTimer = 0;
                }

                break;
            case BehaviorState.Defend:
                // Move to somehwere around def target
                if (lastState != BehaviorState.Defend)
                {
                    navAgent.isStopped = false;
                    if (defendTarget != null)
                    {
                        MoveRandomPointInAngle(defendTarget.transform.position, weaponSystem.optimalRanges.y, 60);
                    }
                    else if (defendLocation != null)
                    {
                        MoveRandomPointInAngle(defendLocation.position, weaponSystem.optimalRanges.y, 120);
                    }
                }

                if (stateTimer > 8)
                {
                    if (defendTarget != null)
                    {
                        MoveRandomPointInAngle(defendTarget.transform.position, weaponSystem.optimalRanges.y, 60);
                    }
                    else if (defendLocation != null)
                    {
                        MoveRandomPointInAngle(defendLocation.position, weaponSystem.optimalRanges.y, 120);
                    }
                    stateTimer = 0;
                }

                break;
            case BehaviorState.Chase:
                // Get in closest optimal range
                if (lastState != BehaviorState.Chase)
                {
                    navAgent.isStopped = false;
                    MoveRandomPointInAngle(lastCombatTarget.transform.position, weaponSystem.optimalRanges.x, 0);
                }

                if (Vector3.Distance(transform.position, lastCombatTarget.transform.position) < weaponSystem.optimalRanges.x)
                {
                    MoveRandomPointInAngle(lastCombatTarget.transform.position, weaponSystem.optimalRanges.x, 30);
                }

                break;
            case BehaviorState.Retreat:
                if (lastState != BehaviorState.Retreat)
                {
                    navAgent.isStopped = false;
                    MoveRandomPointInAngle(lastCombatTarget.transform.position, weaponSystem.optimalRanges.z, 90);
                }

                if (Vector3.Distance(transform.position, navAgent.destination) < 1 && Vector3.Distance(transform.position, lastCombatTarget.transform.position) < weaponSystem.optimalRanges.y)
                {
                    MoveRandomPointInAngle(lastCombatTarget.transform.position, weaponSystem.optimalRanges.z, 90);
                }

                break;
        }
    }

    void Look()
    {
        OffsetTorsoLegTurn(ref lastForward);

        if (lastCombatTarget != null)   // TODO: Check distance here instead of other places?
        {
            AimAtTarget();
        }
        else if (alerted)
        {
            // Pan and scan for target
            torsoRotAxis.Rotate(0, 60 * Time.deltaTime, 0);
        }
        else
        {
            // Torso
            torsoRotAxis.localRotation = Quaternion.Slerp(torsoRotAxis.localRotation, Quaternion.identity, 5 * Time.deltaTime);

            // Arms
            armRotAxis.localRotation = Quaternion.Slerp(armRotAxis.localRotation, Quaternion.identity, 5 * Time.deltaTime);
        }

    }

    protected override void Fire()
    {
        // Check if target in range, etc.
        // Should already be in firing angle and [SOME] range since checking with weapon system
        // prob make/use functions in weapon system instead of here
        if (lastCombatTarget != null)
        {
            switch (aiLevel)
            {                   
                // If recruit level
                // Fire when target is in wider angle
                // Ignore range penalties
                // Ignore current weapon bloom
                case IntelligenceLevel.Recruit:

                    if (firingTimer > 3)
                    {
                        fireReset = true;
                    }
                    else if (firingTimer <= 0)
                    {
                        fireReset = false;
                    }

                    if (!fireReset && weaponSystem.CheckTargetInView(lastCombatTarget, 40f, weaponSystem.optimalRanges.z))
                    {
                        base.Fire();
                        isFiring = true;
                        firingTimer += Time.deltaTime;
                    }
                    else
                    {
                        isFiring = false;
                        firingTimer -= Time.deltaTime;
                    }
                    break;
                // 
                //
                case IntelligenceLevel.Veteran:

                    break;
            }



        }

        if (isFiring)
            firingTimer += Time.deltaTime;
        else
        {
            firingTimer -= Time.deltaTime;
            if (firingTimer < 0) firingTimer = 0;
        }
    }

    protected override void Animate()
    {
        base.Animate();

        // Leg Animation
        if (legsAnimator != null)
        {
            //legsAnimator.SetFloat("moveBlend", currentSpeed);
            legsAnimator.SetFloat("moveBlend", ((true) ? 0.25f : .7f) * navAgent.velocity.magnitude, movementSmoothTime, Time.deltaTime);
        }
    }

    void AimAtTarget()
    {
        if (lastCombatTarget != null)
        {
            // Torso
            Quaternion targetRot = Quaternion.LookRotation(lastCombatTarget.transform.position - transform.position);
            targetRot.x = 0;
            targetRot.z = 0;
            torsoRotAxis.rotation = Quaternion.Slerp(torsoRotAxis.rotation, targetRot, 5 * Time.deltaTime);

            // Arms
            Quaternion targetArmRot = Quaternion.LookRotation(lastCombatTarget.GetTargetCenter().position - armRotAxis.transform.position);
            targetArmRot.y = 0;
            targetArmRot.z = 0;
            armRotAxis.localRotation = Quaternion.Slerp(armRotAxis.localRotation, targetArmRot, 5 * Time.deltaTime);
        }
    }

    bool FindTarget()
    {
        if (lastCombatTarget != null)
        {
            weaponSystem.CheckTargetInView(lastCombatTarget);
        }
        else
        {
            lastCombatTarget = weaponSystem.FindTargetInView(60, weaponSystem.optimalRanges.z);
        }

        if (lastCombatTarget != null)
        {
            lastKnownPosition = Vector3.zero;
            alerted = true;
            Alert();
        }

        return (lastCombatTarget != null);
    }

    public override Vector3 GetFiringSolutionPoint()
    {
        return Vector3.zero;
    }

    bool MoveRandomPointInAngle(Vector3 location, float distanceAway, float angle)
    {
        if (location != Vector3.zero)
        {
            var dir = (location - transform.position).normalized;// + Random.Range(-30, 30);
            dir = Quaternion.Euler(0, Random.Range(-angle / 2, angle / 2), 0) * dir;

            var point = location - (dir * distanceAway);

            RaycastHit hit;
            Physics.Raycast(point + Vector3.up * 50, Vector3.down, out hit, 100f, LayerMask.GetMask("Terrain"));

            NavMeshHit myNavHit;
            if (NavMesh.SamplePosition(hit.point, out myNavHit, distanceAway / 2, -1))
            {
                navAgent.SetDestination(myNavHit.position);
                return true;
            }
        }

        return false;
    }

    void RandomPointOnNavmesh(Vector3 position, float distance)
    {
        var rand = new Vector3(Random.Range(-distance, distance), 0, Random.Range(-distance, distance));

        NavMeshHit myNavHit;
        if (NavMesh.SamplePosition(position + rand, out myNavHit, distance / 2, -1))
        {
            navAgent.SetDestination(myNavHit.position);
        }
    }

    protected override void SectionTookDamage(MechManager source, float damageTaken)
    {
        base.SectionTookDamage(source, damageTaken);

        if (!alerted) alerted = true;
    }

    public void Alert()
    {
        // Alert nearby
        foreach (MechManager ally in LevelManager._instance.GetFactionMechsInRange(transform.position, 50, mechManager.mechFaction))
        {
            var cont = ally.GetComponent<AIController>();
            cont.lastCombatTarget = lastCombatTarget;
        }
    }

    public override void BecomeAlerted(Vector3 lastKnownPos)
    {
        base.BecomeAlerted(lastKnownPos);

        if (!alerted) alerted = true;

        // Alert nearby
        foreach (MechManager ally in LevelManager._instance.GetFactionMechsInRange(transform.position, 50, mechManager.mechFaction))
        {
            ally.GetComponent<AIController>().lastKnownPosition = lastKnownPos;
            ally.GetComponent<AIController>().alerted = true;
        }
    }



}