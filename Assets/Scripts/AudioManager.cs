using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    private AudioSource audioSource;

    [SerializeField] private AudioClip gameMusic; // 🎵 One main soundtrack

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 🚀 Keep music through all scenes
        }
        else
        {
            Destroy(gameObject); // 🔥 Prevent duplicates
            return;
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = gameMusic;
        audioSource.loop = true; // 🔁 Keep looping
        audioSource.Play();
    }
}
