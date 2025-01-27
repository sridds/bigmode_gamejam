using UnityEngine;

public class PlayerDamageScript : MonoBehaviour
{

    [SerializeField] float damageMultiplier = 100;
    [SerializeField] PlayerHealthScript healthScript;
    [SerializeField] PlayerMovement movementScript;

    [SerializeField] GameObject blood;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyHealthScript>().TakeDamage(damageMultiplier * movementScript.currentSpeed);

            var dir = FindFirstObjectByType<PlayerMovement>().Velocity.normalized;
            Debug.DrawRay(dir, dir * 5.0f, Color.blue, 5.0f);


            Quaternion lookRot = Quaternion.LookRotation(Vector3.forward, dir);

            if(collision.gameObject.GetComponent<EnemyHealthScript>().CanTakeDamage()) Instantiate(blood, collision.gameObject.transform.position, lookRot);
        }
    }
}
