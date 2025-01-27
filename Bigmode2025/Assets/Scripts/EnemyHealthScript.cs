using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    [SerializeField] private bool canDecapitate = true;
    [SerializeField] private GameObject decapitatedHead;
        
    public float health = 100;
    float timer = 0;

    bool destroyFlag = false;

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
        }
    }
    void Die()
    {
        if (destroyFlag) return;

        if (canDecapitate)
        {
            GameObject g = Instantiate(decapitatedHead, transform.position, Quaternion.identity);
            EffectController.instance.InstantScreenShake(0.3f, 15, 200, true);
        }

        Destroy(gameObject);
        destroyFlag = true;
    }

    public void InvincibilityFrames(float invincibilityTime)
    {
        timer = invincibilityTime;
    }
}
