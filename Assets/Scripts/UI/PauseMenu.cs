using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject optionsPanelPrefab;

    private GameObject optionsPanelInstance;

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("Escape pressed");
            ToggleOptionsPanel();
        }
    }

    private void ToggleOptionsPanel()
    {
        if (optionsPanelInstance == null)
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();
            optionsPanelInstance = Instantiate(optionsPanelPrefab, canvas.transform, false);
        }

        bool isActive = !optionsPanelInstance.activeSelf;
        optionsPanelInstance.SetActive(isActive);


        Time.timeScale = isActive ? 0f : 1f;

        Cursor.visible = isActive;
        Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;

        var player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            player.enabled = !isActive;
        }

        if (CombatUIManager.Instance != null)
        {
            if (isActive)
            {
                
                CombatUIManager.Instance.ShowCombatUI(false);
            }
            else
            {
                if (CombatUIManager.Instance.InCombatArea)
                    CombatUIManager.Instance.ShowCombatUI(true);
            }
        }
            
    }
}

