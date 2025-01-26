using UnityEngine;

public class PlayerDamageScript : MonoBehaviour
{

    [SerializeField] float damageToDeal = 100;
    [SerializeField] PlayerHealthScript healthScript;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyHealthScript>().TakeDamage(damageToDeal);
        }
    }
}
