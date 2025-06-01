using UnityEngine;

public class Barbell : MonoBehaviour
{
    [SerializeField] private AudioClip barbellSound;
    
    public void DownSound()
    {
        SoundManager.Instance.PlaySound(barbellSound);
    }
}
