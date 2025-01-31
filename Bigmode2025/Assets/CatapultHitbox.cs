using UnityEngine;

public class CatapultHitbox : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colliding");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Colliding With Player");
            collision.gameObject.GetComponent<PlayerHealthScript>().TakeDamage(40);
        }
    }
}
