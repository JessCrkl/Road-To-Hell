using UnityEngine;

public class Bone : CollectableItem
{
    protected override void Collect()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.AddBone(amount);
            Debug.Log($"Collected {amount} bone!");
        }
        Destroy(gameObject);
    }

}
