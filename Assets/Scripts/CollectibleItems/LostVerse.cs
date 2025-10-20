using UnityEngine;

public class LostVerse : CollectableItem
{
    public string lostVerseName;
    public int convincingPower;

    protected override void Collect()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.AddLostVerse(this, 1);
            Debug.Log($"Collected Lost Verse: {itemName}");
        }
        Destroy(gameObject);
    }

}
