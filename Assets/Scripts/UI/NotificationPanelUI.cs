using UnityEngine;
using TMPro;   

public class NotificationPanelUI : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text message;
    public float displayTime = 2f;

    private float timer;

    private void OnEnable()
    {
        if (GameEventsManager.Instance != null) {
        GameEventsManager.Instance.OnItemCollected += ShowNotification;
        } else {
            Debug.LogError("GameEventsManager.Instance is NULL. Make sure it exists in the scene before this UI enables.");
        }
    }

    private void OnDisable()
    {
        if (GameEventsManager.Instance != null)
        {
            GameEventsManager.Instance.OnItemCollected -= ShowNotification;
        }
    }

    private void ShowNotification(string itemName)
    {
        message.text = $"Collected: {itemName}";
        panel.SetActive(true);
        timer = displayTime;
    }

    private void Update()
    {
        if (panel.activeSelf)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                panel.SetActive(false);
            }
        }
    }
}

