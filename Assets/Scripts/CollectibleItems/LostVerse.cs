using System.Collections.Generic;
using UnityEngine;

public class LostVerse : CollectableItem
{
    public string lostVerseName;
    public int convincingPower;
    public int notes;

    [Header("Song Fragment Data")]
    public SongFragment songFragment;

    protected override void Collect()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.AddLostVerse(this, 1);
            if (songFragment != null)
            {
                SongFragmentManager.Instance.CollectFragment(songFragment);
            }
            Debug.Log($"Collected Lost Verse: {itemName}");
        }
        Destroy(gameObject);
    }

}
