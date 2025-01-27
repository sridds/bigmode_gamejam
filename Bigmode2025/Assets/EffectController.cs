using DG.Tweening;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public static EffectController instance;

    Tween cameraShakeTween;
    Vector3 shakeStartLocation;
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
        ScreenShake(2, 0.2f, 5, 90);
    }

    public void ScreenShake(float duration, float strength, int vibrado, float randomness)
    {
        shakeStartLocation = Camera.main.transform.position;
        cameraShakeTween = Camera.main.DOShakePosition(duration, strength, vibrado, randomness);
    }

    void EndShakeTween()
    {
        if (cameraShakeTween == null)
        {
            Debug.Log(Time.time);
        }
    }

    private void Update()
    {
        EndShakeTween();
    }
}
