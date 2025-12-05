using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

public class SongLearningUIManager : MonoBehaviour
{
    public static SongLearningUIManager Instance;

    [Header("UI References")]
    public GameObject songLearningPanel;        // Whole UI panel
    public Transform fragmentPanel;             // Where draggable fragments appear
    public Transform staffSlotPanel;            // Slots on the staff where pieces go
    public StaffSlot staffSlotPrefab;           // Prefab for each slot on staff
    public FragmentDraggable fragmentPrefab;    // Prefab for draggable fragments
    public GameObject helpPanel;
    public Button helpButton;
    public Button backButton;
   

    [Header("Gameplay")]
    public int currentSongIndex;
    public GameObject playerController;        // FPS Controller GameObject
    public SongData song;
    public AudioSource audioSource;
    public InputActionAsset inputActions;
    private InputAction dragFragmentAction;
    private InputAction dropFragmentAction;

    private List<StaffSlot> activeSlots = new();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (helpButton != null)
        {
            helpButton.onClick.AddListener(OpenHelpPanel);
        }
        if (backButton != null)
        {
            backButton.onClick.AddListener(CloseHelpPanel);
        }
    }

    private void OnEnable()
    {
        // Enable the action map
        var songLearningMap = inputActions.FindActionMap("SongLearning");

        dragFragmentAction = songLearningMap.FindAction("DragFragment");
        dropFragmentAction = songLearningMap.FindAction("DropFragment");

        dragFragmentAction.Enable();
        dropFragmentAction.Enable();
    }

    private void OnDisable()
    {
        dragFragmentAction.Disable();
        dropFragmentAction.Disable();
    }

    public void OpenSongLearningUI(int songIndex)
    {
        currentSongIndex = songIndex;

        // Disable player movement
        if (playerController != null)
            playerController.SetActive(false);

        // Show UI
        songLearningPanel.SetActive(true);
        helpPanel.SetActive(false);

        BuildPuzzle();
    }

    public void OpenHelpPanel()
    {
        helpPanel.SetActive(true);
    }

    private void CloseHelpPanel()
    {
        helpPanel.SetActive(false);
    }

    public void CloseSongLearningUI()
    {
        // Hide UI
        songLearningPanel.SetActive(false);
        helpPanel.SetActive(false);

        // Re-enable player movement
        if (playerController != null)
            playerController.SetActive(true);

        // Cleanup old puzzle
        foreach (var slot in activeSlots)
            Destroy(slot.gameObject);
        activeSlots.Clear();

        foreach (Transform child in fragmentPanel)
            Destroy(child.gameObject);
    }

    private void BuildPuzzle()
{
    // Get the collected fragments for the current song
    List<SongFragment> collected = SongFragmentManager.Instance.GetFragmentsForSong(currentSongIndex);

    // 1. Create staff slots (sorted by correctIndex)
    collected.Sort((a, b) => a.correctIndex.CompareTo(b.correctIndex));

    // Create the puzzle slots (staff positions) based on the fragments collected
    for (int i = 0; i < collected.Count; i++)
    {
        StaffSlot slot = Instantiate(staffSlotPrefab, staffSlotPanel);
        slot.slotIndex = collected[i].correctIndex;
        activeSlots.Add(slot);
    }

    // 2. Create draggable fragments at the bottom
    // This part will display the same fragment multiple times, as needed
    foreach (SongFragment frag in collected)
    {
        // Create an instance of each fragment to be displayed at the bottom of the screen
        FragmentDraggable drag = Instantiate(fragmentPrefab, fragmentPanel);
        drag.fragment = frag;  // Reference the correct SongFragment data
        drag.BindData();       // This will assign the correct sprite to the fragment
    }
}


    void UnlockSong()
    {
        // Play full melody
        StartCoroutine(PlayMelody());

        // Mark in save file that song is learned
        PlayerStats.Instance.UnlockedSongs.Add(song.songName);
        Debug.Log("Learned Song: " + song.songName);
    }

    IEnumerator PlayMelody()
    {
        foreach (var note in song.fullMelody)
        {
            audioSource.PlayOneShot(note);
            yield return new WaitForSeconds(note.length * 0.9f);
        }
    }

    // Called by StaffSlot every time it places a fragment
    public void CheckCompletion()
    {
        foreach (StaffSlot slot in activeSlots)
        {
            if (slot.placedFragment == null)
                return;
        }
        
        // check if in correct order
        bool isCorrect = true;
        for (int i = 0; i < activeSlots.Count; i++)
        {
            if (activeSlots[i].placedFragment.correctIndex != i)
            {
                // not in correct order, return
                isCorrect = false;
                return;
            }
        }
        
        if (isCorrect) {
            Debug.Log("SONG COMPLETED!");
            
            UnlockSong();
            SongFragmentManager.Instance.MarkSongLearned(currentSongIndex);
            CloseSongLearningUI();
        }
    }
}
