using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip currentSong;
    [SerializeField] private AudioSource music, sfx;
    
    public static SoundManager Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void PlayAudioClip(AudioClip clip)
    {
        if (music.isPlaying)
        {
            music.Stop();
        }
        
        music.clip = clip;
        music.loop = true;
        music.Play();
    }
    
    public void PlayMusic()
    {
        if (music.isPlaying)
        {
            music.Stop();
        }
        
        music.clip = currentSong;
        music.loop = true;
        music.Play();
    }
    
    public void PlaySound(AudioClip clip, float volume = 1.0f)
    {
        sfx.PlayOneShot(clip);
        sfx.volume = volume;
    }
}