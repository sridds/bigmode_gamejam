using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _stageThemeLoop;
    [SerializeField] private AudioSource _stageThemeIsolated;

    private void Awake()
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
    }

    private void Start()
    {

        GameStateManager.instance.OnGameStateUpdated += GameStateUpdate;
    }

    public void FadeInStageTheme(float fadeTime)
    {
        _stageThemeLoop.DOKill(true);
        _stageThemeLoop.DOFade(1.0f, fadeTime);

        _stageThemeIsolated.DOKill(true);
        _stageThemeIsolated.DOFade(1.0f, fadeTime);
    }

    public void FadeOutStageTheme(float fadeTime)
    {
        _stageThemeLoop.DOKill(true);
        _stageThemeLoop.DOFade(0.0f, fadeTime);

        _stageThemeIsolated.DOKill(true);
        _stageThemeIsolated.DOFade(0.0f, fadeTime);
    }

    private void GameStateUpdate(GameStateManager.PlayerState lastState, GameStateManager.PlayerState newState)
    {
        /*
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
        */
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
