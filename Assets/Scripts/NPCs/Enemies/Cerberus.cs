using UnityEngine;

[RequireComponent(typeof(EnemyBehaviour))]
public class Cerberus : Enemy
{
    [Header("Cerberus Requirements")]
    public int requiredBread = 3;
    public int requiredBones = 2;

    private EnemyBehaviour behaviour;
    private Transform player;
    private bool playerInRange = false;

    protected override void Start()
    {
        base.Start();
        behaviour = GetComponent<EnemyBehaviour>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected override void UpdateBehavior()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= behaviour.detectionRadius)
        {
            if (!playerInRange)
            {
                playerInRange = true;
                Debug.Log("Cerberus growls... you feel its gaze.");
            }
        }
        else
        {
            playerInRange = false;
        }
    }

    public override void TakeDamage(int damage)
    {
        Debug.Log("You don't have any instruments yet!");
    }

    public void TryToAppease()
    {
        var playerStats = PlayerStats.Instance;

        if (playerStats == null)
        {
            Debug.LogWarning("No PlayerStats instance found.");
            return;
        }

        if (playerStats.HasEnoughResources(requiredBread, requiredBones))
        {
            Debug.Log("Cerberus accepts your offerings...");
            playerStats.SpendResources(requiredBread, requiredBones);
            Die();
        }
        else
        {
            Debug.Log("You don’t have enough bread or bones to appease Cerberus!");
        }
    }
}
