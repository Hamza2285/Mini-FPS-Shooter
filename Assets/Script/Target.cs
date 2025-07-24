using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    public float health = 100f;
    private bool isDead = false;

    public Rigidbody[] ragdollBodies;
    public Collider[] ragdollColliders;
    public Animator animator;

    private NavMeshAgent agent;

    void Start()
    {
        SetRagdollActive(false);
        agent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(float amount, Collider hitCollider, Vector3 force, Vector3 hitPoint)
    {
        if (isDead) return;

        health -= amount;

        if (health <= 0f)
        {
            Die(hitCollider, force, hitPoint);
        }
    }

    private void Die(Collider hitCollider, Vector3 force, Vector3 hitPoint)
    {
        isDead = true;

        if (animator != null)
            animator.enabled = false;

        if (agent != null)
            agent.enabled = false;

        SetRagdollActive(true);

        Rigidbody hitRb = hitCollider.attachedRigidbody;
        if (hitRb != null)
        {
            hitRb.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);
        }
    }

    private void SetRagdollActive(bool active)
    {
        foreach (Rigidbody rb in ragdollBodies)
            rb.isKinematic = !active;

        foreach (Collider col in ragdollColliders)
            col.enabled = active;

        Collider rootCol = GetComponent<Collider>();
        if (rootCol != null) rootCol.enabled = !active;

        Rigidbody rootRb = GetComponent<Rigidbody>();
        if (rootRb != null)
        {
            rootRb.linearVelocity = Vector3.zero;
            rootRb.angularVelocity = Vector3.zero;
            rootRb.isKinematic = active;
        }
    }
}
