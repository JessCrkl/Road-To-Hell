using UnityEngine;

public class EnemyBehaviour : Enemy
{
    [Header("Patrol Path Settings")]
    public Transform[] patrolPoints;
    private int currentPointIndex = 0;
    public float patrolSpeed = 2f;
    public float attackSpeed = 4f;
    public float detectionRadius = 5f;

    [Header("Combat Settings")]
    public float chaseRadius = 8f;      
    public float attackRange = 2f;      
    public float chaseSpeed = 3.5f;
    public float attackCooldown = 1.5f;

    [Header("Player Interaction")]
    private Transform player;
    private bool playerDetected = false;
    private Animator animator;
    private Vector3 lastPosition;
    private float nextAttackTime = 0f;
    public int attackDamage = 10;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

        protected override void UpdateBehavior()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            playerDetected = true;
            AttackPlayer();    
        }
        else if (distance <= chaseRadius)
        {
            playerDetected = true;
            ChasePlayer();     
        }
        else
        {
            playerDetected = false;
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

        private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        transform.position += direction * chaseSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void AttackPlayer()
    {
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;
        if (toPlayer.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(toPlayer);
        }

        if (Time.time >= nextAttackTime)
        {
            if (animator != null)
            {
                animator.SetTrigger("Attack");   
            }

            PlayerStats playerHealth = player.GetComponent<PlayerStats>();
        if (playerHealth != null)
            playerHealth.TakeDamage(attackDamage);
                nextAttackTime = Time.time + attackCooldown;
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
