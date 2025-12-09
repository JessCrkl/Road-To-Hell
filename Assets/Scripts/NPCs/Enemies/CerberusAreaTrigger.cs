using UnityEngine;

public class CerberusAreaTrigger : MonoBehaviour
{
    public GameObject combatUIPanel;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && combatUIPanel != null)
        {
            combatUIPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && combatUIPanel != null)
        {
            combatUIPanel.SetActive(false);
        }
    }
}
