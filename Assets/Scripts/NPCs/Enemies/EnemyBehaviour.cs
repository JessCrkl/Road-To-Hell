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
    private Animator animator;
    private Vector3 lastPosition;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
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
        UpdateAnimation();
    }

    private void LateUpdate()
    {
        var terrain = Terrain.activeTerrain;
        if (terrain != null)
        {
            Vector3 pos = transform.position;
            float groundY = terrain.SampleHeight(pos) + terrain.transform.position.y;
            pos.y = groundY;
            transform.position = pos;
        }
    }

   private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPointIndex];
        Vector3 toTarget = targetPoint.position - transform.position;
        toTarget.y = 0f; 

        Vector3 direction = toTarget.normalized;

        transform.position += direction * patrolSpeed * Time.deltaTime;

        float distance = toTarget.magnitude;
         if (distance < 0.3f)
        {
            transform.position = new Vector3(
                targetPoint.position.x,
                transform.position.y,
                targetPoint.position.z
            );

            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;

            return; 
        }

        if (direction.sqrMagnitude > 0.0001f)
        {
            Vector3 lookPos = transform.position + direction;
            transform.LookAt(lookPos);
        }
    }

    private void AttackPlayer()
    {
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;

        Vector3 direction = toPlayer.normalized;

        transform.position += direction * attackSpeed * Time.deltaTime;
        if (direction.sqrMagnitude > 0.0001f)
        {
            Vector3 lookPos = transform.position + direction;
            transform.LookAt(lookPos);
        }
    }

    private void UpdateAnimation()
    {
        if (animator == null) return;

        Vector3 delta = transform.position - lastPosition;
        delta.y = 0f;

        float rawSpeed = delta.magnitude / Time.deltaTime;
        rawSpeed = Mathf.Clamp(rawSpeed, 0f, 5f);
        animator.SetFloat("Speed", rawSpeed, 0.1f, Time.deltaTime);

        lastPosition = transform.position;
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
