using UnityEngine;

[CreateAssetMenu(menuName="Music/Song Data")]

public class SongData : ScriptableObject
{
    public string songName;
    public SongFragment[] fragments;
    public AudioClip[] fullMelody;
}
