using UnityEngine;

public class EnemyBehaviour : Enemy
{
    [Header("Patrol Path Settings")]
    public Transform[] patrolPoints;
    private int currentPointIndex = 0;
    public float patrolSpeed = 2f;
    public float attackSpeed = 4f;
    public float detectionRadius = 5f;

    [Header("Player Interaction")]
    private Transform player;
    private bool playerDetected = false;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player").transform;
    }

    protected override void UpdateBehavior()
    {
        if (playerDetected && player != null)
        {
            AttackPlayer();
        }
        else
        {
            Patrol();
        }
    }

   private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPointIndex];
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        transform.position += direction * patrolSpeed * Time.deltaTime;
        transform.LookAt(targetPoint);

        float distance = Vector3.Distance(transform.position, targetPoint.position);
        if (distance < 0.5f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    private void AttackPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * attackSpeed * Time.deltaTime;
        transform.LookAt(player);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
            Debug.Log($"{name} detected player!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
            Debug.Log($"{name} lost sight of player.");
        }
    }
}
