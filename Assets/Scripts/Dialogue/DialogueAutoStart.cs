using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

        StartCoroutine(StartSceneFade());
    }

    private IEnumerator StartSceneFade()
    {
        if (FadeController.Instance != null)
            yield return FadeController.Instance.FadeIn();

        dialogueManager.StartDialogue(inkJsonAsset);
    }

    private void HandleDialogueEnd()
    {
        Debug.Log("[DialogueAutoStart] Dialogue finished — loading next scene...");
        StartCoroutine(LoadNextSceneFade());
    }
        
    private IEnumerator LoadNextSceneFade()
    {
        if (FadeController.Instance != null)
        {
            yield return FadeController.Instance.FadeOut();
            yield return new WaitForSeconds(0.1f);
        }

        SceneManager.LoadScene(nextSceneName);
    }

        private void OnDestroy()
        {
            if (dialogueManager != null)
                dialogueManager.OnDialogueEnded -= HandleDialogueEnd;
        }
    
    
    #endregion

}
