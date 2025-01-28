using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.U2D;
using UnityEngine.UIElements;

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

    [SerializeField] float maxAdditionalBoost = 6.0f;
    [SerializeField] float additionalSpeedBoost = 2.0f;

    [SerializeField] float driftRotationSpeed; //How fast the car turns 90 degrees at the start of a drift

    public float MaxSpeed { get { return maxSpeed + additionalBoost; } }

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



    float steeringInput = 0;
    float steeringFactor = 0;
    float driftDirection = 0; //gets set to steeringInput when drift starts

    float rotationAngle = 0;
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


    public void AddSpeedBoost()
    {
        additionalBoost += additionalSpeedBoost;

        if (additionalBoost > maxAdditionalBoost) additionalBoost = additionalSpeedBoost;
    }

    public void CancelSpeedBoost()
    {
        additionalBoost = 0.0f;
    }

    void Update()
    {
        Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        SetInputVector(inputVector);
        currentSpeed = rb.linearVelocity.magnitude;
        MovementScreenShake();
    }
        
    void MovementScreenShake()
    {
        float speed = rb.linearVelocity.magnitude;
        if (rb.linearVelocity.magnitude > MaxSpeed)
        {
            EffectController.instance.ContinousScreenShake(velocityShakeAmplitude * speed, velocityShakeFrequency * speed);
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


        if (rb.linearVelocity.magnitude <= MaxSpeed)
        {
            rb.linearVelocity += engineForceVector * Time.fixedDeltaTime;
        }
        if (rb.linearVelocity.magnitude > MaxSpeed)
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
            rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, carDirection * velocity.magnitude, driftFriction);
        }


    }

    void Drifting()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (isDrifting)
            {
                driftBoostTimer += Time.deltaTime;
                if (driftBoostTimer > drift3Time)
                {
                    Debug.Log("3");

                    driftBoost = drift3Speed;
                    drift3Particle.Play();
                    drift2Particle.Stop();

                }
                else if (driftBoostTimer > drift2Time)
                {
                    Debug.Log("2");

                    driftBoost = drift2Speed;
                    drift2Particle.Play();
                    drift1Particle.Stop();

                }
                else if (driftBoostTimer > drift1Time)
                {
                    Debug.Log("1");
                    driftBoost = drift1Speed;
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
            //Put ending drift juice here.
            drift1Particle.Stop();
            drift2Particle.Stop();
            drift3Particle.Stop();
            ghostTrailBoost.SetGhostTrailEnabled(true);
            Invoke(nameof(StopEmittingGhost), 1.0f);

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
            steeringFactor = (steeringInput / driftTurnDelta) + driftDirection;
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
        else if (rb.linearVelocity.magnitude > MaxSpeed) //if dashing
        {
            steeringFactor = steeringInput;
            rotationAngle -= steeringFactor * turnSharpness * rb.linearVelocity.magnitude / (rb.linearVelocity.magnitude - MaxSpeed + 1);
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
