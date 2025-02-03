using System.Collections;
using UnityEngine;

public class Upgrade : MonoBehaviour
{

    public int ScoreCost;
    public float driftTurnDelta;
    public float driftFriction;
    public float maxSpeed;
    public float boostMultiplier;
    bool canBuy = true;
    public GameObject powerUpText;
    public Sprite powerUpSprite;
    public AudioClip audio;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && canBuy)
        {
            if (GameStateManager.instance.Score >= ScoreCost)
            {
                canBuy = false;

                GameStateManager.instance.AddScore(-ScoreCost);
                PlayerUpgrades.instance.driftTurnDelta += driftTurnDelta;
                PlayerUpgrades.instance.driftFriction += driftFriction;
                PlayerUpgrades.instance.maxSpeed += maxSpeed;
                PlayerUpgrades.instance.boostMultiplier += boostMultiplier;
                
                Bought();
            }
            else
            {
                StartCoroutine(Failed());
            }
        }
    }

    IEnumerator Failed()
    {
        canBuy = false;
        yield return new WaitForSeconds(0.4f);
        canBuy = true;
    }
    void Bought()
    {
        GameObject newEffect = Instantiate(powerUpText, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        newEffect.GetComponentInChildren<SpriteRenderer>().sprite = powerUpSprite;
        AudioManager.instance.PlaySound(audio, 0.7f, 0.95f, 1.1f);

        Destroy(gameObject);

    }
}
