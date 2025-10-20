using UnityEngine;

public class Bread : CollectableItem
{
    protected override void Collect()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.AddBread(amount);
            Debug.Log($"Collected {amount} bread!");
        }
        Destroy(gameObject);
    }
}
