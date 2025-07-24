using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolling : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float waitTime = 2f;
    public float stoppingDistance = 0.5f;
    public float slowDownDistance = 2f; // When to start slowing down

    private int currentPointIndex = 0;
    private NavMeshAgent agent;
    private Animator animator;
    private float waitTimer = 0f;
    private float originalSpeed;
    private Target targetScript;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        targetScript = GetComponent<Target>();

        if (agent != null)
            originalSpeed = agent.speed;
            agent.stoppingDistance = 0f;

        if (patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[0].position);
    }

    void Update()
    {
        if (targetScript != null && targetScript.health <= 0f) return; // Don't move if dead
        if (patrolPoints.Length == 0) return;

        float remaining = agent.remainingDistance;

        // Slow down when approaching the point
        if (!agent.pathPending && agent.remainingDistance <= 0.1f)
        {
            agent.speed = Mathf.Lerp(0f, originalSpeed, remaining / slowDownDistance);
        }
        else
        {
            agent.speed = originalSpeed;
        }

        // Handle waiting at the point
        if (!agent.pathPending && remaining <= stoppingDistance)
        {
            agent.isStopped = true;
            waitTimer += Time.deltaTime;

            // Play idle animation
            if (animator != null)
                animator.SetInteger("State", 0); // Idle

            if (waitTimer >= waitTime)
            {
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
                agent.SetDestination(patrolPoints[currentPointIndex].position);
                agent.isStopped = false;
                waitTimer = 0f;
            }
        }
        else
        {
            // Walking animation
            if (animator != null)
                animator.SetInteger("State", 1); // Walking
        }
    }
}
