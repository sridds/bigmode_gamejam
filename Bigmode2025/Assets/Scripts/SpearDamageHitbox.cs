using UnityEngine;

public class DamageHitbox : MonoBehaviour
{

    [SerializeField] float damageToDeal = 20;
    [SerializeField] EnemyHealthScript healthScript;
    [SerializeField] SpearEnemyAIScript spearEnemyAIScript;

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            healthScript.InvincibilityFrames(0.2f);
            spearEnemyAIScript.StopLunge();
            collision.gameObject.GetComponent<PlayerHealthScript>().TakeDamage(damageToDeal);
        }
    }
}
