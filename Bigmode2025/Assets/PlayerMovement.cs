using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float decceleration;

    [SerializeField] float turnSharpness;
    [SerializeField] float driftTurnSharpness;
    [SerializeField] float driftTurnDelta; //Amount that inputting directions while drifting effects drift tightness
    [SerializeField] float driftFriction; //0-1

    [SerializeField] float drift1Time;
    [SerializeField] float drift2Time;
    [SerializeField] float drift3Time;
    [SerializeField] float drift1Speed;
    [SerializeField] float drift2Speed;
    [SerializeField] float drift3Speed;

    [SerializeField] float driftRotationSpeed; //How fast the car turns 90 degrees at the start of a drift

    [SerializeField] float autoAimMinAngle;

    public float MaxSpeed { get { return maxSpeed; } }
    public float CurrentSpeed { get { return currentSpeed; } }

    float driftBoost;
    float driftBoostTimer;
    [Header("Refrences")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform sprite;

    [Header("Juice Refrences")]
    [SerializeField] GhostTrail ghostTrailBoost;
    [SerializeField] Transform speedBoostFire;
    [SerializeField] ParticleSystem[] boostParticleSystem;
    [SerializeField] float treadJitterAmount = 0.1f;
    [SerializeField] TrailRenderer treadLeft;
    [SerializeField] TrailRenderer treadRight;
    [SerializeField] float collisionShakeMax;
    [SerializeField] float collisionShakeMultiplier;
    [SerializeField] int collisionShakeVibrado;
    [SerializeField] float velocityShakeAmplitude;
    [SerializeField] float velocityShakeFrequency;
    [SerializeField] ParticleSystem drift1Particle;
    [SerializeField] ParticleSystem drift2Particle;
    [SerializeField] ParticleSystem drift3Particle;
    [SerializeField] LayerMask crowdLayer;

    [Header("Sound")]
    [SerializeField] AudioSource carMotor;
    [SerializeField] AudioClip motorLoop;
    [SerializeField] AudioClip driftLoop;
    [SerializeField] AudioClip gearOne;
    [SerializeField] AudioClip gearTwo;
    [SerializeField] AudioClip driftRelease;
    [SerializeField] float pitchMin = 0.9f;
    [SerializeField] float pitchMax = 1.3f;

    float steeringInput = 0;
    float steeringFactor = 0;
    float driftDirection = 0; //gets set to steeringInput when drift starts

    public float rotationAngle = 0;
    float visualRotationAngle = 0;

    public Vector2 carDirection;
    bool isDrifting = false;
    bool driftQueued = false;
    [HideInInspector] public float animationAngle;
    [HideInInspector] public float currentSpeed;

    public Vector2 Velocity { get { return rb.linearVelocity; } }
    public bool IsDrifting { get { return isDrifting; } }

    float additionalBoost;

    Tween collisionShake;

    Transform enemyContainer;

    private void Awake()
    {
        if (GameObject.Find("EnemyContainer") != null)
        {
            enemyContainer = GameObject.Find("EnemyContainer").transform;
        }
    }


    void Update()
    {
        if (GameStateManager.instance.currentState == GameStateManager.PlayerState.Playing)
        {
            Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            SetInputVector(inputVector);
            currentSpeed = rb.linearVelocity.magnitude;
            MovementScreenShake();

            float scaled = currentSpeed / maxSpeed + PlayerUpgrades.instance.maxSpeed;
            float pitch = (scaled - pitchMin) / (pitchMax - pitchMin);

            if (isDrifting && carMotor.clip != driftLoop)
            {
                carMotor.clip = driftLoop;
                carMotor.Play();
            }
            if (!isDrifting && carMotor.clip != motorLoop)
            {
                carMotor.clip = motorLoop;
                carMotor.Play();
            }

            if (pitch > pitchMax) pitch = pitchMax;
            else if (pitch < pitchMin) pitch = pitchMin;

            carMotor.pitch = pitch;
        }
    }

    bool hasSet0 = false;
    void MovementScreenShake()
    {
        float speed = rb.linearVelocity.magnitude;
        if (rb.linearVelocity.magnitude > MaxSpeed + 0.2f)
        {
            hasSet0 = false;

            EffectController.instance.ContinousScreenShake(velocityShakeAmplitude * speed, velocityShakeFrequency * speed);
        }
        else
        {
            if (!hasSet0)
            {
                EffectController.instance.ContinousScreenShake(0, 0);
                hasSet0 = true;
            }
        }
    }
    private void LateUpdate()
    {
        animationAngle = Vector2ToDegrees(DegreesToVector2(visualRotationAngle));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplySteering();
        ApplyEngineForce();
        Drifting();
    }
    void ApplyEngineForce()
    {
        Vector2 engineForceVector = carDirection * acceleration;


        if (rb.linearVelocity.magnitude <= MaxSpeed + PlayerUpgrades.instance.maxSpeed)
        {
            rb.linearVelocity += engineForceVector * Time.fixedDeltaTime;
        }
        if (rb.linearVelocity.magnitude > MaxSpeed + PlayerUpgrades.instance.maxSpeed)
        {
            rb.linearVelocity -= decceleration * Time.fixedDeltaTime * engineForceVector;
        }

        Vector3 velocity = rb.linearVelocity;
        if (!isDrifting)
        {
            rb.linearVelocity = carDirection * velocity.magnitude;
        }
        else
        {
            rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, carDirection * velocity.magnitude, driftFriction + PlayerUpgrades.instance.driftFriction);
        }


    }

    bool reachedTier2;
    bool reachedTier3;

    void Drifting()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (isDrifting)
            {
                driftBoostTimer += Time.deltaTime;
                if (driftBoostTimer > drift3Time)
                {
                    if (!reachedTier3)
                    {
                        AudioManager.instance.PlaySound(gearTwo, 0.7f, 0.95f, 1.1f);
                        reachedTier3 = true;
                    }

                    driftBoost = drift3Speed * PlayerUpgrades.instance.boostMultiplier;
                    drift3Particle.Play();
                    drift2Particle.Stop();

                }
                else if (driftBoostTimer > drift2Time)
                {
                    if (!reachedTier2)
                    {
                        AudioManager.instance.PlaySound(gearOne, 0.7f, 0.95f, 1.1f);
                        reachedTier2 = true;
                    }

                    driftBoost = drift2Speed * PlayerUpgrades.instance.boostMultiplier;
                    drift2Particle.Play();
                    drift1Particle.Stop();

                }
                else if (driftBoostTimer > drift1Time)
                {
                    driftBoost = drift1Speed * PlayerUpgrades.instance.boostMultiplier;
                    drift1Particle.Play();
                }
            }
            else
            {
                driftQueued = true;

            }

        }
        else if (isDrifting) //Happens one time once drifting ends
        {
            if (reachedTier2 || reachedTier3)
            {
                AudioManager.instance.PlaySound(driftRelease, 0.7f, 0.95f, 1.1f);
            }
            reachedTier2 = false;
            reachedTier3 = false;

            //Put ending drift juice here.
            drift1Particle.Stop();
            drift2Particle.Stop();
            drift3Particle.Stop();
            ghostTrailBoost.SetGhostTrailEnabled(true);
            Invoke(nameof(StopEmittingGhost), 1.0f);

            Transform closestEnemy = null;
            float smallestAngle = 0;

            if (enemyContainer != null)
            {
                foreach (Transform enemy in enemyContainer)
                {
                    //Vector2 angle = (transform.position - enemy.position).normalized;
                    //float floatAngle = Vector2ToDegrees(angle);
                    float dotProduce = Vector2.Dot(DegreesToVector2(visualRotationAngle - 90), (transform.position - enemy.transform.position).normalized);
                    if (dotProduce > 0 && dotProduce > autoAimMinAngle)
                    {
                        float maxDistance = 0;
                        if (driftBoost == drift3Speed * PlayerUpgrades.instance.boostMultiplier)
                        {
                            maxDistance = 35 * PlayerUpgrades.instance.boostMultiplier;
                        }
                        else if (driftBoost == drift2Speed * PlayerUpgrades.instance.boostMultiplier)
                        {
                            maxDistance = 22 * PlayerUpgrades.instance.boostMultiplier;

                        }
                        else if (driftBoost == drift1Speed * PlayerUpgrades.instance.boostMultiplier)
                        {
                            maxDistance = 16 * PlayerUpgrades.instance.boostMultiplier;
                        }

                        if (dotProduce > smallestAngle && Mathf.Abs((enemy.transform.position - transform.position).magnitude) < maxDistance)
                        {
                            closestEnemy = enemy;
                            smallestAngle = dotProduce;
                        }
                    }


                }
            }


            if (closestEnemy != null)
            {
                visualRotationAngle = Vector2ToDegrees((transform.position - closestEnemy.position).normalized) + 90;
            }

            rotationAngle = visualRotationAngle;



            rb.AddForce(carDirection * driftBoost, ForceMode2D.Impulse);
            driftBoost = 0;
            driftBoostTimer = 0;
            isDrifting = false;
        }
        else
        {
            driftQueued = false;
        }

        if (driftQueued && !isDrifting && steeringFactor != 0)
        {
            driftQueued = false;
            driftDirection = steeringFactor;

            //Juice on drift start
            StartCoroutine(StartVisualDriftRotation());


            EmitTreads(false);
            sprite.transform.DOComplete();
            sprite.transform.localPosition = Vector3.zero;
            sprite.transform.DOLocalJump(sprite.transform.localPosition, 0.8f, 1, driftRotationSpeed * 1.5f).OnComplete(() => EmitTreads(true));

            isDrifting = true;
        }

    }

    void StopEmittingGhost()
    {
        ghostTrailBoost.SetGhostTrailEnabled(false);
    }

    void EmitTreads(bool emitting)
    {
        treadLeft.emitting = emitting;
        treadRight.emitting = emitting;
    }
    void ApplySteering()
    {
        if (isDrifting)
        {
            steeringFactor = (steeringInput / driftTurnDelta * (PlayerUpgrades.instance.driftTurnDelta + 1)) + driftDirection;
            rotationAngle -= steeringFactor * driftTurnSharpness * rb.linearVelocity.magnitude;
            visualRotationAngle -= steeringFactor * driftTurnSharpness * rb.linearVelocity.magnitude;
            foreach (ParticleSystem p in boostParticleSystem)
            {
                if (p.isEmitting)
                {
                    p.Stop();
                }
            }
        }
        else if (rb.linearVelocity.magnitude > MaxSpeed + PlayerUpgrades.instance.maxSpeed) //if dashing
        {
            steeringFactor = steeringInput;
            rotationAngle -= steeringFactor * turnSharpness * rb.linearVelocity.magnitude / (rb.linearVelocity.magnitude - MaxSpeed + PlayerUpgrades.instance.maxSpeed + 1);
            visualRotationAngle = rotationAngle;

            foreach (ParticleSystem p in boostParticleSystem)
            {
                if (!p.isEmitting)
                {
                    p.Play();
                }
            }



        }
        else
        {
            steeringFactor = steeringInput;
            rotationAngle -= steeringFactor * turnSharpness * rb.linearVelocity.magnitude;
            visualRotationAngle = rotationAngle;

            foreach (ParticleSystem p in boostParticleSystem)
            {
                if (p.isEmitting)
                {
                    p.Stop();
                }
            }
        }

        carDirection = DegreesToVector2(rotationAngle + 90);

        speedBoostFire.localEulerAngles = new Vector3(0,0,visualRotationAngle);
    }
    IEnumerator StartVisualDriftRotation()
    {
        float elapsed = 0;
        float storedDirection = driftDirection;
        while (elapsed < driftRotationSpeed)
        {
            visualRotationAngle -= 60 * storedDirection * Time.deltaTime / driftRotationSpeed;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public delegate void HitWall(Vector2 pos);
    public HitWall OnHitWall;

    //Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            rotationAngle = Vector2ToDegrees(Vector2.Reflect(DegreesToVector2(rotationAngle), DegreesToVector2(Vector2ToDegrees(collision.GetContact(0).normal) - 90)));

            //Shake the car on collision
            sprite.transform.DOComplete();

            sprite.transform.localPosition = Vector3.zero;
            collisionShake.Complete();
            float shakeAmount = Mathf.Clamp(collisionShakeMultiplier * collision.GetContact(0).normalImpulse, 0, collisionShakeMax);
            collisionShake = sprite.transform.DOShakePosition(0.65f, shakeAmount, collisionShakeVibrado);

            //OnHitWall?.Invoke(transform.position);
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 5.0f, Vector2.up, 0.0f, crowdLayer);
            hits = hits.OrderBy((x) => Vector2.Distance(transform.position, x.transform.position)).ToArray();

            int index = 0;
            foreach(RaycastHit2D hit in hits)
            {
                Debug.Log("Hi");
                hit.transform.TryGetComponent<CrowdMember>(out CrowdMember member);
                member.DelayedShock(index * 0.025f);

                index++;
            }
        }

    }

    //Helper Functions
    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
    }
    Vector2 DegreesToVector2(float degrees)
    {
        Vector2 vectorAngle;
        float radians = degrees * Mathf.Deg2Rad; // Convert degrees to radians
        vectorAngle = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;
        return vectorAngle;
    }

    float Vector2ToDegrees(Vector2 vector)
    {
        float returnDegrees = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        if (returnDegrees < 0)
        {
            return returnDegrees + 360;
        }
        else
        {
            return returnDegrees;
        }
    }


}
