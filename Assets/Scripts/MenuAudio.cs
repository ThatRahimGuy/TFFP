using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip hoverSound;
    public AudioClip pressedSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void OnHover()
    {
        audio.clip = hoverSound;
        audio.Play();
    }

    public void OnPressed()
    {
        audio.clip = pressedSound;
        audio.Play();
    }
}
