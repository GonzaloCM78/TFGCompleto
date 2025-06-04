using UnityEngine;
using UnityEngine.AI;

public class ZombieAnimationController : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private Transform targetPlayer;
    public float detectionRange = 5f;
    private bool isDead = false;
    public LayerMask obstacleMask;


    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (isDead) return;

        FindClosestPlayer();

        if (targetPlayer != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);

            if (distanceToPlayer <= detectionRange)
            {
                if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
                {
                    agent.SetDestination(targetPlayer.position);
                }

                if (distanceToPlayer <= 7f)
                {
                    Vector3 origin = transform.position + Vector3.up * 1f; // elevar origen para evitar el suelo
                    Vector3 direction = (targetPlayer.position - origin).normalized;

                    if (Physics.Raycast(origin, direction, out RaycastHit hit, 7f, ~obstacleMask))
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            animator.SetBool("isWalking", false);
                            animator.SetBool("isAttacking", true);
                        }
                        else
                        {
                            animator.SetBool("isWalking", true);
                            animator.SetBool("isAttacking", false);
                        }
                    }
                    else
                    {
                        animator.SetBool("isWalking", true);
                        animator.SetBool("isAttacking", false);
                    }

                }
                else
                {
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isAttacking", false);
                }
            }
            else
            {
                if (agent != null && agent.isOnNavMesh)
                    agent.ResetPath();

                animator.SetBool("isWalking", false);
                animator.SetBool("isAttacking", false);
            }
        }
    }

    void FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player.transform;
            }
        }

        targetPlayer = closestPlayer;
    }

    public void Die()
    {
        isDead = true;

        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.enabled = false;
        }

        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isDead", true);
        }

        this.enabled = false;

        Destroy(gameObject, 3f);
    }
}
