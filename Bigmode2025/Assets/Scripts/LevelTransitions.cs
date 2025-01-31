using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitions : MonoBehaviour
{
    [SerializeField]
    private GameObject levelText;

    [SerializeField]
    private GameObject completeText;

    [SerializeField]
    private AudioClip slamSound;

    [SerializeField]
    private SpriteRenderer zoomer;

    [SerializeField] GameObject cutsceneObjectsHolder;
    [SerializeField] GameObject playerIcon;
    [SerializeField] float iconMoveTime;
    [SerializeField] GameObject nextLevelGameObject;
    [SerializeField] GameObject prevLevelGameObject;

    [SerializeField] GameObject blackWipe;

    [SerializeField] int nextLevelBuildIndex;

    private void Start()
    {
        StartCoroutine(EndWipe(true, false, 1));
    }

    public void StartTransition()
    {
        //StartCoroutine(StartWipe(false));
        StartCoroutine(LevelEnd());
    }

    public IEnumerator DeathAnimation(float wipeHalfTime, float waitTime)
    {
        TimescaleManager.instance.Slow();
        FindObjectOfType<CinematicBarController>().Focus(250, 0.5f, Ease.OutQuad, 5);

        yield return new WaitForSecondsRealtime(waitTime);

        StartCoroutine(StartWipe(false, true, 0.2f));

        yield return null;
    }

    IEnumerator LevelEnd()
    {
        FindObjectOfType<CinematicBarController>().Focus(250, 0.5f, Ease.OutQuad, 5);

        float elapsed = 0.0f;
        float duration = 0.6f;

        zoomer.material.DOFloat(0.2f, "_Zoom", 0.6f).SetEase(Ease.OutQuad).SetUpdate(UpdateType.Normal, true);
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(1.0f, 0.2f, elapsed / duration);

            yield return null;
        }

        yield return new WaitForSecondsRealtime(1.0f);
        zoomer.material.DOFloat(0.0f, "_Zoom", 0.3f).SetEase(Ease.OutQuad).SetUpdate(UpdateType.Normal, true);

        float slamDuration = 0.2f;

        Vector3 previousScale = levelText.transform.localScale;
        levelText.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);
        levelText.SetActive(true);
        levelText.transform.DOScale(previousScale, slamDuration).SetEase(Ease.Linear).SetUpdate(UpdateType.Normal, true);
        yield return new WaitForSecondsRealtime(slamDuration);
        levelText.transform.DOShakePosition(0.5f, 10f, 45, 90, false, true, ShakeRandomnessMode.Full).SetUpdate(UpdateType.Normal, true);
        AudioManager.instance.PlaySound(slamSound, 1.0f, 1.0f, 1.0f);

        yield return new WaitForSecondsRealtime(0.5f);

        previousScale = completeText.transform.localScale;
        completeText.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);
        completeText.SetActive(true);
        completeText.transform.DOScale(previousScale, slamDuration).SetEase(Ease.Linear).SetUpdate(UpdateType.Normal, true);
        yield return new WaitForSecondsRealtime(slamDuration);
        completeText.transform.DOShakePosition(0.5f, 10f, 45, 90, false, true, ShakeRandomnessMode.Full).SetUpdate(UpdateType.Normal, true);
        EffectController.instance.InstantScreenShake(0.5f, 15.0f, 30.0f, true);
        AudioManager.instance.PlaySound(slamSound, 1.0f, 1.0f, 1.0f);

        yield return null;
    }

    IEnumerator StartWipe(bool transition, bool dead, float wipeHalfTime)
    {
        float elapsed = 0.0f;

        Vector3 startPos = new Vector3(480, 0, 0);

        while (elapsed < wipeHalfTime)
        {
            blackWipe.transform.localPosition = Vector3.Lerp(startPos, new Vector3(0, 0, 0), elapsed / wipeHalfTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (transition)
        {
            cutsceneObjectsHolder.SetActive(false);
            SceneManager.LoadScene(nextLevelBuildIndex);
        }
        else if (!dead)
        {
            cutsceneObjectsHolder.SetActive(true);
            GameObject.FindGameObjectWithTag("Player").SetActive(false);
        }
        else if (dead)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        StartCoroutine(EndWipe(false, dead, wipeHalfTime));


        yield return null;
    }

    IEnumerator LevelTransitionCutscene(float wipeHalfTime)
    {

        float elapsed = 0.0f;

        Vector3 startPos = new Vector3(playerIcon.transform.position.x, prevLevelGameObject.transform.position.y, playerIcon.transform.position.z);

        while (elapsed < iconMoveTime)
        {
            playerIcon.transform.position = Vector3.Lerp(startPos, new Vector3(startPos.x, nextLevelGameObject.transform.position.y, startPos.z), elapsed / iconMoveTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(StartWipe(true, false, wipeHalfTime));
        yield return null;
    }

    IEnumerator EndWipe(bool startOfLevel, bool dead, float wipeHalfTime)
    {
        float elapsed = 0.0f;

        Vector3 startPos = new Vector3(0, 0, 0);

        while (elapsed < wipeHalfTime)
        {
            blackWipe.transform.localPosition = Vector3.Lerp(startPos, new Vector3(-480, 0, 0), elapsed / wipeHalfTime);

            elapsed += Time.deltaTime;
            yield return null;
        }
        if (!startOfLevel && !dead)
        {
            StartCoroutine(LevelTransitionCutscene(wipeHalfTime));
        }
        yield return null;
    }
}
