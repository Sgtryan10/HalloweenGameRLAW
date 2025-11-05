using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MichaelMyersAI : MonoBehaviour
{
    public enum State { Patrol, Investigate, Stalk, Chase, Attack, ReturnToPatrol }

    [Header("References")]
    public Transform[] patrolPoints;
    public Transform playerTransform;
    public string playerObjectName = "FirstPersonController";
    private NavMeshAgent agent;

    [Header("Perception")]
    public float sightRange = 25f;
    [Range(0, 180)] public float sightAngle = 65f;
    public float loseSightTime = 3f;
    public float hearingRange = 12f;
    public float immediateDetectionDistance = 6f;
    public LayerMask obstructionMask;

    [Header("Movement")]
    public float stalkingSpeed = 2f;
    public float chaseSpeed = 6f;
    public float patrolSpeed = 3.5f;

    [Header("Attack")]
    public float attackRange = 1.8f;
    public float attackCooldown = 2f;
    public bool instantKill = true;

    [Header("Timers")]
    public float timeToWaitAtLastKnown = 2f;
    public float returnToPatrolDelay = 4f;

    private State currentState;
    private int patrolIndex = 0;
    private Vector3 lastKnownPlayerPos = Vector3.positiveInfinity;
    private float timeSinceLastSeen = Mathf.Infinity;
    private float attackTimer = 0f;

    private bool playerInLOS = false;
    private bool playerInHearing = false;
    private Transform tr;

    void Awake()
    {
        tr = transform;
        agent = GetComponent<NavMeshAgent>();

        if (playerTransform == null)
        {
            GameObject p = GameObject.Find(playerObjectName);
            if (p != null) playerTransform = p.transform;
        }

        agent.autoBraking = false;
        agent.updateRotation = true;
    }

    void Start()
    {
        EnterState(State.Patrol);
    }

    void Update()
    {
        attackTimer -= Time.deltaTime;
        timeSinceLastSeen += Time.deltaTime;
        UpdatePerception();

        switch (currentState)
        {
            case State.Patrol: StatePatrol(); break;
            case State.Investigate: StateInvestigate(); break;
            case State.Stalk: StateStalk(); break;
            case State.Chase: StateChase(); break;
            case State.Attack: StateAttack(); break;
            case State.ReturnToPatrol: StateReturnToPatrol(); break;
        }
    }

    void EnterState(State s)
    {
        currentState = s;
        switch (s)
        {
            case State.Patrol:
                agent.speed = patrolSpeed;
                if (patrolPoints.Length > 0)
                    agent.SetDestination(patrolPoints[patrolIndex].position);
                break;

            case State.Investigate:
                agent.speed = stalkingSpeed;
                if (lastKnownPlayerPos != Vector3.positiveInfinity)
                    agent.SetDestination(lastKnownPlayerPos);
                break;

            case State.Stalk:
                agent.speed = stalkingSpeed;
                break;

            case State.Chase:
                agent.speed = chaseSpeed;
                break;

            case State.Attack:
                agent.isStopped = true;
                break;

            case State.ReturnToPatrol:
                agent.speed = patrolSpeed;
                if (patrolPoints.Length > 0)
                    agent.SetDestination(patrolPoints[patrolIndex].position);
                break;
        }
    }

    // blelelellelell LOGIC PART
    void StatePatrol()
    {
        if (patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[patrolIndex].position);
        }

        if (PlayerVisible()) EnterState(State.Chase);
        else if (PlayerPartiallyVisible()) { lastKnownPlayerPos = playerTransform.position; EnterState(State.Stalk); }
        else if (playerInHearing) { lastKnownPlayerPos = playerTransform.position; EnterState(State.Investigate); }
    }

    void StateInvestigate()
    {
        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
            StartCoroutine(InvestigateCoroutine());

        if (PlayerVisible()) EnterState(State.Chase);
    }

    IEnumerator InvestigateCoroutine()
    {
        float t = 0f;
        while (t < timeToWaitAtLastKnown)
        {
            if (PlayerVisible()) { EnterState(State.Chase); yield break; }
            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(returnToPatrolDelay);
        EnterState(State.ReturnToPatrol);
    }

    void StateStalk()
    {
        if (playerTransform == null) return;
        agent.SetDestination(playerTransform.position);

        if (PlayerVisible()) EnterState(State.Chase);
        else if (Vector3.Distance(tr.position, playerTransform.position) <= attackRange)
            EnterState(State.Attack);
        else if (!playerInHearing && !PlayerPartiallyVisible() && timeSinceLastSeen > loseSightTime)
            EnterState(State.ReturnToPatrol);
    }

    void StateChase()
    {
        if (playerTransform == null) return;
        agent.SetDestination(playerTransform.position);

        float dist = Vector3.Distance(tr.position, playerTransform.position);
        if (dist <= attackRange) EnterState(State.Attack);

        if (!playerInLOS && timeSinceLastSeen > loseSightTime)
            EnterState(State.Investigate);
    }

    void StateAttack()
    {
        if (playerTransform == null) return;

        float dist = Vector3.Distance(tr.position, playerTransform.position);
        if (dist > attackRange + 0.5f)
        {
            EnterState(State.Chase);
            return;
        }

        if (attackTimer <= 0f)
        {
            DoAttack();
            attackTimer = attackCooldown;
        }
    }

    void StateReturnToPatrol()
    {
        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
            EnterState(State.Patrol);

        if (PlayerVisible()) EnterState(State.Chase);
        else if (PlayerPartiallyVisible())
        {
            lastKnownPlayerPos = playerTransform.position;
            EnterState(State.Stalk);
        }
    }

    // oh my god this is taking forever perception
    void UpdatePerception()
    {
        if (!playerTransform) return;

        // hear the player ~!!!!!
        float hearingDist = Vector3.Distance(tr.position, playerTransform.position);
        playerInHearing = hearingDist <= hearingRange;

        // eyesight of the player o.o
        Vector3 toPlayer = playerTransform.position - tr.position;
        float dist = toPlayer.magnitude;
        bool inRange = dist <= sightRange;
        bool inAngle = Vector3.Angle(tr.forward, toPlayer) <= sightAngle;
        bool hasLOS = false;

        if (inRange && inAngle)
        {
            Vector3 eye = tr.position + Vector3.up * 1.6f;
            Vector3 target = playerTransform.position + Vector3.up * 1.2f;
            if (!Physics.Raycast(eye, (target - eye).normalized, dist, obstructionMask))
                hasLOS = true;
        }

        if (dist <= immediateDetectionDistance)
        {
            Vector3 eye = tr.position + Vector3.up * 1.6f;
            Vector3 target = playerTransform.position + Vector3.up * 1.2f;
            if (!Physics.Raycast(eye, (target - eye).normalized, dist, obstructionMask))
                hasLOS = true;
        }

        playerInLOS = hasLOS;
        if (playerInLOS)
        {
            lastKnownPlayerPos = playerTransform.position;
            timeSinceLastSeen = 0f;
        }
    }

    bool PlayerVisible() => playerInLOS;
    bool PlayerPartiallyVisible() => playerInHearing;

    // Kill the player plss end my misery
    void DoAttack()
    {
        if (instantKill && playerTransform)
        {
            var controller = playerTransform.GetComponent<MonoBehaviour>();
            var fps = playerTransform.GetComponent("FirstPersonController") as MonoBehaviour;
            if (fps != null) fps.enabled = false;
            Debug.Log("Michael Myers killed the player!");
        }
    }

    // fixing stuff because this took forever
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hearingRange);

        Vector3 left = Quaternion.Euler(0, -sightAngle, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, sightAngle, 0) * transform.forward;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + left * sightRange);
        Gizmos.DrawLine(transform.position, transform.position + right * sightRange);
    }
}
