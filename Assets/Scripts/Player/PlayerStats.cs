using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }
    
    [Header("Player Statistics")]
    public int experience = 0;
    public int keyCount = 0;
    public int lostVersesCount = 0;
    public int breadCount = 0;
    public int boneCount = 0;
    //public int instrumentCount = 0;

    // private  List<Instruments> instrumentsInInventory = new();
    private readonly List<Key> keysInInventory = new();
    private readonly List<LostVerse> lostVersesInInventory = new();
    public HashSet<string> UnlockedSongs = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    #region Collect Add Methods
    public void AddXP(int amount)
    {
        experience += amount;
        Debug.Log("XP: " + experience);
    }
    public void AddKey(Key key, int amount)
    {
        keyCount += amount;
        // Debug.Log("Keys: " + keyCount);
        keysInInventory.Add(key);
        Debug.Log($"Added key: {key.name}");
    }

    public void AddLostVerse(LostVerse lostVerse, int amount)
    {
        lostVersesCount += amount;
        // Debug.Log("Lost Verses: " + lostVersesCount);
        lostVersesInInventory.Add(lostVerse);
        Debug.Log($"Added Lost Verse: {lostVerse.itemName}");

        if(lostVersesCount == 4)
        {
            // show songbook prompt
        }
    }
    public void AddBread(int amount)
    {
        breadCount += amount;
        Debug.Log("Bread: " + breadCount);
    }

    public void AddBone(int amount)
    {
        boneCount += amount;
        Debug.Log("Bone: " + boneCount);
    }
    #endregion

    #region Helper Methods
    public bool HasEnoughResources(int requiredBread, int requiredBones)
    {
        return breadCount >= requiredBread && boneCount >= requiredBones;
    }

    public void SpendResources(int bread, int bones)
    {
        breadCount -= bread;
        boneCount -= bones;
        Debug.Log($"Gave {bread} bread and {bones} bones");
    }

    public bool HasLearnedSong(string songName)
    {
        return UnlockedSongs.Contains(songName);
    }
    #endregion
}
