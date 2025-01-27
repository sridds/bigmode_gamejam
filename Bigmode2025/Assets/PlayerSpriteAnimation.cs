using UnityEngine;

public class PlayerSpriteAnimation : MonoBehaviour
{
    float carAngle;
    [SerializeField] Sprite[] carSprites;
    [SerializeField] SpriteRenderer carSpriteRenderer;
    [SerializeField] PlayerMovement playerMovementRef;
    [SerializeField] float betweenFrameRotationAmount;
    int targetSprite = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        carAngle = playerMovementRef.animationAngle;

        float angleSizePerSprite = 360 / (carSprites.Length-1);
        targetSprite = Mathf.RoundToInt(carAngle / angleSizePerSprite);

        float angleMultiplier = (targetSprite - (carAngle / angleSizePerSprite)) * betweenFrameRotationAmount;
        carSpriteRenderer.transform.eulerAngles = new Vector3(0, 0, -angleMultiplier);

        carSpriteRenderer.sprite = carSprites[targetSprite];

    }
}

