using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

public class SongLearningUIManager : MonoBehaviour
{
    public static SongLearningUIManager Instance;

    [Header("UI References")]
    public GameObject songLearningPanel;        
    public Transform fragmentPanel;             
    public Transform staffSlotPanel;            
    public StaffSlot staffSlotPrefab;           
    public FragmentDraggable fragmentPrefab;    
    public GameObject helpPanel;
    public Button helpButton;
    public Button backButton;
   

    [Header("Gameplay")]
    public int currentSongIndex;
    public FPSController fpsController;        
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

         OpenSongLearningUI(0); // for testing
    }

    private void OnEnable()
    {
        // songlearning the action map
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
        Debug.Log($"Opening Song Learning UI for songIndex {songIndex}");
        currentSongIndex = songIndex;

        // stop player movement
        if (fpsController != null)
        fpsController.SongLearningActive = true;

        songLearningPanel.SetActive(true);
        helpPanel.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

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
        songLearningPanel.SetActive(false);
        helpPanel.SetActive(false);

        // start player movement again
        if (fpsController != null)
        fpsController.SongLearningActive = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // cleanup puzzle
        foreach (var slot in activeSlots)
            Destroy(slot.gameObject);
        activeSlots.Clear();

        foreach (Transform child in fragmentPanel)
            Destroy(child.gameObject);
    }

    private void BuildPuzzle()
{
    Debug.Log($"[SongLearning] BuildPuzzle for songIndex={currentSongIndex}");

    // Get the collected fragments for the current song
    List<SongFragment> collected = SongFragmentManager.Instance.GetFragmentsForSong(currentSongIndex);

    if (collected.Count == 0 && song != null && song.fragments != null)
    {
        collected = new List<SongFragment>(song.fragments);
        Debug.Log($"[SongLearning] Test mode: using {collected.Count} fragments from SongData.");
    }

    if (collected.Count == 0)
    {
        Debug.LogWarning("[SongLearning] No fragments found for this song.");
        return;
    }

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
            if (activeSlots[i].placedFragment.correctIndex != activeSlots[i].slotIndex)
            {
                isCorrect = false;
                break;
            }

        }
        
        if (isCorrect) {
            Debug.Log("Song Completed! Unlocking song...");
            
            UnlockSong();
            SongFragmentManager.Instance.MarkSongLearned(currentSongIndex);
            CloseSongLearningUI();
        }
    }
}
