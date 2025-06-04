using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public int lifes = 5;

    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private bool isDead = false;

    public Transform target; // Jugador
    public Transform entryTarget; // Punto de entrada al mapa

    private bool hasEnteredMap = false;

    // Anti-atasco
    private Vector3 lastPosition;
    private float stuckTimer = 0f;

    [Header("Power Up Drop")]
    [Tooltip("Lista de posibles bonificaciones que puede soltar este zombi")]
    public GameObject[] powerUpPrefabs;
    [Range(0f, 1f)] public float dropChance = 0.2f; // 20% por defecto

    private AudioSource audioSource;
    public AudioClip[] walkingSounds;
    public AudioClip[] deathSounds;

    private float walkSoundCooldown = 0f;
    private float minSoundInterval = 3f;
    private float maxSoundInterval = 6f;



    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null) return;

        navMeshAgent.updatePosition = true;
        navMeshAgent.updateRotation = true;

        if (!navMeshAgent.isOnNavMesh)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 2.0f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
                navMeshAgent.Warp(hit.position);
            }
            else
            {
                Debug.LogWarning("Zombi fuera del NavMesh y no se pudo reubicar.");
                enabled = false;
                return;
            }
        }

        if (entryTarget != null)
        {
            navMeshAgent.SetDestination(entryTarget.position);
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        lastPosition = transform.position;
    }

    void Update()
    {
        if (isDead || navMeshAgent == null || target == null) return;

        if (!hasEnteredMap && entryTarget != null)
        {
            navMeshAgent.SetDestination(entryTarget.position);

            float dist = Vector3.Distance(transform.position, entryTarget.position);
            if (dist < 1.5f)
            {
                hasEnteredMap = true;
            }

            return;
        }

        if (Vector3.Distance(navMeshAgent.destination, target.position) > 0.5f)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(target.position, out hit, 2.0f, NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(hit.position);
            }
            else if (!navMeshAgent.hasPath || navMeshAgent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                Debug.LogWarning("Destino fuera de la NavMesh y no hay ruta válida.");
            }
        }

        HandleStuckLogic();

        HandleWalkingSounds();
    }

    private void HandleStuckLogic()
    {
        float moved = Vector3.Distance(transform.position, lastPosition);
        if (moved < 0.05f)
        {
            stuckTimer += Time.deltaTime;

            if (stuckTimer > 1.5f)
            {
                Debug.Log("Zombi atascado. Forzando nuevo destino.");
                Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                navMeshAgent.SetDestination(target.position + randomOffset);
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }

        lastPosition = transform.position;
    }

    public void GrenadeImpact()
    {
        LoseLife(1);
        Debug.Log("Ay");
    }

    public void LoseLife(int lifesToLose)
    {
        if (isDead) return;

        if (GameManager.Instance.isInstaKillActive)
        {
            lifes = 0;
        }
        else
        {
            lifes -= lifesToLose;
        }
        Debug.Log("Vida actual del enemigo: " + lifes);

        if (lifes <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            navMeshAgent.enabled = false;
        }

        if (animator != null)
        {
            animator.SetTrigger("die");
        }

        if (audioSource != null && deathSounds.Length > 0)
        {
            AudioClip clip = deathSounds[Random.Range(0, deathSounds.Length)];
            audioSource.PlayOneShot(clip);
        }

        GetComponent<Collider>().enabled = false;

        var animController = GetComponent<ZombieAnimationController>();
        if (animController != null)
        {
            animController.Die();
        }

        GameManager.Instance.AddPoints(100);
        GameManager.Instance.OnZombieKilled();

        TryDropPowerUp();

        Destroy(gameObject, 3f);
    }

    private void TryDropPowerUp()
    {
        if (powerUpPrefabs.Length == 0) return;
        if (Random.value > dropChance) return;

        int index = Random.Range(0, powerUpPrefabs.Length);
        GameObject selected = powerUpPrefabs[index];

        Vector3 spawnPos = transform.position + Vector3.up * 2.5f;
        Instantiate(selected, spawnPos, Quaternion.identity);

    }

    public void ForceDie()
    {
        if (isDead) return;

        lifes = 0;
        Die();
    }

    private void HandleWalkingSounds()
    {
        if (!navMeshAgent.hasPath || navMeshAgent.velocity.magnitude < 0.2f) return;

        walkSoundCooldown -= Time.deltaTime;
        if (walkSoundCooldown <= 0f && walkingSounds.Length > 0)
        {
            AudioClip clip = walkingSounds[Random.Range(0, walkingSounds.Length)];
            audioSource.PlayOneShot(clip);

            walkSoundCooldown = Random.Range(minSoundInterval, maxSoundInterval);
        }
    }


}
