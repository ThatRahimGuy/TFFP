using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{

    public AudioMixer audioMixer;

    public Slider musicSlider;
    public Slider sfxSlider;
  
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        AudioManager.instance.UiSFX();
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
        AudioManager.instance.UiSFX();
    }

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SaveVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        audioMixer.GetFloat("SFXVolume", out float sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("Music Volume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }

}

