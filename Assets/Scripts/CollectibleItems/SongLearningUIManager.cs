using UnityEngine;

public class SongLearningUIManager : MonoBehaviour
{
    public static SongLearningUIManager Instance;

    [Header("References")]
    public GameObject songLearningPanel;
    // public PlayerInput playerInput;       
    public FPSController fpsController;   

    private void Awake()
    {
        Instance = this;
    }

    public void OpenSongLearningUI(int songIndex)
    {
        fpsController.SongLearningActive = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Switch Action Map so movement stops
        // playerInput.SwitchCurrentActionMap("SongLearning");

        // Hard kill any leftover movement
        fpsController.moveInput = Vector2.zero;
        fpsController.lookInput = Vector2.zero;
        fpsController.sprintInput = false;

        songLearningPanel.SetActive(true);

        // BuildPuzzle(songIndex);
    }

    public void CloseSongLearningUI()
    {
        songLearningPanel.SetActive(false);

        // Go back to normal movement
        // playerInput.SwitchCurrentActionMap("Player");

        fpsController.SongLearningActive = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void CheckCompletion()
{
    // foreach (var slot in activeSlots)
    // {
    //     if (slot.placedFragment == null)
    //         return;

    //     if (slot.placedFragment.fragmentIndex != slot.slotIndex)
    //         return; // A wrong piece is placed
    // }

    Debug.Log("SONG COMPLETED!");
    // SongFragmentManager.Instance.MarkSongLearned(currentSongIndex);

    CloseSongLearningUI();
}

}
