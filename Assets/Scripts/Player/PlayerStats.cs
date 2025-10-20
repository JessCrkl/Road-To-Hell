using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }
    
    [Header("Player Statistics")]
    public int experience = 0;
    public int keys = 0;
    public int lostVerses = 0;
    //public int instruments = 0;
    private  List<Key> keysInInventory = new();
    private  List<LostVerse> lostVersesInInventory = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddXP(int amount)
    {
        Debug.Log("XP: " + experience);
        experience += amount;
    }
    public void AddKey(Key key)
    {
        Debug.Log("Keys: " + keys);
        keysInInventory.Add(key);
    }

    public void AddLostVerse(LostVerse lostVerse)
    {
        Debug.Log("Lost Verses: " + lostVerses);
        lostVersesInInventory.Add(lostVerse);
    }
}
