using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _stageThemeLoop;
    [SerializeField] private AudioSource _stageThemeIsolated;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);

        GameStateManager.instance.OnGameStateUpdated += GameStateUpdate;
    }

    private void GameStateUpdate(GameStateManager.PlayerState lastState, GameStateManager.PlayerState newState)
    {
        if(newState == GameStateManager.PlayerState.Playing)
        {
            _stageThemeLoop.mute = false;
            _stageThemeIsolated.mute = true;
        }

        else
        {
            _stageThemeLoop.mute = true;
            _stageThemeIsolated.mute = false;
        }
    }

    private void Update()
    {
    }

    public void PlaySound(AudioClip clip, float volume, float minPitch, float maxPitch)
    {
        // temporary
        _sfxSource.pitch = Random.Range(minPitch, maxPitch);
        _sfxSource.PlayOneShot(clip, volume);
    }
}
