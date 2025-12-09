using UnityEngine;
using UnityEngine.UI;

public class CombatUIManager : MonoBehaviour
{
    public static CombatUIManager Instance { get; private set; }

    [Header("Panel")]
    public GameObject combatPanel;

    [Header("Sliders")]
    public Slider playerHealthSlider;
    public Slider cerberusSleepSlider;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (combatPanel != null)
            combatPanel.SetActive(false);
    }
    

    public void ShowCombatUI(bool show)
    {
        if (combatPanel != null)
            combatPanel.SetActive(show);
    }

    public void InitPlayerHealth(int maxHP)
    {
        if (playerHealthSlider == null) return;
        playerHealthSlider.maxValue = maxHP;
        playerHealthSlider.value = maxHP;
    }

    public void UpdatePlayerHealth(int currentHP)
    {
        if (playerHealthSlider == null) return;
        playerHealthSlider.value = currentHP;
    }

    public void InitCerberusSleep(int maxSleep)
    {
        if (cerberusSleepSlider == null) return;
        cerberusSleepSlider.maxValue = maxSleep;
        cerberusSleepSlider.value = maxSleep;
    }

    public void UpdateCerberusSleep(int currentSleep)
    {
        if (cerberusSleepSlider == null) return;
        cerberusSleepSlider.value = currentSleep;
    }
}
