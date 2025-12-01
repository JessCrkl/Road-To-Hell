using UnityEngine;
using UnityEngine.UI;

public class StaffSlot : MonoBehaviour
{
    public int slotIndex;
    public Image slotImage; 
    public SongFragment placedFragment;

    public bool CanPlace(SongFragment fragment)
    {
        return fragment.correctIndex == slotIndex;
    }

    public void PlaceFragment(SongFragment fragment)
    {
        placedFragment = fragment;
        slotImage.sprite = fragment.sheetSprite;

        SongLearningUIManager.Instance.CheckCompletion();
    }
}
