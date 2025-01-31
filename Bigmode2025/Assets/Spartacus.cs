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
    public AudioClip[] deathSounds;

    private float timer;

    private void Start()
    {
        AudioManager.instance.FadeOutStageTheme(1.0f);
    }

    void Update()
    {
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
        spartacusName.gameObject.SetActive(false);
        bloodToEnable.SetActive(true);
        headSprite.SetActive(false);
        EffectController.instance.StartCoroutine(EffectController.instance.FreezeFrame(0.07f));
        EffectController.instance.StartCoroutine(EffectController.instance.InstantScreenShake(1.0f, 25, 200, true));
        Instantiate(headPrefab, headSprite.transform.position, Quaternion.identity);

        foreach (CrowdMember c in FindObjectsByType<CrowdMember>(FindObjectsSortMode.None))
        {
            c.SetPermaShocked();
        }
        AudioManager.instance.PlaySound(deathSounds[Random.Range(0, deathSounds.Length - 1)], 1.0f, 0.95f, 1.1f);

        yield return new WaitForSeconds(5.0f);
        GameObject.Find("LevelTransition").GetComponent<LevelTransitions>().StartTransition(false);
    }

    bool killFlag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (killFlag) return;

        if (collision.TryGetComponent<PlayerMovement>(out PlayerMovement movement))
        {
            KillSpartacus();
            killFlag = true;
        }
    }
}
