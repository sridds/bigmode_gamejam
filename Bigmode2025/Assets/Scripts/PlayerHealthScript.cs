using UnityEngine;

public class PlayerHealthScript : MonoBehaviour
{
    public float health = 100;
    [SerializeField] float invincibilityTime = 0.2f;
    float timer = 0;
    private void Update()
    {
        timer -= Time.deltaTime;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("TakingDamage");
        if (timer <= 0)
        {
            Debug.Log("ActuallyTakingDamage");
            health -= damage;

            if (health <= 0)
            {
                Die();
            }
            else
            {
                timer = invincibilityTime;
            }
        }
    }
    void Die()
    {
        Debug.Log("Dead");
    }
}
