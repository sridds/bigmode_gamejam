using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _introTrack;
    [SerializeField] private AudioSource _track1;
    [SerializeField] private AudioSource _track2;

    bool isIntroPlaying;

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

    private void Update()
    {
        if(isIntroPlaying && !_introTrack.isPlaying)
        {
            PlayLoop();
            isIntroPlaying = false;
        }
    }

    public void PlayStageTheme()
    {
        _introTrack.Play();
        isIntroPlaying = true;
    }

    public void PlayLoop()
    {
        _introTrack.Stop();
        _track1.Play();
        _track2.Play();

        isIntroPlaying = false;
    }

    public void FadeInStageTheme(float fadeTime)
    {
        _track1.DOKill(true);
        _track1.DOFade(1.0f, fadeTime);

        _track2.DOKill(true);
        _track2.DOFade(1.0f, fadeTime);
    }

    public void FadeOutStageTheme(float fadeTime)
    {
        _track1.DOKill(true);
        _track1.DOFade(0.0f, fadeTime);

        _track2.DOKill(true);
        _track2.DOFade(0.0f, fadeTime);
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

    public void PlaySound(AudioClip clip, float volume, float minPitch, float maxPitch)
    {
        // temporary
        _sfxSource.pitch = Random.Range(minPitch, maxPitch);
        _sfxSource.PlayOneShot(clip, volume);
    }
}
