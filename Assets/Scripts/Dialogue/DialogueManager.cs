using UnityEngine;
using Ink.Runtime;
using TMPro;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public TextAsset inkJsonAsset;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public Transform choicesContainer;
    public GameObject choicePrefab;
    public bool DialogueActive { get; private set; }

    private Story story;
    private readonly List<GameObject> currentChoiceButtons = new();

    void Start()
    {
        DialogueActive = false;
        dialoguePanel.SetActive(false);
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
        ContinueStory();
    }

    public void ContinueStory()
    {
        if (story == null)
        {
            Debug.LogWarning("No active story to continue.");
            return;
        }

        if (story.currentChoices.Count > 0)
        {
            Debug.Log("Waiting for player to choose an option...");
            return;
        }

        if (story.canContinue)
        {
            ClearChoices(); 
            string nextLine = story.Continue().Trim();
            dialogueText.text = nextLine;

            if (story.currentChoices.Count > 0)
            {
                DisplayChoices();
            }
        }
        else if (story.currentChoices.Count > 0)
        {
            DisplayChoices();
        }
        else
        {
            EndDialogue();
        }
    }

    void DisplayChoices()
    {
        ClearChoices();

        foreach (var choice in story.currentChoices)
        {
            var choiceButton = Instantiate(choicePrefab, choicesContainer);
            var textComponent = choiceButton.GetComponentInChildren<TMP_Text>();
            textComponent.text = choice.text.Trim();

            currentChoiceButtons.Add(choiceButton);

            int choiceIndex = choice.index; 
            choiceButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                ChooseOption(choiceIndex);
            });
        }
    }

    void ChooseOption(int choiceIndex)
    {
        story.ChooseChoiceIndex(choiceIndex);
        ClearChoices();

        if (story.canContinue)
        {
            string nextLine = story.Continue().Trim();
            dialogueText.text = nextLine;

            if (story.currentChoices.Count > 0)
            {
                DisplayChoices();
            }
        }
        else if (story.currentChoices.Count > 0)
        {
            DisplayChoices();
        }
        else
        {
            EndDialogue();
        }
    }

    void ClearChoices()
    {
        foreach (var button in currentChoiceButtons)
        {
            if (button != null)
                Destroy(button);
        }
        currentChoiceButtons.Clear();
    }

    void EndDialogue()
    {
        DialogueActive = false;
        dialoguePanel.SetActive(false);
        ClearChoices();
        dialogueText.text = "";
    }
}
