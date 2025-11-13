using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueAutoStart : MonoBehaviour
{

     [SerializeField] private TextAsset inkJsonAsset;
    [SerializeField] private string nextSceneName;
    
    private DialogueManager dialogueManager;

    #region Unity Methods
    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialogueManager>();

        if (dialogueManager == null)
        {
            Debug.LogError("[DialogueAutoStart] DialogueManager not found in scene.");
            return;
        }

        dialogueManager.OnDialogueEnded += HandleDialogueEnd;

        // start dialogue automatically when scene loaded
        dialogueManager.StartDialogue(inkJsonAsset);
    }

         private void HandleDialogueEnd()
        {
            Debug.Log("[DialogueAutoStart] Dialogue finished — loading next scene...");
            SceneManager.LoadScene(nextSceneName);
        }

        private void OnDestroy()
        {
            if (dialogueManager != null)
                dialogueManager.OnDialogueEnded -= HandleDialogueEnd;
        }
    
    
    #endregion

}
