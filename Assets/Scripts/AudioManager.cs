using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] AudioSource UISource;
    [SerializeField] List<AudioClip> uiClips = new List<AudioClip> ();

    public void UiSFX()
    {
        AudioClip clip = uiClips[0];

        UISource.Play();
    }
}
