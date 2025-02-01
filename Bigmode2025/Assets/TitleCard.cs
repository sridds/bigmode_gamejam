using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class TitleCard : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _veni;
    [SerializeField]
    private SpriteRenderer _vidi;
    [SerializeField]
    private SpriteRenderer _vroom;

    [SerializeField]
    private AudioClip _slamSound;

    [SerializeField]
    private AudioClip _splatterSound;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private Sprite _bloodyVroomLogo;

    [SerializeField]
    private float _sinAmplitude = 2.0f;

    [SerializeField]
    private float _sinFrequency = 0.5f;

    bool fullTitleRevealed;
    float ySin;
    float originalYValue;

    void Start()
    {
        _veni.gameObject.SetActive(false);
        _vidi.gameObject.SetActive(false);
        _vroom.gameObject.SetActive(false);

        originalYValue = transform.position.y;

        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(2.0f);

        // veni
        yield return new WaitForSeconds(0.5f);
        _veni.gameObject.SetActive(true);
        _audioSource.PlayOneShot(_slamSound);

        _veni.transform.localScale = new Vector3(4, 4);
        _veni.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.1f);
        _veni.transform.DOShakePosition(0.3f, 2f, 40, 90, false, true, ShakeRandomnessMode.Full);

        // vidi
        yield return new WaitForSeconds(0.5f);
        _vidi.gameObject.SetActive(true);
        _audioSource.PlayOneShot(_slamSound);

        _vidi.transform.localScale = new Vector3(4, 4);
        _vidi.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);

        _vidi.transform.DOShakePosition(0.3f, 2f, 40, 90, false, true, ShakeRandomnessMode.Full);
        _veni.transform.DOShakePosition(0.2f, 0.5f, 40, 90, false, true, ShakeRandomnessMode.Full);

        // vroom
        yield return new WaitForSeconds(0.5f);
        _audioSource.PlayOneShot(_slamSound);
        _vroom.transform.localScale = new Vector3(4, 4);
        _vroom.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);

        _vroom.gameObject.SetActive(true);
        _vroom.transform.DOShakePosition(0.3f, 3f, 40, 90, false, true, ShakeRandomnessMode.Full);
        _vidi.transform.DOShakePosition(0.2f, 0.5f, 40, 90, false, true, ShakeRandomnessMode.Full);
        _veni.transform.DOShakePosition(0.2f, 0.5f, 40, 90, false, true, ShakeRandomnessMode.Full);

        fullTitleRevealed = true;
    }

    private void Update()
    {
        if (!fullTitleRevealed) return;

        ySin = Mathf.Sin(Time.time * _sinFrequency) * _sinAmplitude;
        transform.position = new Vector3(transform.position.x, originalYValue + ySin);
    }

    public void MakeBloody()
    {
        _vroom.transform.DOShakePosition(0.3f, 3f, 40, 90, false, true, ShakeRandomnessMode.Full);
        _vidi.transform.DOShakePosition(0.2f, 0.5f, 40, 90, false, true, ShakeRandomnessMode.Full);
        _veni.transform.DOShakePosition(0.2f, 0.5f, 40, 90, false, true, ShakeRandomnessMode.Full);

        _vroom.sprite = _bloodyVroomLogo;
        AudioManager.instance.PlaySound(_splatterSound, 1.0f, 1.0f, 1.0f);
    }
}
