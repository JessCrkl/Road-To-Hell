using UnityEngine;

[CreateAssetMenu(menuName="Music/Song Fragment")]
public class SongFragment : ScriptableObject
{
    public string fragmentName;
    public int songIndex;     // song this fragment belongs to
    public int correctIndex; // correct order in the sheet
    public Sprite sheetSprite;
    public AudioClip[] notes;
}
