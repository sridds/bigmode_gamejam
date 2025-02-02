using DG.Tweening;
using System.Collections;
using UnityEngine;
using static EnemyManager;

public class PlayerHealthScript : MonoBehaviour
{
    public int healthIncrements = 4;
    public int maxHealthIncrements = 4;

    [SerializeField] float invincibilityTime = 0.2f;
    float timer = 0;
    [SerializeField] Transform sprite;
    Tween collisionShake;
    [SerializeField] float damageShakeAmount;
    [SerializeField] int damageShakeVibrado;
    [SerializeField] float whiteFlashDuration;
    [SerializeField] Material whiteFlashMaterial, defaultMaterial;
    [SerializeField] SpriteRenderer[] spriteRenderers;

    [SerializeField]
    private AudioClip hurtSound;

    [SerializeField]
    private AudioClip deathSound;

    public delegate void DamageTaken(int oldHealth, int newHealth);
    public DamageTaken OnDamageTaken;

    public delegate void Healed(int oldHealth, int newHealth);
    public Healed OnHealed;

    EnemyManager enemyManager;

    [SerializeField] int comboBeforeHeal;
    [SerializeField] int comboHealAmount;
    [SerializeField] ParticleSystem healParticle;

    [SerializeField] ParticleSystem explosionPrefab;

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
                //healthIncrements += comboHealAmount;
                int oldHealth = healthIncrements;
                healthIncrements += 2;
                if (healthIncrements > maxHealthIncrements)
                {
                    healthIncrements = maxHealthIncrements;
                }
                hasHealed = true;
                OnHealed?.Invoke(oldHealth, healthIncrements);
            }
        }
        else
        {
            hasHealed = false;
        }
    }

    public void TakeDamage(float damage)
    {
        int previousHealth = healthIncrements;
        Debug.Log("Taking Damage");
        if (GameStateManager.instance.currentState != GameStateManager.PlayerState.Playing) return;
        if (healthIncrements <= 0) return;

        if (timer <= 0)
        {
            healthIncrements--;
            //health -= damage;

            sprite.transform.DOComplete();
            collisionShake = sprite.transform.DOShakePosition(0.65f, damageShakeAmount, damageShakeVibrado);

            StopCoroutine(DamageFlash());
            StartCoroutine(DamageFlash());

            FindObjectOfType<EnemyManager>().StopCombo();

            if (healthIncrements <= 0)
            {
                Die();
                EffectController.instance.StartCoroutine(EffectController.instance.InstantScreenShake(0.6f, 80, 200, true));
            }
            else
            {
                timer = invincibilityTime;
                AudioManager.instance.PlaySound(hurtSound, 1.0f, 0.95f, 1.1f);
                EffectController.instance.StartCoroutine(EffectController.instance.InstantScreenShake(0.4f, 50, 200, true));
            }

            AudioManager.instance.DamageEffect();

            
            OnDamageTaken?.Invoke(previousHealth, healthIncrements);
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
        Instantiate(explosionPrefab, transform.position, Quaternion.identity, gameObject.transform);
        GameStateManager.instance.UpdateState(GameStateManager.PlayerState.Dead);

        AudioManager.instance.PlaySound(deathSound, 0.9f, 0.95f, 1.1f);
    }
}
