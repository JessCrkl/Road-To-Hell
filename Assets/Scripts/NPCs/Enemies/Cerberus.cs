using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
        playerHealth = PlayerStats.Instance;

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

    public void AttackPlayer()
    {
        if (player == null || isDead) return;

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;
        float distance = toPlayer.magnitude;

        if (distance > biteRange)
        {
            Vector3 dir = toPlayer.normalized;
            transform.position += dir * attackSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(dir);
            return;
        }

        if (toPlayer.sqrMagnitude > 0.001f)
        transform.rotation = Quaternion.LookRotation(toPlayer);

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(biteDamage);
                Debug.Log($"{name} bit the player for {biteDamage} damage.");
            }
            else
            {
                Debug.LogWarning("Cerberus: playerHealth is null, cannot damage player.");
            }
        }
    }

    public void TakeSongDamage(float amount)
    {
        currentHealth -= Mathf.RoundToInt(amount);
        Debug.Log($"Cerberus song hit for {amount}, health now {currentHealth}/{maxHealth}");

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

        if (!PlayerStats.Instance.HasLearnedSong("Cerberus Melody"))
        {
            Debug.Log("You haven’t learned the Cerberus Melody yet.");
            return;
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, 5f);
        foreach (var hit in hits)
        {
            Cerberus cerb = hit.GetComponent<Cerberus>();
            if (cerb != null)
            {
                cerb.TakeSongDamage(cerb.songDamagePerSecond);
                Debug.Log("You play the Cerberus Melody...");
                return;
            }
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

    protected override void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Cerberus is dying...");

        EnemyBehaviour behaviour = GetComponent<EnemyBehaviour>();
        if (behaviour != null)
            behaviour.enabled = false;

        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.ResetTrigger("Attack");   
            animator.SetFloat("Speed", 0f);    
            animator.SetTrigger("Death");     
        }

        StartCoroutine(DelayedDeath());
    }

    private IEnumerator DelayedDeath()
{
    Animator animator = GetComponent<Animator>();
    const string deathStateName1 = "Cerberus_death_01";
    const string deathStateName2 = "Cerberus_death_02";

    float timeout = 5f; 

    if (animator != null)
    {
        
        float t = 0f;
        while (t < timeout)
        {
            var info = animator.GetCurrentAnimatorStateInfo(0);
            Debug.Log($"Current Animation State Info: {info}");
            if (info.IsName(deathStateName1) || info.IsName(deathStateName2))
            {
                
                while (info.normalizedTime < 1f)
                {
                    yield return null;
                    info = animator.GetCurrentAnimatorStateInfo(0);
                }
                break;
            }

            t += Time.deltaTime;
            yield return null;
        }
    }
    else
    {
        // fallback if no animator
        yield return new WaitForSeconds(2f);
    }

    if (CombatUIManager.Instance != null)
    {
        Debug.Log("Hiding combat UI from Cerberus death");
        CombatUIManager.Instance.ShowCombatUI(false);
        CombatUIManager.Instance.ShowCerberusHint(false);
        CombatUIManager.Instance.SetInCombatArea(false);
    }

    if (PlayerStats.Instance != null)
    {
        PlayerStats.Instance.AddXP(rewardXP);
        // PlayerStats.Instance.AddLostVerse(rewardLostVerse, lostVersesRewardCount);
        PlayerStats.Instance.AddKey(rewardKey, keyRewardCount);
    }
    if (rewardKey != null)
    {
        GameEventsManager.Instance.ItemCollected(rewardKey.name);
    }

    yield return new WaitForSeconds(2f);
    GameEventsManager.Instance.ItemCollected($"+{rewardXP} XP");

    Destroy(gameObject);
    }


}
