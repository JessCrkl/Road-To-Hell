using UnityEngine;

public class Key : CollectableItem
{
    public string unlocksDoorName;

    protected override void Collect()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.AddKey(this, 1);
            Debug.Log($"Collected Key: {itemName}");
        }
        Destroy(gameObject);
    }
}
