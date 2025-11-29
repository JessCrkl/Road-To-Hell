using UnityEngine;
using System;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager Instance { get; private set; }
    public event Action<string> OnItemCollected;

    #region Unity Methods
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    public void ItemCollected(string itemName)
    {
        OnItemCollected?.Invoke(itemName);
    }

}
