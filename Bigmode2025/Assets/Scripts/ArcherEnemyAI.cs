using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ArcherEnemyAI : MonoBehaviour
{
    GameObject player;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject spriteObject;
    //[SerializeField] GameObject damageHitbox;
    [SerializeField] GameObject shakeHolder;
    //[SerializeField] GameObject arrow;
    [SerializeField] GhostTrail trail;

    Rigidbody2D rb;

    [Header("Attacking and Movement")]
    [SerializeField] float targetStandDistance = 5;
    [SerializeField] float targetAvoidDistance = 10;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float fireDistance = 8;
    [SerializeField] float fireWaitTime = 0.5f;
    [SerializeField] float arrowTravelTime = 0.3f;
    [SerializeField] float fireCooldown = 1f;
    //[SerializeField] float standStillAfterFireTime = 0.2f;
    [SerializeField] Transform arrowInstantiatePosition;
    [SerializeField] GameObject arrowPrefab;
    GameObject currentArrow;

    public Coroutine shooting;
    public bool shootingBool = false;

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

                    //arrow.transform.DOLocalMoveX(1.2f, 0.5f).SetEase(Ease.OutQuad);
                }
            }
            else
            {
                if (_renderer.flipX)
                {
                    _renderer.flipX = false;

                    //arrow.transform.DOLocalMoveX(-1.2f, 0.5f).SetEase(Ease.OutQuad);
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
            canLunge = false;
            if (timer <= 0)
            {
                float moveHorizontal = player.transform.position.x - transform.position.x;
                float moveVertical = player.transform.position.y - transform.position.y;

                rb.linearVelocity = new Vector3(moveHorizontal, moveVertical, 0).normalized * moveSpeed;
            }
        }
        else if (Vector3.Distance(player.transform.position, transform.position) < targetAvoidDistance)
        {
            canLunge = false;
            if (timer <= 0)
            {
                float moveHorizontal = player.transform.position.x - transform.position.x;
                float moveVertical = player.transform.position.y - transform.position.y;

                rb.linearVelocity = new Vector3(-moveHorizontal, -moveVertical, 0).normalized * moveSpeed;
            }
        }
        else
        {
            canLunge = true;
            rb.linearVelocity = Vector3.zero;
            timer = walkingCooldown;

        }
    }

    public IEnumerator ShootAttack()
    {
            chargingLunge = true;
            canLunge = false;

            rb.linearVelocity = Vector3.zero;

            canBeTerrified = false;
            CancelTerrifiedState();
            _renderer.sprite = _preChargeFace;

            currentArrow = Instantiate(arrowPrefab, arrowInstantiatePosition.position, arrowInstantiatePosition.rotation);
            currentArrow.GetComponent<ArcherDamageHitbox>().healthScript = GetComponent<EnemyHealthScript>();
            currentArrow.GetComponent<ArcherDamageHitbox>().archerEnemyAIScript = GetComponent<ArcherEnemyAI>();

            //Point arrow towards player on spawn with 360 degree spin
            Vector3 predictedPosition = (player.transform.position);
            Vector3 targetPosition = ((predictedPosition - transform.position).normalized * fireDistance) + transform.position;

            var dir = (targetPosition - transform.position).normalized;
            Quaternion lookRot = Quaternion.LookRotation(Vector3.forward, dir);
            currentArrow.transform.DOLocalRotate(new Vector3(lookRot.eulerAngles.x, lookRot.eulerAngles.y, lookRot.eulerAngles.z + 360.0f), fireWaitTime * 0.7f, RotateMode.FastBeyond360);

            //Wait until 75% of the charge time has passed
            yield return new WaitForSeconds(fireWaitTime * 0.9f);

            //Actually point the aim direction that will be fired
/*            predictedPosition = (player.transform.position);
            targetPosition = ((predictedPosition - transform.position).normalized * fireDistance) + transform.position;*/

            dir = (targetPosition - transform.position).normalized;
            lookRot = Quaternion.LookRotation(Vector3.forward, dir);
            currentArrow.transform.DOLocalRotate(new Vector3(lookRot.eulerAngles.x, lookRot.eulerAngles.y, lookRot.eulerAngles.z), fireWaitTime * 0.1f, RotateMode.Fast);

            //finish charging the attack
            yield return new WaitForSeconds(fireWaitTime * 0.1f);

            //Actually attack

            chargingLunge = false;
            lunging = true;
            canBeTerrified = true;

            _renderer.sprite = _chargeFace;
            //damageHitbox.SetActive(true);

            //healthScript.InvincibilityFrames(arrowTravelTime);

            //Lunge lerp
            float elapsed = 0.0f;

            Vector3 startPos = currentArrow.transform.position;
            if (currentArrow != null)
            {
                while (elapsed < arrowTravelTime)
                {
                    currentArrow.transform.position = Vector3.Lerp(startPos, targetPosition, elapsed / arrowTravelTime);

                    elapsed += Time.deltaTime;
                    yield return null;
                }
            }

        shootingBool = false;
        StartCoroutine(LungeEnding());
        yield return null;
    }

    IEnumerator LungeEnding()
    {
        if (shootingBool == false)
        {
            Destroy(currentArrow);
            //damageHitbox.SetActive(false);

            shooting = null;

            lunging = false;

            //yield return new WaitForSeconds(standStillAfterFireTime);

            canBeTerrified = true;

            //waiting before enemy can lunge again
            StartCoroutine(FireCooldown());
        }

        //back to normal
        yield return null;
    }

    IEnumerator FireCooldown()
    {
        yield return new WaitForSeconds(fireCooldown);

        canLunge = true;
    }

    public void StopLunge()
    {
        lunging = false;
        shootingBool = false;

        StopAllCoroutines();

        //damageHitbox.SetActive(false);
        Destroy(currentArrow);

        shooting = null;

        lunging = false;

        //yield return new WaitForSeconds(standStillAfterFireTime);

        canBeTerrified = true;

        StartCoroutine(FireCooldown());

    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
