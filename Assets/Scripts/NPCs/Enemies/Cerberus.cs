using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(EnemyBehaviour))]
public class Cerberus : Enemy
{
    [Header("Cerberus Requirements")]
    public int requiredBread = 3;
    public int requiredBones = 2;

    [Header("Put to Sleep Requirements")]
    public string requiredSongName = "Cerberus Melody"; 
    public float songRange = 5f;
    public float songDamagePerSecond = 10f;
    public int currentSleep;
    public Slider sleepBar;   

    [Header("Attack Fields")]
    private EnemyBehaviour behaviour;
    private Transform player;
    private bool playerInRange = false;
    private float attackSpeed = 2.0f;
    private float attackCooldown = 1.0f;
    private float lastAttackTime = -999f;
    public int biteDamage = 10;
    public float biteRange = 1.5f;
    private PlayerStats playerHealth;

    protected override void Start()
    {
        base.Start();
        behaviour = GetComponent<EnemyBehaviour>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        playerHealth = player.GetComponent<PlayerStats>();

        if (CombatUIManager.Instance != null)
        {
            CombatUIManager.Instance.InitCerberusSleep(maxHealth);
        }
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

    private void AttackPlayer()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * attackSpeed * Time.deltaTime;
        transform.LookAt(player);

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= biteRange && Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(biteDamage);
                Debug.Log($"{name} bit the player for {biteDamage} damage.");
            }
        }
    }


    public override void TakeDamage(int damage)
    {
        Debug.Log("Use your song or offerings...weapons won't harm the Cerberus.");
    }

    public void TakeSongDamage(float amount)
    {
        currentHealth -= Mathf.RoundToInt(amount);
        if (currentHealth < 0) currentHealth = 0;

        if (CombatUIManager.Instance != null)
            CombatUIManager.Instance.UpdateCerberusSleep(currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Cerberus has fallen asleep...");
            Die();
        }
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
