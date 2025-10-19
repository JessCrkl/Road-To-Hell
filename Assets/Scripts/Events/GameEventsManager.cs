using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

    public QuestEvents questEvents;
    public DialogueEvents dialogueEvents;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Game Events Manager in scene.");

        }

        instance = this;

        //initialize events
        questEvents = new QuestEvents();
        dialogueEvents = new DialogueEvents();
    }
}
