using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    [SerializeField] private bool canDecapitate = true;
    [SerializeField] private GameObject decapitatedHead;
        
    public float health = 1;
    float timer = 0;

    bool destroyFlag = false;

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    public bool CanTakeDamage()
    {
        if (timer <= 0) return true;

        return false;
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

            EffectController.instance.StartCoroutine(EffectController.instance.FreezeFrame(0.07f));
            EffectController.instance.StartCoroutine(EffectController.instance.InstantScreenShake(0.5f, 15, 200, true));
        }

        Destroy(gameObject);
        destroyFlag = true;
    }

    public void InvincibilityFrames(float invincibilityTime)
    {
        timer = invincibilityTime;
    }
}
