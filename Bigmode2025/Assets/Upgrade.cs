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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && canBuy)
        {
            if (GameStateManager.instance.Score >= ScoreCost)
            {
                //Remove Currency
                GameStateManager.instance.AddScore(-ScoreCost);
                PlayerUpgrades.instance.driftTurnDelta += driftTurnDelta;
                PlayerUpgrades.instance.driftFriction += driftFriction;
                PlayerUpgrades.instance.maxSpeed += maxSpeed;
                PlayerUpgrades.instance.boostMultiplier += boostMultiplier;
                StartCoroutine(Bought());
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
    IEnumerator Bought()
    {
        yield return null;
        Destroy(gameObject);

    }
}
