using UnityEngine;

public class SoundManager : MonoBehaviour
{
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
    
    private void PlayMusic(AudioClip clip)
    {
        if (music.isPlaying)
        {
            music.Stop();
        }
        
        music.clip = clip;
        music.loop = true;
        music.Play();
    }
    
    public void PlaySound(AudioClip clip)
    {
        sfx.PlayOneShot(clip);
    }
}