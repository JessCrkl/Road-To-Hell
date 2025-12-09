using UnityEngine;

public class CerberusAreaTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (CombatUIManager.Instance != null)
        {
            CombatUIManager.Instance.SetInCombatArea(true);
            CombatUIManager.Instance.ShowCombatUI(true);
        }

        if (PlayerStats.Instance != null && PlayerStats.Instance.HasLearnedSong("Cerberus Melody"))
        {
            CombatUIManager.Instance.ShowCerberusHint(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (CombatUIManager.Instance != null)
        {
            CombatUIManager.Instance.SetInCombatArea(false);
            CombatUIManager.Instance.ShowCombatUI(false);
            CombatUIManager.Instance.ShowCerberusHint(false);
        }
    }
}
