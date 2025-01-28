using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource _sfxSource;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySound(AudioClip clip, float volume, float minPitch, float maxPitch)
    {
        // temporary
        _sfxSource.pitch = Random.Range(minPitch, maxPitch);
        _sfxSource.PlayOneShot(clip, volume);
    }
}
