using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Ink.Runtime;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public event Action OnDialogueEnded;

    [Header("Ink References")]
    public TextAsset inkJsonAsset;

    [Header("UI Stuff")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public Transform choicesContainer;
    public GameObject choicePrefab;
    public GameObject clickToContinuePrompt;
    public float continuePromptDelay = 7.0f;
    
    public bool DialogueActive { get; private set; }

    private Story story;
    private readonly List<GameObject> currentChoiceButtons = new();
    private bool showingContinuePrompt;
    private Coroutine promptCoroutine;

    #region Unity Methods
    void Start()
    {
        DialogueActive = false;
        dialoguePanel.SetActive(false);
        if (clickToContinuePrompt != null)
        {
            clickToContinuePrompt.SetActive(false);
        }
    }

    void Update()
    {
        if (!DialogueActive || story == null)
            return;

        if (story.currentChoices.Count == 0)
        {
            bool clickDetected = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
            bool spaceDetected = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
            bool enterDetected = Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame;

            if (clickDetected || spaceDetected || enterDetected)
            {
                ContinueStory();
            }
        }
    }
    #endregion

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
            //Debug.LogWarning("No active story to continue.");
            return;
        }

        if (story.currentChoices.Count > 0)
        {
            //Debug.Log("Waiting for player to choose an option...");
            return;
        }

        if (clickToContinuePrompt != null)
            clickToContinuePrompt.SetActive(false);

        if (promptCoroutine != null)
            StopCoroutine(promptCoroutine);

        if (story.canContinue)
        {
            Debug.LogWarning("Continuing story...");
            ClearChoices();
            string nextLine = story.Continue().Trim();
            dialogueText.text = nextLine;

            if (story.currentChoices.Count > 0)
            {
                DisplayChoices();
            }
            else
            {
                promptCoroutine = StartCoroutine(ShowContinuePromptAfterDelay());
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
    
    IEnumerator ShowContinuePromptAfterDelay()
    {
        yield return new WaitForSeconds(continuePromptDelay);

        if (clickToContinuePrompt != null && DialogueActive && story.currentChoices.Count == 0)
        {
            clickToContinuePrompt.SetActive(true);
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
        ContinueStory();

        // debug
        // if (story.canContinue)
        // {
        //     string nextLine = story.Continue().Trim();
        //     dialogueText.text = nextLine;

        //     if (story.currentChoices.Count > 0)
        //     {
        //         DisplayChoices();
        //     }
        // }
        // else if (story.currentChoices.Count > 0)
        // {
        //     DisplayChoices();
        // }
        // else
        // {
        //     EndDialogue();
        // }
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

        if (clickToContinuePrompt != null)
            clickToContinuePrompt.SetActive(false);

        OnDialogueEnded?.Invoke();
    }
}