using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SongLearningUIManager : MonoBehaviour
{
    public static SongLearningUIManager Instance;

    [Header("UI References")]
    public GameObject songLearningPanel;        // Whole UI panel
    public Transform fragmentPanel;             // Where draggable fragments appear
    public Transform staffSlotPanel;            // Slots on the staff where pieces go
    public StaffSlot staffSlotPrefab;           // Prefab for each slot on staff
    public FragmentDraggable fragmentPrefab;    // Prefab for draggable fragments

    [Header("Gameplay")]
    public int currentSongIndex;
    public GameObject playerController;        // Your FPS Controller GameObject

    private List<StaffSlot> activeSlots = new();

    private void Awake()
    {
        Instance = this;
    }

    public void OpenSongLearningUI(int songIndex)
    {
        currentSongIndex = songIndex;

        // Disable player movement
        if (playerController != null)
            playerController.SetActive(false);

        // Show UI
        songLearningPanel.SetActive(true);

        BuildPuzzle();
    }

    public void CloseSongLearningUI()
    {
        // Hide UI
        songLearningPanel.SetActive(false);

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
        List<SongFragment> collected =
            SongFragmentManager.Instance.GetFragmentsForSong(currentSongIndex);

        // 1. Create staff slots (sorted by fragmentIndex)
        collected.Sort((a, b) => a.correctIndex.CompareTo(b.correctIndex));

        for (int i = 0; i < collected.Count; i++)
        {
            StaffSlot slot = Instantiate(staffSlotPrefab, staffSlotPanel);
            slot.slotIndex = collected[i].correctIndex;
            activeSlots.Add(slot);
        }

        // 2. Create draggable fragments (unsorted = puzzle)
        foreach (SongFragment frag in collected)
        {
            FragmentDraggable drag = Instantiate(fragmentPrefab, fragmentPanel);
            drag.fragment = frag;
            drag.BindData();
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

        // all placed — solve
        Debug.Log("SONG COMPLETED!");

        SongFragmentManager.Instance.MarkSongLearned(currentSongIndex);

        CloseSongLearningUI();
    }
}
