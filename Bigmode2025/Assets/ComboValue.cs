using UnityEngine;
using DG.Tweening;

public class ComboValue : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _renderer;
    
    [SerializeField]
    private float shakeAmount = 0.1f;

    [SerializeField]
    private float shakeInterval = 0.05f;

    [SerializeField]
    private float lifeTime = 2.0f;

    float timer;
    Vector2 startingPosition;

    private void Start()
    {
        
        //Destroy(gameObject, lifeTime);
    }

    public void DelayedHop(float delayAmount)
    {
        Invoke(nameof(Hop), delayAmount);
    }

    private void Hop()
    {
        transform.localScale = new Vector3(2.2f, 2.2f, 0.0f);
        transform.DOScale(1.0f, 0.2f);
        transform.DOJump(transform.position, 1.0f, 1, 0.2f);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer > shakeAmount)
        {
            _renderer.transform.localPosition = new Vector2(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount));
            timer = 0.0f;
        }
    }

    public void UpdateSprite(Sprite spr)
    {
        _renderer.sprite = spr;
    }


}
