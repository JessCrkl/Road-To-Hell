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

    void OnStartDialogue(InputValue value)
    {
        if (value.isPressed && currentTrigger != null && !dialogueManager.DialogueActive)
        {
            dialogueManager.StartDialogue(currentTrigger.inkJsonAsset);
        }
    }
    #endregion
}
