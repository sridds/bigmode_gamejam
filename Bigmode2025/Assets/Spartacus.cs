using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class Spartacus : MonoBehaviour
{
    public SpriteRenderer spartacusName;
    public float spartacusBlinkInterval;
    public GameObject headPrefab;
    public GameObject headSprite;
    public GameObject bloodToEnable;
    public GameObject superSmallBlood;
    public AudioClip[] deathSounds;
    public AudioClip bloodSpurtSound;
    public AudioClip bloodSmallSpurtSound;
    public AudioSource music;
    public GameObject[] leftCrowdToMove;
    public GameObject[] rightCrowdToMove;

    private float timer;
    private bool canBlink;
    private bool canHit;

    private void Start()
    {
        canBlink = false;
        StartCoroutine(ISpartacusAppear());
    }

    public IEnumerator ISpartacusAppear()
    {
        AudioManager.instance.FadeOutStageTheme(1.0f);

        yield return new WaitForSeconds(5.0f);
        GameStateManager.instance.UpdateState(GameStateManager.PlayerState.Cutscene);

        FindObjectOfType<PlayerMovement>(true).gameObject.SetActive(false);
        FindObjectOfType<PlayerMovement>(true).transform.position = Vector3.zero;


        music.gameObject.SetActive(true);

        FindObjectOfType<CameraTargetController>().SetFocus(transform, 1.0f, 0.0f);
        FindObjectOfType<CinematicBarController>().Focus(250, 0.5f, Ease.OutQuad, 0);

        transform.DOMoveY(transform.position.y - 18, 4.0f, false).SetEase(Ease.Linear);

        yield return new WaitForSeconds(1.2f);
        StartCoroutine(ICrowdMove(rightCrowdToMove, 1.5f));
        StartCoroutine(ICrowdMove(leftCrowdToMove, -1.5f));
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(ICrowdMove(rightCrowdToMove, -1.5f));
        StartCoroutine(ICrowdMove(leftCrowdToMove, 1.5f));


        canBlink = true;
        yield return new WaitForSeconds(4.0f);

        FindObjectOfType<PlayerMovement>(true).gameObject.SetActive(true);
        FindObjectOfType<PlayerMovement>(true).transform.position = Vector3.zero;

        FindObjectOfType<CameraTargetController>().SetToDefault();
        FindObjectOfType<CinematicBarController>().Focus(460, 0.5f, Ease.OutQuad, 0);
        GameStateManager.instance.UpdateState(GameStateManager.PlayerState.Playing);

       


        canHit = true;
    }

    public IEnumerator ICrowdMove(GameObject[] crowd, float offset)
    {
        foreach (GameObject g in crowd)
        {
            g.transform.DOMoveX(g.transform.position.x + offset, 0.7f);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
        }
    }

    void Update()
    {
        if (!canBlink) return;

        timer += Time.deltaTime;

        if(timer > spartacusBlinkInterval)
        {
            spartacusName.enabled = !spartacusName.enabled;
            timer = 0.0f;
        }
    }

    public void KillSpartacus()
    {
        Debug.Log("kill spartacus.");
        StartCoroutine(IKillSpartacus());
    }

    public IEnumerator IKillSpartacus()
    {
        music.gameObject.SetActive(false);
        spartacusName.gameObject.SetActive(false);
        bloodToEnable.SetActive(true);
        headSprite.SetActive(false);
        //FindObjectOfType<CameraTargetController>().SetFocus(transform, 1.0f, 0.0f);
        EffectController.instance.StartCoroutine(EffectController.instance.FreezeFrame(0.07f));
        EffectController.instance.StartCoroutine(EffectController.instance.InstantScreenShake(1.0f, 25, 200, true));
        Instantiate(headPrefab, headSprite.transform.position, Quaternion.identity);

        foreach (CrowdMember c in FindObjectsByType<CrowdMember>(FindObjectsSortMode.None))
        {
            c.SetPermaShocked();
        }
        AudioManager.instance.PlaySound(deathSounds[Random.Range(0, deathSounds.Length - 1)], 1.0f, 0.95f, 1.1f);
        AudioManager.instance.PlaySound(bloodSpurtSound, 0.5f, 1.0f, 1.0f);

        yield return new WaitForSeconds(8.0f);

        AudioManager.instance.PlaySound(bloodSmallSpurtSound, 1.0f, 1.0f, 1.0f);
        superSmallBlood.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        GameObject.Find("LevelTransition").GetComponent<LevelTransitions>().StartTransition(false);
    }

    bool killFlag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canHit) return;
        if (killFlag) return;

        if (collision.TryGetComponent<PlayerMovement>(out PlayerMovement movement))
        {
            KillSpartacus();
            killFlag = true;
        }
    }
}
