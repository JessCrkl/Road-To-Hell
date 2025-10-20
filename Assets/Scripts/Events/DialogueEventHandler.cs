using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueEventHandler : MonoBehaviour
{
    public static DialogueEventHandler Instance { get; private set; }
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] GameObject interactPrompt;
    private DialogueTrigger currentTrigger;
    
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
    
    void Update()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(currentTrigger != null && !dialogueManager.DialogueActive);
    }

    void OnValidate()
    {
        if (dialogueManager == null)
            dialogueManager = FindAnyObjectByType<DialogueManager>();
    }
    #endregion

    #region Event Handler Methods
    public void SetCurrentTrigger(DialogueTrigger trigger)
    {
        currentTrigger = trigger;
    }

    public void ClearTrigger(DialogueTrigger trigger)
    {
        if (currentTrigger == trigger)
            currentTrigger = null;
    }

    public void TryStartDialogue()
    {
        if(currentTrigger != null && !dialogueManager.DialogueActive)
        {
            Debug.Log($"[DialogueEventHandler] Starting dialogue from trigger: {currentTrigger.name}");
            dialogueManager.StartDialogue(currentTrigger.inkJsonAsset);
        } else {
            Debug.Log("[DialogueEventHandler] Cannot start dialogue — no trigger or already active.");
        }
    }
    #endregion
} //eoc
