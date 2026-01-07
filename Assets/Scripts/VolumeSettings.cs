using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_SFX = "SFXVolume";

    void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    void SetMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }
}
