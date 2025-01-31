using DG.Tweening;
using System.Collections;
using UnityEngine;
using static EnemyManager;

public class PlayerHealthScript : MonoBehaviour
{
    public float health = 100;
    public float maxHealth = 100;

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

    EnemyManager enemyManager;

    [SerializeField] int comboBeforeHeal;
    [SerializeField] int comboHealAmount;
    [SerializeField] ParticleSystem healParticle;
    bool hasHealed = false;
    private void OnEnable()
    {
        enemyManager = FindFirstObjectByType<EnemyManager>();

    }
    private void Update()
    {
        timer -= Time.deltaTime;
        Heal();
    }

    public void Heal()
    {
        if (enemyManager.Combo % comboBeforeHeal == 0 && enemyManager.Combo != 0)
        {
            if (hasHealed == false)
            {
                Instantiate(healParticle, transform.position, Quaternion.identity, transform);
                health += comboHealAmount;
                if (health > maxHealth)
                {
                    health = maxHealth;
                }
                hasHealed = true;
            }
        }
        else
        {
            hasHealed = false;
        }
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
        GameStateManager.instance.UpdateState(GameStateManager.PlayerState.dead);
    }
}
