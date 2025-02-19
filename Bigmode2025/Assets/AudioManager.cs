using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _introTrack;
    [SerializeField] private AudioSource _track1;
    [SerializeField] private AudioSource _track2;
    [SerializeField] private AudioSource _shopTheme;

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

    public void StopAll()
    {
        _track1.Stop();
        _track2.Stop();
        _shopTheme.Stop();
    }
    private void Start()
    {
        //GameStateManager.instance.OnGameStateUpdated += GameStateUpdate;
    }

    public void DamageEffect()
    {
        StartCoroutine(IDamageEffect());
    }

    private IEnumerator IDamageEffect()
    {
        _track1.DOKill(true);
        _track2.DOKill(true);

        _track1.DOPitch(0.5f, 0.2f).SetUpdate(UpdateType.Normal, true);
        _track2.DOPitch(0.5f, 0.2f).SetUpdate(UpdateType.Normal, true);

        yield return new WaitForSecondsRealtime(0.2f);

        _track1.DOPitch(1.0f, 0.8f).SetUpdate(UpdateType.Normal, true);
        _track2.DOPitch(1.0f, 0.8f).SetUpdate(UpdateType.Normal, true);
    }

    public void PlayIntro()
    {
        _track1.Stop();
        _track2.Stop();
        _introTrack.Play();
    }

    public void StopIntro()
    {
        _introTrack.Stop();
    }

    public void PlayShopTheme()
    {
        _shopTheme.volume = 1.0f;
        _track1.Stop();
        _track2.Stop();
        _shopTheme.Play();
    }

    public void StartStageTheme()
    {
        if (_track1.isPlaying) return;

        _shopTheme.Stop();
        _track1.Play();
        _track2.Play();
        _track1.volume = 1.0f;
        _track2.volume = 1.0f;
        _track2.mute = true;
    }

    public void SwitchToBacking()
    {
        _track1.mute = true;
        _track2.mute = false;
    }

    public void SwitchToMain()
    {
        _track1.mute = false;
        _track2.mute = true;
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
