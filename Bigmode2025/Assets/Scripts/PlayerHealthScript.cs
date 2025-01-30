using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerHealthScript : MonoBehaviour
{
    public float health = 100;
    [SerializeField] float invincibilityTime = 0.2f;
    float timer = 0;
    [SerializeField] Transform sprite;
    Tween collisionShake;
    [SerializeField] float damageShakeAmount;
    [SerializeField] int damageShakeVibrado;
    [SerializeField] float whiteFlashDuration;
    [SerializeField] Material whiteFlashMaterial, defaultMaterial;
    [SerializeField] SpriteRenderer[] spriteRenderers;

    public delegate void DamageTaken();
    public DamageTaken OnDamageTaken;


    private void Update()
    {
        timer -= Time.deltaTime;
    }

    public void TakeDamage(float damage)
    {
        if (timer <= 0)
        {
            health -= damage;

            collisionShake.Complete();
            collisionShake = sprite.transform.DOShakePosition(0.65f, damageShakeAmount, damageShakeVibrado);

            StopCoroutine(DamageFlash());
            StartCoroutine(DamageFlash());

            FindObjectOfType<EnemyManager>().StopCombo();

            if (health <= 0)
            {
                Die();
            }
            else
            {
                timer = invincibilityTime;
            }
            OnDamageTaken?.Invoke();
        }
    }

    IEnumerator DamageFlash()
    {

        int i = 0;
        foreach (SpriteRenderer renderer in spriteRenderers)
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
        Debug.Log("Dead");
    }
}
