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
        if (timer <= 0)
        {
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
