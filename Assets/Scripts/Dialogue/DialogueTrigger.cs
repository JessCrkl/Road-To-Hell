using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DialogueTrigger : MonoBehaviour
{
    [Tooltip("The Ink JSON story for this NPC or object.")]
    public TextAsset inkJsonAsset;

    [Tooltip("Trigger name for debugging or identification.")]
    public string triggerName;

    [HideInInspector] public bool playerInRange = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            DialogueEventHandler.Instance.SetCurrentTrigger(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            DialogueEventHandler.Instance.ClearTrigger(this);
        }
    }
}
