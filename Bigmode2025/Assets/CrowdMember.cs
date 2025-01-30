using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class CrowdMember : MonoBehaviour
{
    public enum ECrowdMemberState
    {
        Bored,
        Happy,
        Shocked,
        Yell
    }

    private ECrowdMemberState crowdState;

    [SerializeField]
    private Sprite shockedSprite;

    [SerializeField]
    private Sprite happySprite;

    [SerializeField]
    private Sprite boredSprite;

    [SerializeField]
    private Sprite yellSprite;

    [SerializeField]
    private Sprite blinkSprite;

    [SerializeField]
    private SpriteRenderer renderer;

    [SerializeField]
    private float sinAmplitude = 0.6f;

    [SerializeField]
    private float sinSpeed = 1.0f;

    [SerializeField]
    private bool flipTowardDirectionOfCar = true;

    PlayerMovement movement;

    private float sinTimer;
    private float blinkTimer;
    private float blinkRandomTime;

    private bool blinking;

    private void Start()
    {
        sinTimer += Random.Range(0f, 100f);

        blinkTimer = 0.0f;
        blinking = false;
        blinkRandomTime = Random.Range(4, 12);

        movement = FindObjectOfType<PlayerMovement>();
        FindObjectOfType<PlayerHealthScript>().OnDamageTaken += Yell;
    }

    private void Update()
    {
        renderer.transform.localPosition = new Vector3(renderer.transform.localPosition.x, Mathf.Sin((Time.time + sinTimer) * sinSpeed) * sinAmplitude);

        if (crowdState != ECrowdMemberState.Bored || crowdState != ECrowdMemberState.Happy) return;

        blinkTimer += Time.deltaTime;

        // occassionaly blink every couple seconds
        if (blinkTimer > blinkRandomTime)
        {
            blinkTimer = 0.0f;
            blinkRandomTime = Random.Range(4, 12);
            renderer.sprite = blinkSprite;
            blinking = true;
        }

        // blink
        if(blinking && blinkTimer > 0.25f)
        {
            blinking = false;
        }

        if (!blinking)
        {
            renderer.sprite = crowdState == ECrowdMemberState.Bored ? boredSprite : happySprite;
        }

        // flip towards car
        if(flipTowardDirectionOfCar)
        {
            if(movement.transform.position.x > transform.position.x)
            {
                renderer.flipX = true;
            }
            else
            {
                renderer.flipX = false;
            }
        }
    }

    public void EnterHappyState()
    {
        crowdState = ECrowdMemberState.Happy;
        blinking = false;
        renderer.sprite = happySprite;
    }

    public void EnterBoredState()
    {
        crowdState = ECrowdMemberState.Bored;
        blinking = false;
        renderer.sprite = boredSprite;
    }

    public void Yell()
    {
        if (crowdState != ECrowdMemberState.Bored && crowdState != ECrowdMemberState.Happy) return;
        // only some yell
        if (Random.Range(0, 6) == 3) return;

        Debug.Log("fuck");

        StartCoroutine(IYell());
    }

    private IEnumerator IYell()
    {
        renderer.sprite = yellSprite;
        crowdState = ECrowdMemberState.Yell;

        float randomTime = Random.Range(0.7f, 1.5f);
        yield return new WaitForSeconds(Random.Range(0.0f, 0.2f));
        transform.DOKill(true);
        transform.DOShakePosition(0.5f, 0.25f, 25, 90, false, true, ShakeRandomnessMode.Full);
        transform.DOShakeRotation(0.5f, new Vector3(0, 0, 15), 25, 90, true, ShakeRandomnessMode.Full);
        yield return new WaitForSeconds(randomTime);

        EnterBoredState();
    }

    public void DelayedShock(float delay)
    {
        if (crowdState != ECrowdMemberState.Bored && crowdState != ECrowdMemberState.Happy) return;

        StartCoroutine(IShock(delay));
    }

    private IEnumerator IShock(float delay)
    {
        yield return new WaitForSeconds(delay);
        crowdState = ECrowdMemberState.Shocked;
        renderer.sprite = shockedSprite;

        transform.DOKill(true);
        transform.DOJump(transform.position, 2.0f, 1, 0.3f).SetEase(Ease.OutQuad);
        transform.DOShakeRotation(0.4f, new Vector3(0, 0, 30), 35, 90, true, ShakeRandomnessMode.Full);

        yield return new WaitForSeconds(0.7f);
        EnterBoredState();
    }
}
