using UnityEngine;

public enum EnemyType
{
    Cerberus,
    HellHound,
    LostSoul,
    Hades,

    // i'll add more here if i scale the game
}

public abstract class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public int maxHealth;
    public int currentHealth;
    public int rewardXP;
    public LostVerse rewardLostVerse;
    public Key rewardKey;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        PlayerStats.Instance.AddXP(rewardXP);
        PlayerStats.Instance.AddLostVerse(rewardLostVerse);
        PlayerStats.Instance.AddKey(rewardKey);
        Destroy(gameObject);
    }

    protected abstract void UpdateBehavior();
    
    void Update()
    {
        UpdateBehavior();
    }
}
