using DG.Tweening;
using System.Collections;
using UnityEngine;
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

    [SerializeField] float TimeUntilMaxDrift;
    [SerializeField] float driftBoostMaxSpeed;

    [SerializeField] float driftRotationSpeed; //How fast the car turns 90 degrees at the start of a drift


    float driftBoost;
    float driftBoostTimer;
    [Header("Refrences")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform sprite;

    [Header("Juice Refrences")]
    [SerializeField] Transform speedBoostFire;
    [SerializeField] ParticleSystem[] boostParticleSystem;
    [SerializeField] float treadJitterAmount = 0.1f;
    [SerializeField] TrailRenderer treadLeft;
    [SerializeField] TrailRenderer treadRight;

    float steeringInput = 0;
    float steeringFactor = 0;
    float driftDirection = 0; //gets set to steeringInput when drift starts

    float rotationAngle = 0;
    float visualRotationAngle = 0;

    Vector2 carDirection;
    bool isDrifting = false;
    bool driftQueued = false;
    [HideInInspector] public float animationAngle;

    void Update()
    {
        Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        SetInputVector(inputVector);
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


        if (rb.linearVelocity.magnitude <= maxSpeed)
        {
            rb.linearVelocity += engineForceVector * Time.fixedDeltaTime;
        }
        if (rb.linearVelocity.magnitude > maxSpeed)
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
                //While actively drifting
                if (driftBoostTimer < TimeUntilMaxDrift)
                {
                    driftBoost = Mathf.Lerp(0, driftBoostMaxSpeed, driftBoostTimer / TimeUntilMaxDrift);
                    driftBoostTimer += Time.deltaTime;
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
            sprite.transform.DOLocalJump(sprite.transform.localPosition, 0.8f, 1, driftRotationSpeed * 1.5f).OnComplete(() => EmitTreads(true));

            isDrifting = true;
        }

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
        }
        else if (rb.linearVelocity.magnitude > maxSpeed) //if dashing
        {
            steeringFactor = steeringInput;
            rotationAngle -= steeringFactor * turnSharpness * rb.linearVelocity.magnitude / (rb.linearVelocity.magnitude - maxSpeed + 1);
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
    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
    }

    //Helper Functions
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
