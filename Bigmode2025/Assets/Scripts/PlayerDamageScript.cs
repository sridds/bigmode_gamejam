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

            var dir = FindObjectOfType<PlayerMovement>().Velocity.normalized;
            Debug.DrawRay(dir, dir * 5.0f, Color.blue, 5.0f);


            Quaternion lookRot = Quaternion.LookRotation(Vector3.forward, dir);

            Instantiate(blood, collision.gameObject.transform.position, lookRot);
        }
    }
}
