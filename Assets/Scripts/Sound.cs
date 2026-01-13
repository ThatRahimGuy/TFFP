using UnityEngine;

[System.Serializable]
public class Sound : MonoBehaviour
{
    [HideInInspector] public AudioSource source;
    public string clipName;
    public AudioClip audioClip;
    public bool isLoop;
    public bool playOnAwake;

    [Range(0, 1)]
    public float volume = 0.5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
