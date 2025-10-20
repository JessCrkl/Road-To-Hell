using UnityEngine;

public class EnemyBehaviour : Enemy
{
    public Transform[] patrolPoints;
    private int currentPointIndex = 0;
    public float moveSpeed = 3f;
    public float detectionRadius = 5f;
    private Transform player;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player").transform;
    }

    protected override void UpdateBehavior()
    {
        if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            AttackPlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform target = patrolPoints[currentPointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
    }

    void AttackPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        // TODO: Add attack logic here
    }
}
