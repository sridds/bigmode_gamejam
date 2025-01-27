using UnityEngine;
using System.Collections;

public class SpearEnemyAIScript : MonoBehaviour
{
    GameObject player;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject spriteObject;
    [SerializeField] GameObject damageHitbox;

    Rigidbody2D rb;

    [SerializeField] float targetStandDistance = 5;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float lungeDistance = 8;
    [SerializeField] float lungeWaitTime = 0.5f;
    [SerializeField] float lungeTravelTime = 0.3f;
    [SerializeField] float lungeCooldown = 1f;
    [SerializeField] float standStillAfterLungeTime = 0.2f;

    //If the player is right at the edge of the target distance, this prevents weird stuttering movement
    [SerializeField] float walkingCooldown = 0.1f;
    float timer = 0.1f;

    public bool lunging = false;
    public bool canLunge = true;

    EnemyHealthScript healthScript;

    [Header("Faces")]
    [SerializeField]
    private Sprite _neutralFace;

    [SerializeField]
    private Sprite _shockedFace;

    [SerializeField]
    private Sprite _preChargeFace;

    [SerializeField]
    private Sprite _chargeFace;

    [SerializeField]
    private Sprite _worriedFace;

    [SerializeField]
    private Sprite _decapitatedFace;

    [SerializeField]
    private SpriteRenderer _renderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        healthScript = GetComponent<EnemyHealthScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!lunging)
        {
            MoveTowardsPlayer();

            if (Vector3.Distance(player.transform.position, transform.position) < 5.0f)
            {
                _renderer.sprite = _shockedFace;
            }
            else
            {
                _renderer.sprite = _neutralFace;
            }
        }
    }

    void MoveTowardsPlayer()
    {
        timer = timer - Time.deltaTime;

        if (Vector3.Distance(player.transform.position, transform.position) > targetStandDistance)
        {
            if (timer <= 0)
            {
                float moveHorizontal = player.transform.position.x - transform.position.x;
                float moveVertical = player.transform.position.y - transform.position.y;

                rb.linearVelocity = new Vector3(moveHorizontal, moveVertical, 0).normalized * moveSpeed;
            }
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
            timer = walkingCooldown;

        }
    }

    public IEnumerator LungeAttack()
    {
        rb.linearVelocity = Vector3.zero;

        _renderer.sprite = _preChargeFace;
        lunging = true;
        //gearing up for lunge sprite change
        spriteRenderer.color = Color.yellow;
        canLunge = false;

        Vector3 targetPosition = (spriteObject.transform.up * lungeDistance) + transform.position;

        yield return new WaitForSeconds(lungeWaitTime);

        _renderer.sprite = _chargeFace;
        damageHitbox.SetActive(true);

        //while lunging sprite change
        spriteRenderer.color = Color.red;

        healthScript.InvincibilityFrames(lungeTravelTime);

        //Lunge lerp
        float elapsed = 0.0f;

        Vector3 startPos = transform.position;

        while(elapsed < lungeTravelTime)
        {
            transform.position = Vector3.Lerp(startPos, targetPosition, elapsed / lungeTravelTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        //waiting before enemy can lunge again
        StartCoroutine(LungeEnding());

        yield return null;
    }

    IEnumerator LungeEnding()
    {
        //Stagger after lunge
        spriteRenderer.color = Color.green;

        damageHitbox.SetActive(false);

        yield return new WaitForSeconds(standStillAfterLungeTime);

        lunging = false;

        //waiting before enemy can lunge again
        yield return new WaitForSeconds(lungeCooldown);

        canLunge = true;

        //back to normal
        spriteRenderer.color = Color.white;
        yield return null;
    }

    public void StopLunge()
    {
        StopAllCoroutines();
        lunging = false;

        StartCoroutine(LungeEnding());

    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }


}
