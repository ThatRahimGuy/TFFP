using UnityEngine;

[System.Serializable]
public struct MusicTrack
{
    public string trackName;
    public AudioClip clip;
}

public class MusicLibrary : MonoBehaviour
{
    public MusicTrack[] tracks;
    private string trackName;

    public AudioClip GetClipFromName(string name)
    {
        foreach (var track in tracks)
        {
            if (track.trackName == trackName)
            {
               return track.clip;
            }
        }
        return null;
    }
}
