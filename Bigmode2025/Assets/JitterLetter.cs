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
        startingPosition = transform.position;
        //Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > shakeInterval)
        {
            transform.localPosition = new Vector2(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount));
            timer = 0.0f;
        }
    }
}
