using UnityEngine;
using System.Collections;

public class SongLearnManager : MonoBehaviour
{
    public StaffSlot[] slots;
    public SongData song;
    public AudioSource audioSource;

    public void CheckCompletion()
    {
        foreach (var slot in slots)
        {
            if (slot.placedFragment == null) return;
        }

        // All fragments placed correctly!
        UnlockSong();
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
}
