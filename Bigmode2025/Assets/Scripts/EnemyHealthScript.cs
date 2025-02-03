using System.Collections;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    [SerializeField] private bool canDecapitate = true;
    [SerializeField] private GameObject decapitatedHead;
    [SerializeField] private ScreenJumper jumper;
    [SerializeField] SpriteRenderer[] spriteRenderers;
    [SerializeField] float whiteFlashDuration;
    [SerializeField] Material whiteFlashMaterial, defaultMaterial;
    [SerializeField] AudioClip[] hurtSounds;
    [SerializeField] AudioClip[] deathSounds;
    [SerializeField] AudioClip wilhelmScream;
    [SerializeField] int scoreToAdd;
    public float health = 1;
    float timer = 0;
    [SerializeField] bool isShopkeeper;
    [SerializeField] bool isShieldEnemy;
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
            StopCoroutine(DamageFlash());
            StartCoroutine(DamageFlash());
            if (health <= 0)
            {
                if (Random.Range(0, 13) == 6)
                {
                    AudioManager.instance.PlaySound(wilhelmScream, 0.24f, 0.95f, 1.1f);

                }

                AudioManager.instance.PlaySound(deathSounds[Random.Range(0, deathSounds.Length - 1)], 1.0f, 0.95f, 1.1f);
                Die();
            }
            else
            {
                EffectController.instance.StartCoroutine(EffectController.instance.InstantScreenShake(0.3f, 10, 200, true));
                AudioManager.instance.PlaySound(hurtSounds[Random.Range(0, hurtSounds.Length - 1)], 1.0f, 0.95f, 1.1f);
            }
        }
    }

    IEnumerator DamageFlash()
    {

        int i = 0;
        foreach(SpriteRenderer renderer in spriteRenderers)
        {
            renderer.material = whiteFlashMaterial;
            i++;
        }
        yield return new WaitForSeconds(whiteFlashDuration);

        i = 0;
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.material = defaultMaterial;
            i++;
        }
    }

    void Die()
    {
        if (destroyFlag) return;

        bool flying = false;

        if (!isShopkeeper)
        {
            if (Random.Range(5, 14) == 7 || FindObjectOfType<EnemyManager>().EnemiesRemaining == 1)
            {
                ScreenJumper j = Instantiate(jumper, transform.position, Quaternion.identity);
                flying = true;
            }
        }


        if (canDecapitate && !flying)
        {
            GameObject g = Instantiate(decapitatedHead, transform.position, Quaternion.identity);
        }

        EffectController.instance.StartCoroutine(EffectController.instance.FreezeFrame(0.07f));
        EffectController.instance.StartCoroutine(EffectController.instance.InstantScreenShake(0.5f, 15, 200, true));
        GameStateManager.instance.AddScore(scoreToAdd);

        if (!isShopkeeper)
        {
            FindFirstObjectByType<EnemyManager>().RegisterEnemyDeath(gameObject);
        }
        else
        {
            if (!isShieldEnemy)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject.transform.parent);
            }
        }
        
        destroyFlag = true;
    }

    public void InvincibilityFrames(float invincibilityTime)
    {
        timer = invincibilityTime;
    }
}
