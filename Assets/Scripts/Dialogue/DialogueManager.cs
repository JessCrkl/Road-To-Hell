using UnityEngine;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{

    [Header("Ink Story")]
    [SerializeField] private TextAsset inkJson;

    public GameObject dialoguePanel;
    private bool dialoguePlaying = false;
    private Story story;
    

    private void OnEnable()
    {
        GameEventsManager.instance.dialogueEvents.onEnterDialogue += EnterDialogue;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.dialogueEvents.onEnterDialogue -= EnterDialogue;
    }

    public void EnterDialogue(string knotName)
    {
        if (dialoguePlaying)
        {
            return;
        }

        dialoguePlaying = true;

        Debug.Log("Entering dialogue for knot name: " + knotName);
    }
}
