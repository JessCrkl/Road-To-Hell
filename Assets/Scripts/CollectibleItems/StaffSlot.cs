using UnityEngine;
using UnityEngine.UI;

public class StaffSlot : MonoBehaviour
{
    public int slotIndex;
    public Image slotImage; 
    public SongFragment placedFragment;
    public SongFragment expectedFragment;
    public bool IsEmpty => placedFragment == null;
    public Sprite emptySprite;

    public void PlaceFragment(SongFragment fragment)
    {
        placedFragment = fragment;
        slotImage.sprite = fragment.sheetSprite;

        SongLearningUIManager.Instance.CheckCompletion();
    }

    public void ClearFragment()
    {
        placedFragment = null;

        if (emptySprite != null)
        {
            slotImage.sprite = emptySprite;
        } else
        {
            slotImage.sprite = null;
        }
            
    }
}
