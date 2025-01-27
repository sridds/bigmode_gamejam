using UnityEngine;

public class DamageHitbox : MonoBehaviour
{

    [SerializeField] float damageToDeal = 20;
    [SerializeField] EnemyHealthScript healthScript;
    [SerializeField] SpearEnemyAIScript spearEnemyAIScript;

    private void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //healthScript.InvincibilityFrames(0.01f);
            spearEnemyAIScript.StopLunge();
            collision.gameObject.GetComponent<PlayerHealthScript>().TakeDamage(damageToDeal);
        }
    }
}
