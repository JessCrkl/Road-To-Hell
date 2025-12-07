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
    public AudioSource melodyHelper;
    public InputActionAsset inputActions;
    private InputAction dragFragmentAction;
    private InputAction dropFragmentAction;

    private List<StaffSlot> activeSlots = new();
    [Header("Audio")]
    public Button playHintButton;
    public AudioClip hintAudio;
    private bool isHintPlaying = false;
    public AudioClip incorrectClip; 
    public AudioClip correctClip; 

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (playHintButton != null) {
        playHintButton.onClick.AddListener(PlayHint);
        }
        if (helpButton != null)
        {
            helpButton.onClick.AddListener(OpenHelpPanel);
        }
        if (backButton != null)
        {
            backButton.onClick.AddListener(CloseHelpPanel);
        }

         OpenSongLearningUI(0); // FOR TESTING
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

    // 1. Build slots from the *solution* order (song.fragments)
    foreach (Transform child in staffSlotPanel) {
        Destroy(child.gameObject);}
    
    activeSlots.Clear();

    if (song == null || song.fragments == null || song.fragments.Length == 0)
    {
        Debug.LogWarning("[SongLearning] Song has no fragments/solution.");
        return;
    }

    for (int i = 0; i < song.fragments.Length; i++)
    {
        SongFragment expected = song.fragments[i];

        StaffSlot slot = Instantiate(staffSlotPrefab, staffSlotPanel);
        slot.slotIndex = i;
        slot.expectedFragment = expected;
        activeSlots.Add(slot);
    }

    // 2. Create draggable fragments at the bottom
    // This part will display the same fragment multiple times, as needed
    foreach (Transform child in fragmentPanel) {
        Destroy(child.gameObject); }

    SongFragment[] palette = song.paletteFragments != null && song.paletteFragments.Length > 0? song.paletteFragments : song.fragments;

    foreach (SongFragment frag in palette)
    {
        SpawnFragmentInPalette(frag);
    }
}
public void SpawnFragmentInPalette(SongFragment frag)
    {
        FragmentDraggable drag = Instantiate(fragmentPrefab, fragmentPanel);
        drag.fragment = frag;
        drag.BindData();

        drag.isPaletteSource = true;
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
            melodyHelper.PlayOneShot(note);
            yield return new WaitForSeconds(note.length * 0.9f);
        }
    }

    private void PlayHint()
    {
        if (melodyHelper == null || hintAudio == null) return;

        // no spamming the help button
        if (isHintPlaying) return;

        StartCoroutine(PlayHintCoroutine());
    }

    private IEnumerator PlayHintCoroutine()
    {
        isHintPlaying = true;

        melodyHelper.PlayOneShot(hintAudio);

        yield return new WaitForSeconds(hintAudio.length);

        isHintPlaying = false;
    }

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
            StaffSlot slot = activeSlots[i];
            if (slot.placedFragment != slot.expectedFragment)
            {
                // Debug.Log("Song Incorrect!");
                isCorrect = false;
                StartCoroutine(HandleIncorrectSong());
                return;
            }

        }
        
        if (isCorrect) {
            StartCoroutine(HandleCorrectSong());
        }
    }

    private IEnumerator HandleCorrectSong()
    {
        Debug.Log("Song Completed! Unlocking song...");
            
            if (correctClip != null && melodyHelper != null)
            {
                melodyHelper.PlayOneShot(correctClip);
            }
            yield return new WaitForSeconds(0.4f);

            UnlockSong();
            SongFragmentManager.Instance.MarkSongLearned(currentSongIndex);
            CloseSongLearningUI();
    }

    private IEnumerator HandleIncorrectSong()
{
    // optional: small delay before clearing, or do visual shake first

    if (incorrectClip != null && melodyHelper != null)
    {
        melodyHelper.PlayOneShot(incorrectClip);
    }

    yield return new WaitForSeconds(0.4f);

    foreach (var slot in activeSlots)
    {
        slot.ClearFragment();
    }

    Debug.Log("Song incorrect. Slots cleared.");
}
}
