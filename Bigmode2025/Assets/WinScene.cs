using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class WinScene : MonoBehaviour
{
    [SerializeField]
    private GameObject panel1;

    [SerializeField]
    private GameObject imageInPanel1;

    [SerializeField]
    private AudioClip slamSound;

    [SerializeField]
    private AudioClip textAppear;

    [SerializeField]
    private Image[] words;

    private void Start()
    {
        panel1.SetActive(false);

        StartCoroutine(ShowPanel());

        Time.timeScale = 1.0f;
    }


    private IEnumerator ShowPanel()
    {
        yield return new WaitForSeconds(1.0f);

        panel1.transform.localScale = new Vector3(1.5f, 1.5f, 3);
        panel1.SetActive(true);
        AudioManager.instance.PlaySound(slamSound, 1.0f, 0.95f, 1.1f);

        yield return panel1.transform.DOScale(1.0f, 0.2f).SetEase(Ease.Linear).WaitForCompletion();
        imageInPanel1.transform.DOMoveY(imageInPanel1.transform.position.y + 5.0f, 4.0f, false).SetEase(Ease.Linear);
        yield return panel1.transform.DOShakePosition(0.4f, 6.0f, 40).WaitForCompletion();

        yield return new WaitForSeconds(0.5f);

        foreach (Image i in words)
        {
            i.transform.position = new Vector3(i.transform.position.x, i.transform.position.y - 2);
            i.transform.DOMoveY(i.transform.position.y + 2, 0.4f, false);
            i.DOFade(1.0f, 0.4f);

            AudioManager.instance.PlaySound(textAppear, 1.0f, 1.0f, 1.0f);

            yield return new WaitForSeconds(3.0f);
            AudioManager.instance.PlaySound(textAppear, 1.0f, 1.0f, 1.0f);
            i.transform.DOMoveY(i.transform.position.y - 2, 0.4f, false);
            i.DOFade(0.0f, 0.4f);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
