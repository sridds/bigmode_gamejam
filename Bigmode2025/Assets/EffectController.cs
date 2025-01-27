using DG.Tweening;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public static EffectController instance;

    [SerializeField] CinemachineBasicMultiChannelPerlin[] cameraShakes;

    float consistantAmplitude;
    float consistantFrequency;
    float instantAmplitude;
    float instantFrequency;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    public void ContinousScreenShake(float amplitude, float frequency)
    {
        consistantAmplitude = amplitude;
        consistantFrequency = frequency;
    }

    public IEnumerator FreezeFrame(float freezeFrameDuration)
    {
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(freezeFrameDuration);
        Time.timeScale = 1.0f;
    }

    public IEnumerator InstantScreenShake(float duration, float amplitude, float frequency, bool doFadeOut)
    {
        instantAmplitude += amplitude;
        instantFrequency += frequency;
        if (doFadeOut)
        {
            float elapsed = 0;

            while (elapsed < duration)
            {
                instantAmplitude -= amplitude * Time.unscaledDeltaTime / duration;
                instantFrequency -= frequency * Time.unscaledDeltaTime / duration;
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSecondsRealtime(duration);
            instantAmplitude -= amplitude;
            instantFrequency -= frequency;
        }
    }

    private void Update()
    {
        foreach(CinemachineBasicMultiChannelPerlin channel in cameraShakes)
        {
            channel.AmplitudeGain = consistantAmplitude + instantAmplitude;
            channel.FrequencyGain = consistantFrequency + instantFrequency;
            channel.AmplitudeGain = Mathf.Clamp(channel.AmplitudeGain, 0, Mathf.Infinity);
            channel.FrequencyGain = Mathf.Clamp(channel.FrequencyGain, 0, Mathf.Infinity);

        }
    }
}
