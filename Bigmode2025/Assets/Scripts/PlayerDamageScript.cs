using UnityEngine;

public class PlayerDamageScript : MonoBehaviour
{

    [SerializeField] float damageToDeal = 100;
    [SerializeField] PlayerHealthScript healthScript;
    [SerializeField] GameObject blood;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyHealthScript>().TakeDamage(damageToDeal);

            var dir = FindObjectOfType<PlayerMovement>().carDirection;
            Quaternion lookRot = Quaternion.LookRotation(Vector3.forward, dir);

            Instantiate(blood, collision.gameObject.transform.position, lookRot);
        }
    }
}
