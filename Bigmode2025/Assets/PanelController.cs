using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using DG.Tweening.Core.Easing;
using UnityEngine.SceneManagement;

public class PanelController : MonoBehaviour
{
    [SerializeField]
    private GameObject panel1;
    [SerializeField]
    private GameObject panel2;
    [SerializeField]
    private GameObject panel3;

    [SerializeField]
    private GameObject blackWipe;

    [SerializeField]
    private Image text1;

    [SerializeField]
    private Image text2;

    [SerializeField]
    private Image text3;

    [SerializeField]
    private Image _aGameBy;

    [SerializeField]
    private Image[] names;

    [SerializeField]
    private GameObject flash;

    [SerializeField]
    private GameObject beam;

    [SerializeField]
    private GameObject panelToScroll;

    [SerializeField]
    private GameObject carGameObject;

    [SerializeField]
    private Animator carAnimator;

    [SerializeField]
    private TrailRenderer _trailLeft;

    [SerializeField]
    private TrailRenderer _trailRight;

    [SerializeField]
    private AudioClip beamClip;

    [SerializeField]
    private AudioSource carMotorSource;

    [SerializeField]
    private AudioClip panelAppearClip;

    [SerializeField]
    private float _panel2MoveSpeed = 3.0f;

    [SerializeField]
    private GameObject _panel2Contents;

    [SerializeField]
    private GameObject _panel2Renderer;

    [SerializeField]
    private RectTransform _panel3Rect;

    [SerializeField]
    private UnityEngine.UI.Outline _outline;

    [SerializeField]
    private GameObject _newCar;

    [SerializeField]
    private DummySpearEnemy[] exploders;

    [SerializeField]
    private TitleCard titleCard;

    [SerializeField]
    private SpriteRenderer pressAnyKey;

    [SerializeField]
    private float flashInterval;

    [SerializeField]
    private float timeBeforePanel2 = 2.0f;

    [SerializeField]
    private float timeBeforeBeam = 0.5f;

    [SerializeField]
    private AudioClip pressStartSound;

    [SerializeField]
    private AudioClip startGameSound;

    float ySin;
    float startingY;
    float flashTime;

    private void Start()
    {
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);
        
        StartCoroutine(ShowPanel());

        startingY = _newCar.transform.position.y;
    }

    bool canMovePanel;
    bool canPressAnyKey;
    bool pressedKey;

    private void Update()
    {
        _panel2Renderer.transform.position = new Vector3(0, 0);

        if (!canMovePanel) return;
        _panel2Contents.transform.position += new Vector3(_panel2MoveSpeed * Time.deltaTime, 0, 0);
        ySin = Mathf.Sin(0.8f * Time.time) * 0.8f;
        _newCar.transform.position = new Vector3(_newCar.transform.position.x, startingY + ySin);

        if (canPressAnyKey && !pressedKey)
        {
            flashTime += Time.deltaTime;

            pressAnyKey.gameObject.SetActive(true);

            if(flashTime > flashInterval)
            {
                pressAnyKey.enabled = !pressAnyKey.enabled;
                flashTime = 0.0f;
            }
        }

        if(canPressAnyKey && Input.anyKey && !pressedKey)
        {
            StartCoroutine(PressedKey());
            pressedKey = true;
        }

        if (pressedKey)
        {
            flashTime += Time.deltaTime;

            if (flashTime > flashInterval / 2)
            {
                pressAnyKey.enabled = !pressAnyKey.enabled;
                flashTime = 0.0f;
            }
        }
    }

    private IEnumerator PressedKey()
    {
        AudioManager.instance.StopIntro();
        GetComponent<AudioSource>().PlayOneShot(startGameSound, 1.0f);
        titleCard.NoMoreSinPlease();
        flash.SetActive(true);
        flash.GetComponent<SpriteRenderer>().DOFade(0.0f, 1.0f);
        yield return new WaitForSeconds(3);

        blackWipe.transform.DOMoveX(0.0f, 0.8f, false);

        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator ShowPanel()
    {
        yield return new WaitForSeconds(2.0f);

        AudioManager.instance.PlaySound(pressStartSound, 1.0f, 1.0f, 1.0f);
        _aGameBy.DOFade(1.0f, 0.4f);

        yield return new WaitForSeconds(1.0f);

        foreach (Image i in names)
        {
            i.transform.position = new Vector3(i.transform.position.x, i.transform.position.y - 3);

            i.transform.DOMoveY(i.transform.position.y + 3, 0.4f, false);
            i.DOFade(1.0f, 0.4f);

            AudioManager.instance.PlaySound(pressStartSound, 1.0f, 1.0f, 1.0f);

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(2.0f);

        _aGameBy.DOFade(0.0f, 0.4f);
        foreach (Image i in names)
        {
            i.DOFade(0.0f, 0.4f);
        }
        yield return new WaitForSeconds(2.0f);

        AudioManager.instance.PlayIntro();
        yield return new WaitForSeconds(2.0f);

        panel1.transform.localScale = new Vector3(1.5f, 1.5f, 3);
        panel1.SetActive(true);
        AudioManager.instance.PlaySound(panelAppearClip, 1.0f, 0.95f, 1.1f);

        // fly in text
        text1.transform.position = new Vector3(text1.transform.position.x - 5, text1.transform.position.y);
        text1.transform.DOMoveX(text1.transform.position.x + 5, 0.4f, false).SetEase(Ease.OutQuad);
        text1.DOFade(1.0f, 0.4f).SetEase(Ease.OutQuad);

        yield return panel1.transform.DOScale(1.0f, 0.2f).SetEase(Ease.Linear).WaitForCompletion();
        panelToScroll.transform.DOMoveY(panelToScroll.transform.position.y - 4.0f, timeBeforePanel2 + 0.4f, false).SetEase(Ease.Linear);
        yield return panel1.transform.DOShakePosition(0.4f, 6.0f, 40).WaitForCompletion();


        yield return new WaitForSeconds(timeBeforePanel2);

        panel2.SetActive(true);
        panel2.transform.localScale = new Vector3(1.5f, 1.5f, 3);

        text2.transform.position = new Vector3(text2.transform.position.x - 5, text2.transform.position.y);
        text2.transform.DOMoveX(text2.transform.position.x + 5, 0.4f, false).SetEase(Ease.OutQuad);
        text2.DOFade(1.0f, 0.4f).SetEase(Ease.OutQuad);

        yield return panel2.transform.DOScale(1.0f, 0.2f).SetEase(Ease.Linear).WaitForCompletion();
        panel1.transform.DOShakePosition(0.4f, 3.0f, 40);
        AudioManager.instance.PlaySound(panelAppearClip, 1.0f, 0.95f, 1.1f);
        yield return panel2.transform.DOShakePosition(0.4f, 6.0f, 40).WaitForCompletion();

        yield return new WaitForSeconds(timeBeforeBeam);

        beam.SetActive(true);
        AudioManager.instance.PlaySound(beamClip, 0.7f, 1.0f, 1.0f);
        carGameObject.SetActive(true);

        yield return new WaitForSeconds(0.6f);
        foreach (DummySpearEnemy spearEnemy in exploders)
        {
            spearEnemy.Yell();
        }

        yield return new WaitForSeconds(0.4f);
        _trailLeft.emitting = true;
        _trailRight.emitting = true;
        carMotorSource.enabled = true;
        carMotorSource.DOFade(0.0f, 4.0f);
        text3.gameObject.SetActive(true);


        foreach (DummySpearEnemy spearEnemy in exploders)
        {
            spearEnemy.Explode();
        }
        EffectController.instance.StartCoroutine(EffectController.instance.InstantScreenShake(0.5f, 15, 200, true));

        yield return new WaitForSeconds(2);
        carAnimator.SetBool("Drive", true);

        text1.DOFade(0.0f, 0.4f);
        text2.DOFade(0.0f, 0.4f);
        text3.DOFade(0.0f, 0.4f);

        panel3.SetActive(true);
        canMovePanel = true;
        panel3.transform.localScale = new Vector3(1.5f, 1.5f, 3);
        yield return panel3.transform.DOScale(1.0f, 0.2f).SetEase(Ease.Linear).WaitForCompletion();
        panel1.transform.DOShakePosition(0.4f, 3.0f, 40);
        panel2.transform.DOShakePosition(0.4f, 3.0f, 40);
        AudioManager.instance.PlaySound(panelAppearClip, 1.0f, 0.95f, 1.1f);
        yield return panel3.transform.DOShakePosition(0.4f, 6.0f, 40).WaitForCompletion();

        _newCar.transform.DOLocalMoveX(_newCar.transform.localPosition.x + 12f, 1.0f);
        yield return new WaitForSeconds(1);
        _panel3Rect.DOSizeDelta(new Vector2(480, 360), 0.5f, false).SetEase(Ease.OutQuad);
        _panel3Rect.DOLocalMoveY(0, 0.5f, false).SetEase(Ease.OutQuad);
        _outline.DOColor(Color.black, 0.5f).SetEase(Ease.OutQuad);

        titleCard.gameObject.SetActive(true);

        yield return new WaitForSeconds(4.0f);
        canPressAnyKey = true;
        AudioManager.instance.PlaySound(pressStartSound, 1.0f, 1.0f, 1.0f);

        FindObjectOfType<MenuSpawners>().StartSpawning();

        yield return null;
    }

    public void MakeLogoBloody()
    {
        titleCard.MakeBloody();
    }
}
