using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1 : MonoBehaviour
{
    public float Height = 2f;

    public float delayBeforeStart = 0.4f; 

    [Header("Enemy Settings")]
    public float lookRadius;
    public float hitRadius;
    public float PatrolRadius;
    public bool inLookRange = false;
    public bool DrawGizmos = true;

    [Header("Move Speed")]
    public float normalSpeed = 14f;
    public float slopeSpeed = 6f;

    [Header("Patrol")]
    public float timeForChangeSpot;
    [Tooltip("Shows how much time left to change spot(always leave at 0 value)")]
    public float timeToChangeSpot;

    [Header("Time Settings")]
    public float timeSpeed = 1f;

    [Header("What is Enemies")]
    public string EnemyTag;

    //Attack

    [Header("Attack")]
    public float Damage = 20f;
    public float timeBeforeDamage = 0.8f;

    [Header("Melee Attack")]
    public bool DrawMeleeAttackGizmo = false;
    public float AttackRadius = 1f;

    public float AttackDelay = 1f;
    private float CurrentAttackDelay;
    public float AttackStoppingTime = 1f;
    private float CurrentAttackStoppingTime;

    [Space]
    Transform target;

    [Header("Target Rotation")]
    public float FacingRotationSpeed = 12f;
    [Tooltip("if the variable set to true, Enemy will look directly on target(and rotate it self in target origin position)")]
    public bool lookAtTarget = false;

    NavMeshAgent agent;

    bool pathAvailable;
    NavMeshPath navMeshPath;

    [Header("Assing Fields")]
    public Transform HitPoint;
    public LayerMask PlayerEnemyLayer;
    public Animator anim;

    RaycastHit slopeHit;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        CurrentAttackStoppingTime = 0;

        CurrentAttackDelay = 0;

        target = PlayerManager.instance.player.transform;

        navMeshPath = new NavMeshPath();

        inLookRange = false;

        anim.SetTrigger("Spawn");
    }

    void Update()
    {
        delayBeforeStart -= Time.deltaTime * 1f;

        if (delayBeforeStart <= 0f)
        {
            CurrentAttackDelay -= Time.deltaTime * timeSpeed;
            CurrentAttackStoppingTime -= Time.deltaTime * timeSpeed;

            float distanceToPlayer = Vector3.Distance(target.position, transform.position);

            if (pathAvailable == true)
            {
                if (distanceToPlayer <= lookRadius) //if agent see player, he starting to chase him;
                {
                    agent.SetDestination(target.position);

                    agent.speed = normalSpeed;

                    anim.SetBool("isRunning", true);
                }

                if (distanceToPlayer <= hitRadius) //if agent to close with player, he damage him (play animation only);
                {
                    if (lookAtTarget == true)
                    {
                        transform.LookAt(target);
                    }

                    FacingTarget();

                    Hitting();

                    agent.speed = 0f;

                    anim.SetBool("isRunning", false);
                }
                else if (distanceToPlayer >= hitRadius && distanceToPlayer <= lookRadius)
                {
                    agent.speed = normalSpeed;

                    anim.SetBool("isRunning", true);

                    inLookRange = true;
                }
                else if (distanceToPlayer >= lookRadius)
                {
                    Patrol();

                    inLookRange = false;
                }
            }
            else if (pathAvailable == false)
            {
                Patrol();
            }

            if (CalculateNewPath() == true)
            {
                pathAvailable = true;
            }
            else
            {
                pathAvailable = false;
            }
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, Height / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    void Hitting()
    {
        if (CurrentAttackDelay <= 0)
        {
            Invoke("Hit", timeBeforeDamage);

            CurrentAttackDelay = AttackDelay;

            CurrentAttackStoppingTime = AttackStoppingTime;
        }
    }

    public void Hit()
    {
        Collider[] PlayerToDamage = Physics.OverlapSphere(HitPoint.position, AttackRadius, PlayerEnemyLayer);
        for (int i = 0; i < PlayerToDamage.Length; i++)
        {
            PlayerHealth1 target1 = PlayerToDamage[i].GetComponent<PlayerHealth1>();

            if (target1 != null)
            {
                target1.TakeDamage(Damage);
            }

            //Instantiate(EnemyTargetBloodParticle, HitPoint.position, HitPoint.rotation);
        }
    }

    bool CalculateNewPath()
    {
        agent.CalculatePath(target.position, navMeshPath);
        if (navMeshPath.status != NavMeshPathStatus.PathComplete)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void FacingTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * FacingRotationSpeed);
    }

    void Patrol()
    {
        timeToChangeSpot -= Time.deltaTime * timeSpeed;

        if (timeToChangeSpot <= 0)
        {
            SetNewRandomSpot();

            timeToChangeSpot = timeForChangeSpot;
        }
    }

    void SetNewRandomSpot()
    {
        Vector3 newDestination = RandomNavSphere(transform.position, PatrolRadius, -1);
        agent.SetDestination(newDestination);
    }

    Vector3 RandomNavSphere(Vector3 originPosition, float radius, int layerMask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += originPosition;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, radius, layerMask);

        return navHit.position;
    }

    void FixedUpdate()
    {
        MoveSlope();
    }

    void MoveSlope()
    {
        if (!OnSlope())
        {
            agent.speed = normalSpeed;
        }
        else if (OnSlope())
        {
            agent.speed = slopeSpeed;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (DrawGizmos == true)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, lookRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, hitRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, PatrolRadius);
        }

        if (DrawMeleeAttackGizmo == true)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(HitPoint.position, AttackRadius);
        }
    }
}
