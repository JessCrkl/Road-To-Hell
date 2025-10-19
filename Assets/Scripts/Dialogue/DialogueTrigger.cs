using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DialogueTrigger : MonoBehaviour
{
    [Tooltip("The Ink JSON story for this NPC or object.")]
    public TextAsset inkJsonAsset;

    [Tooltip("Trigger name for debugging or identification.")]
    public string triggerName;

    [HideInInspector] public bool playerInRange = false;

    private void Awake()
    {
        Debug.Log($"[DialogueTrigger] Awake on {gameObject.name}");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log($"[DialogueTrigger] Player entered {gameObject.name}");
            playerInRange = true;
            DialogueEventHandler.Instance.SetCurrentTrigger(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log($"[DialogueTrigger] Player exited {gameObject.name}");
            playerInRange = false;
            DialogueEventHandler.Instance.ClearTrigger(this);
        }
    }
}
