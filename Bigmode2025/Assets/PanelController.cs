using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    [SerializeField]
    private GameObject panel1;
    [SerializeField]
    private GameObject panel2;
    [SerializeField]
    private GameObject panel3;

    [SerializeField]
    private GameObject beam;

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

    private void Start()
    {
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);
        
        StartCoroutine(ShowPanel());
    }

    bool canMovePanel;

    private void Update()
    {
        _panel2Renderer.transform.position = new Vector3(0, 0);

        if (!canMovePanel) return;
        _panel2Contents.transform.position += new Vector3(_panel2MoveSpeed * Time.deltaTime, 0, 0);
    }

    private IEnumerator ShowPanel()
    {
        yield return new WaitForSeconds(2.0f);
        AudioManager.instance.PlayStageTheme();
        yield return new WaitForSeconds(2.0f);

        panel1.transform.localScale = new Vector3(1.5f, 1.5f, 3);
        panel1.SetActive(true);
        AudioManager.instance.PlaySound(panelAppearClip, 1.0f, 0.95f, 1.1f);
        yield return panel1.transform.DOScale(1.0f, 0.2f).SetEase(Ease.Linear).WaitForCompletion();
        yield return panel1.transform.DOShakePosition(0.4f, 6.0f, 40).WaitForCompletion();

        yield return new WaitForSeconds(2.0f);

        panel2.SetActive(true);
        panel2.transform.localScale = new Vector3(1.5f, 1.5f, 3);
        yield return panel2.transform.DOScale(1.0f, 0.2f).SetEase(Ease.Linear).WaitForCompletion();
        panel1.transform.DOShakePosition(0.4f, 3.0f, 40);
        AudioManager.instance.PlaySound(panelAppearClip, 1.0f, 0.95f, 1.1f);
        yield return panel2.transform.DOShakePosition(0.4f, 6.0f, 40).WaitForCompletion();

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


        foreach (DummySpearEnemy spearEnemy in exploders)
        {
            spearEnemy.Explode();
        }
        EffectController.instance.StartCoroutine(EffectController.instance.InstantScreenShake(0.5f, 15, 200, true));

        yield return new WaitForSeconds(2);
        carAnimator.SetBool("Drive", true);

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

        yield return null;
    }

    public void MakeLogoBloody()
    {
        titleCard.MakeBloody();
    }
}
