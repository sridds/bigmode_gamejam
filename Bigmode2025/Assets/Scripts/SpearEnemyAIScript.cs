using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SpearEnemyAIScript : MonoBehaviour
{
    GameObject player;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject spriteObject;
    [SerializeField] GameObject damageHitbox;
    [SerializeField] GameObject shakeHolder;
    [SerializeField] GameObject spear;
    [SerializeField] GhostTrail trail;

    Rigidbody2D rb;

    [Header("Attacking and Movement")]
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
    public bool isTerrified;

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
    private Sprite _hurtFace;

    [SerializeField]
    private Sprite _decapitatedFace;

    [SerializeField]
    private SpriteRenderer _renderer;

    [SerializeField]
    private Animator _animator;

    [Header("Audio")]
    [SerializeField]
    private AudioClip _lungeSound;

    float terrifiedTimer;
    bool canBeTerrified = true;
    public bool knockedBack = false;
    private AudioSource source;
    bool chargingLunge = false;
    EnemyManager enemyManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        healthScript = GetComponent<EnemyHealthScript>();

        source = GetComponent<AudioSource>();

        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        enemyManager.enemies.Add(gameObject);
        enemyManager.UpdateCount();
    }

    public bool CanApplyKnockback()
    {
        if (lunging) return false;

        return true;
    }

    public void ApplyKnockback()
    {
        Debug.Log("Ending Lunge");
        StopAllCoroutines();
        StartCoroutine(LungeEnding());

        knockedBack = true;
        rb.linearDamping = 3.0f;
        _animator.SetBool("Damaged", true);
        _animator.SetBool("Scared", false);
        chargingLunge = false;

        _renderer.sprite = _hurtFace;
        isTerrified = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < 4.0f && canBeTerrified && !knockedBack)
        {
            isTerrified = true;
            terrifiedTimer = 0.0f;
        }

        if (isTerrified)
        {
            TerrifiedState();
            return;
        }

        if (!lunging && !knockedBack && !chargingLunge)
        {
            MoveTowardsPlayer();

            _animator.SetBool("Walking", true);
            _renderer.sprite = _neutralFace;

            if (player.transform.position.x > transform.position.x)
            {
                if (!_renderer.flipX)
                {
                    _renderer.flipX = true;

                    spear.transform.DOLocalMoveX(1.2f, 0.5f).SetEase(Ease.OutQuad);
                }
            }
            else
            {
                if (_renderer.flipX)
                {
                    _renderer.flipX = false;

                    spear.transform.DOLocalMoveX(-1.2f, 0.5f).SetEase(Ease.OutQuad);
                }
            }
        }

        if (knockedBack && rb.linearVelocity.magnitude <= 1.0f)
        {
            knockedBack = false;
            rb.linearDamping = 0.0f;
            _animator.SetBool("Damaged", false);
        }
    }

    void TerrifiedState()
    {
        rb.linearVelocity = Vector2.zero;
        terrifiedTimer += Time.deltaTime;
        _animator.SetBool("Walking", false);
        _animator.SetBool("Scared", true);
        _renderer.sprite = _shockedFace;

        if (terrifiedTimer >= 0.5f)
        {
            CancelTerrifiedState();
        }
    }

    void CancelTerrifiedState()
    {
        isTerrified = false;
        _animator.SetBool("Scared", false);
        _renderer.sprite = _neutralFace;
        terrifiedTimer = 0.0f;
    }

    void MoveTowardsPlayer()
    {
        if (knockedBack) return;
             
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
        chargingLunge = true;
        rb.linearVelocity = Vector3.zero;

        canBeTerrified = false;
        CancelTerrifiedState();
        _renderer.sprite = _preChargeFace;
        //gearing up for lunge sprite change
        spriteRenderer.color = Color.yellow;
        canLunge = false;

        
        Vector3 playerVelocity = player.GetComponent<Rigidbody2D>().linearVelocity * (lungeWaitTime + lungeTravelTime);
        Vector3 predictedPosition = (player.transform.position);
        Vector3 targetPosition = ((predictedPosition - transform.position).normalized * lungeDistance) + transform.position;

        shakeHolder.transform.DOShakePosition(0.4f, new Vector3(0.5f, 0.0f), 25, 90, false, true, ShakeRandomnessMode.Full);

        // calculate spear rotation
        var dir = (targetPosition - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(Vector3.forward, dir);
        spear.transform.DOLocalRotate(new Vector3(lookRot.eulerAngles.x, lookRot.eulerAngles.y, lookRot.eulerAngles.z + 360.0f), lungeWaitTime, RotateMode.FastBeyond360);

        yield return new WaitForSeconds(lungeWaitTime / 2);
        targetPosition = ((player.transform.position - transform.position).normalized * lungeDistance) + transform.position;
        yield return new WaitForSeconds(lungeWaitTime / 2);

        // calculate spear rotation
        dir = (targetPosition - transform.position).normalized;
        lookRot = Quaternion.LookRotation(Vector3.forward, dir);
        spear.transform.DOLocalRotate(new Vector3(lookRot.eulerAngles.x, lookRot.eulerAngles.y, lookRot.eulerAngles.z + 360.0f), lungeWaitTime, RotateMode.FastBeyond360);
        source.pitch = Random.Range(0.95f, 1.1f);

        source.PlayOneShot(_lungeSound);

        lunging = true;
        chargingLunge = false;

        _renderer.sprite = _chargeFace;
        damageHitbox.SetActive(true);

        //while lunging sprite change
        spriteRenderer.color = Color.red;

        healthScript.InvincibilityFrames(lungeTravelTime);

        //Lunge lerp
        float elapsed = 0.0f;

        Vector3 startPos = transform.position;

        trail.SetGhostTrailEnabled(true);

        while (elapsed < lungeTravelTime)
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

        trail.SetGhostTrailEnabled(false);
        yield return new WaitForSeconds(standStillAfterLungeTime);

        spear.transform.DOLocalRotateQuaternion(Quaternion.identity, 0.5f);
        lunging = false;
        canBeTerrified = true;

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
