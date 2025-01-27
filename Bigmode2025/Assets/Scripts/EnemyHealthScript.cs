using System.Collections;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    [SerializeField] private bool canDecapitate = true;
    [SerializeField] private GameObject decapitatedHead;
    [SerializeField] SpriteRenderer[] spriteRenderers;
    [SerializeField] float whiteFlashDuration;
    [SerializeField] Material whiteFlashMaterial, defaultMaterial;
        
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
            StopCoroutine(DamageFlash());
            StartCoroutine(DamageFlash());
            if (health <= 0)
            {
                Die();
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
