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

    [SerializeField] GameObject cutsceneObjectsHolder;
    [SerializeField] GameObject playerIcon;
    [SerializeField] float iconMoveTime;
    [SerializeField] GameObject nextLevelGameObject;
    [SerializeField] GameObject prevLevelGameObject;

    [SerializeField] GameObject blackWipe;
    [SerializeField] float wipeHalfTime = 1;

    [SerializeField] int nextLevelBuildIndex;

    private void Start()
    {
        StartCoroutine(EndWipe(true));
    }

    public void StartTransition()
    {
        //StartCoroutine(StartWipe(false));
        StartCoroutine(LevelEnd());
    }

    IEnumerator LevelEnd()
    {
        FindObjectOfType<CinematicBarController>().Focus(250, 0.5f, Ease.OutQuad, 5);

        float elapsed = 0.0f;
        float duration = 0.6f;

        while(elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(1.0f, 0.2f, elapsed / duration);

            yield return null;
        }

        yield return new WaitForSecondsRealtime(1.0f);

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

        //FindObjectOfType<CinematicBarController>().Focus(380, 0.5f, Ease.OutQuad, 0);

        yield return null;
    }

    IEnumerator StartWipe(bool transition)
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

        cutsceneObjectsHolder.SetActive(true);
        GameObject.FindGameObjectWithTag("Player").SetActive(false);

        StartCoroutine(EndWipe(false));


        yield return null;
    }

    IEnumerator LevelTransitionCutscene()
    {

        float elapsed = 0.0f;

        Vector3 startPos = new Vector3(playerIcon.transform.position.x, prevLevelGameObject.transform.position.y, playerIcon.transform.position.z);

        while (elapsed < iconMoveTime)
        {
            playerIcon.transform.position = Vector3.Lerp(startPos, new Vector3(startPos.x, nextLevelGameObject.transform.position.y, startPos.z), elapsed / iconMoveTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(StartWipe(true));
        yield return null;
    }

    IEnumerator EndWipe(bool startOfLevel)
    {
        float elapsed = 0.0f;

        Vector3 startPos = new Vector3(0, 0, 0);

        while (elapsed < wipeHalfTime)
        {
            blackWipe.transform.localPosition = Vector3.Lerp(startPos, new Vector3(-480, 0, 0), elapsed / wipeHalfTime);

            elapsed += Time.deltaTime;
            yield return null;
        }
        if (!startOfLevel)
        {
            StartCoroutine(LevelTransitionCutscene());
        }

        yield return null;
    }
}
