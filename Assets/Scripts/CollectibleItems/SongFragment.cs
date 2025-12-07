using UnityEngine;

[CreateAssetMenu(menuName="Music/Song Fragment")]
public class SongFragment : ScriptableObject
{
    public string fragmentName;
    public int songIndex;
    public int correctIndex;
    public Sprite sheetSprite;
    public AudioClip[] notes;
}
