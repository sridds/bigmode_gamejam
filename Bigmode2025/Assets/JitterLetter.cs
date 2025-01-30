using UnityEngine;
using DG.Tweening;

public class Jitter : MonoBehaviour
{
    [SerializeField]
    private float shakeAmount = 0.1f;

    [SerializeField]
    private float shakeInterval = 0.05f;

    [SerializeField]
    private float lifeTime = 2.0f;

    float timer;
    Vector2 startingPosition;

    private void Start()
    {
        startingPosition = transform.localPosition;
    }

    void Update()
    {
        timer += Time.unscaledDeltaTime;

        if (timer > shakeInterval)
        {
            transform.localPosition = startingPosition + new Vector2(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount));
            timer = 0.0f;
        }
    }
}
