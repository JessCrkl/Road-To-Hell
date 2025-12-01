using System.Collections.Generic;
using UnityEngine;

public class SongFragmentManager : MonoBehaviour
{
    public static SongFragmentManager Instance;
    private HashSet<int> learnedSongs = new();

    private void Awake()
    {
        Instance = this;
    }

    private HashSet<SongFragment> collectedFragments = new HashSet<SongFragment>();

    public void CollectFragment(SongFragment fragment)
    {
        collectedFragments.Add(fragment);
    }

    public bool HasFragment(int songIndex, int fragmentIndex)
    {
        foreach (var f in collectedFragments)
        {
            if (f.songIndex == songIndex && f.correctIndex == fragmentIndex)
                return true;
        }
        return false;
    }

    public List<SongFragment> GetFragmentsForSong(int songIndex)
    {
        List<SongFragment> list = new List<SongFragment>();
        foreach (var f in collectedFragments)
        {
            if (f.songIndex == songIndex)
                list.Add(f);
        }
        return list;
    }

    public void MarkSongLearned(int songIndex)
    {
        learnedSongs.Add(songIndex);
    }

    public bool IsSongLearned(int songIndex)
    {
        return learnedSongs.Contains(songIndex);
    }
}
