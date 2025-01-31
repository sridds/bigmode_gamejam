using UnityEngine;

public class ArcherDamageHitbox : MonoBehaviour
{
    [SerializeField] float damageToDeal = 20;
    [SerializeField] public EnemyHealthScript healthScript;
    [SerializeField] public ArcherEnemyAI archerEnemyAIScript;

    private void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //healthScript.InvincibilityFrames(0.01f);
            archerEnemyAIScript.StopLunge();
            collision.gameObject.GetComponent<PlayerHealthScript>().TakeDamage(damageToDeal);
        }
    }
}
