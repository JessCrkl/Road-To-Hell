using UnityEngine;

public class CerberusAreaTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (CombatUIManager.Instance != null)
            CombatUIManager.Instance.ShowCombatUI(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (CombatUIManager.Instance != null)
            CombatUIManager.Instance.ShowCombatUI(false);
    }
}
