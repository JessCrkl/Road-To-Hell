using UnityEngine;
using Ink.Runtime;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextAsset inkJsonAsset;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public bool DialogueActive { get; private set; }

    private Story story;
    
    void Start()
    {
        DialogueActive = false;
        dialoguePanel.SetActive(false);
        if (inkJsonAsset != null)
        {
            story = new Story(inkJsonAsset.text);
        }
        
    }
    void Update()
    {
        
    }

    public void StartDialogue(TextAsset newInkJson)
    {
        if (DialogueActive)
        {
            Debug.LogWarning("Dialogue already active, ignoring StartDialogue call.");
            return;
        }
        story = new Story(newInkJson.text);
        DialogueActive = true;
        dialoguePanel.SetActive(true);

        if (story.canContinue)
        {
            dialogueText.text = story.Continue().Trim();
        }
    }

    public void ContinueStory()
    {
        if (story == null)
        {
            Debug.LogWarning("No active story to continue.");
            return;
        }
        if (story.canContinue)
        {
            dialogueText.text = story.Continue().Trim();
        } else {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        DialogueActive = false;
        dialoguePanel.SetActive(false);
        
    }
}
