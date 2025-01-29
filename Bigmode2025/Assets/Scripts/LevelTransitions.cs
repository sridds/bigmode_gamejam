using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitions : MonoBehaviour
{
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
        StartCoroutine(StartWipe(false));
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
