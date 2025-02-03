using UnityEngine;

public class CatapultHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        Debug.Log("Colliding");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Colliding With Player");
            collision.gameObject.GetComponent<PlayerHealthScript>().TakeDamage(40);
        }
    }
}
