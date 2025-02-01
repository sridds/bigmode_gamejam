using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ScreenJumper : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip screamClip;

    [SerializeField]
    private AudioClip slamClip;

    [SerializeField]
    private AudioClip glassSlam;

    void Start()
    {
        transform.parent = Camera.main.transform;

        StartCoroutine(ScreenJump());
    }

    private IEnumerator ScreenJump()
    {
        source.pitch = Random.Range(1.3f, 1.4f);
        source.clip = screamClip;
        source.Play();

        float endZValue = Random.Range(-15, 15);
        transform.DORotate(new Vector3(0, 0, endZValue + 360), 0.6f, RotateMode.WorldAxisAdd);

        bool leftCentric = Random.Range(0, 2) == 1;
        float xVal = leftCentric ? Random.Range(-14, -9) : Random.Range(9, 14);

        transform.DOLocalJump(new Vector3(xVal, Random.Range(-4, 2), 10), 2.0f, 1, 0.6f).SetEase(Ease.InQuad);
        yield return transform.DOScale(3.0f, 0.6f).WaitForCompletion();

        // smack screen
        source.Stop();
        source.volume = 1;

        source.pitch = Random.Range(0.95f, 1.1f);
        source.PlayOneShot(slamClip);
        source.PlayOneShot(glassSlam);

        transform.DOShakePosition(0.4f, 2, 40, 90, false, true, ShakeRandomnessMode.Full);
        EffectController.instance.StartCoroutine(EffectController.instance.InstantScreenShake(0.4f, 20, 200, true));

        yield return new WaitForSeconds(1.0f);
        transform.DOLocalMoveY(-35, 2.0f, false).SetEase(Ease.Linear);

        yield return null;

        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
}
