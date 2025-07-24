using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    private Transform player;
    public float chaseRange = 15f;
    public float moveSpeed = 3.5f;

    private NavMeshAgent agent;
    private Animator anim;
    private float distanceToPlayer;
    private bool isDead = false;

    public bool enableChasing = false; // Toggle for enabling/disabling chasing

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.speed = moveSpeed;

        SetRagdollState(false); // Disable ragdoll at start

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (isDead || player == null || !agent.enabled || !agent.isOnNavMesh || !enableChasing)
            return;

        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            agent.SetDestination(player.position);
            anim.SetInteger("State", 1); // Run
        }
        else
        {
            if (agent.hasPath)
                agent.ResetPath();

            anim.SetInteger("State", 0); // Idle
        }
    }


    public void Die(Collider hitPart, Vector3 hitForce, Vector3 hitPoint)
    {
        if (isDead) return;
        isDead = true;

        if (agent != null) agent.enabled = false;

        if (anim != null && anim.enabled)
        {
            anim.Update(0f); // Final pose
            anim.enabled = false;
        }

        StartCoroutine(EnableRagdollAfterDelay(0.05f, hitPart, hitForce, hitPoint));
    }

    private IEnumerator EnableRagdollAfterDelay(float delay, Collider hitPart, Vector3 hitForce, Vector3 hitPoint)
    {
        yield return new WaitForSeconds(delay);

        SetRagdollState(true);

        if (hitPart != null)
        {
            Rigidbody rb = hitPart.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForceAtPosition(hitForce, hitPoint, ForceMode.Impulse);
            }
        }

        // Disable main collider after death
        Collider mainCol = GetComponent<Collider>();
        if (mainCol != null)
            mainCol.enabled = false;
    }

    void SetRagdollState(bool state)
    {
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        Collider[] cols = GetComponentsInChildren<Collider>();

        foreach (Rigidbody rb in rbs)
            rb.isKinematic = !state;

        foreach (Collider col in cols)
            col.enabled = state;

        Collider mainCol = GetComponent<Collider>();
        if (mainCol != null)
            mainCol.enabled = true;
    }
}
