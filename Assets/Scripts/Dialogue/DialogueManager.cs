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
        story = new Story(inkJsonAsset.text);
    }
    void Update()
    {
        // if (DialogueActive && Input.GetKeyDown(KeyCode.Space))
        //     ContinueStory();
    }

    public void StartDialogue(TextAsset newInkJson)
    {
        story = new Story(newInkJson.text);
        DialogueActive = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    public void ContinueStory()
    {
        if (story.canContinue)
        {
            dialogueText.text = story.Continue();
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
