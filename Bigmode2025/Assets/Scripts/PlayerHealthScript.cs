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
        Debug.Log("Taking Damage");
        if (timer <= 0)
        {
            health -= damage;

            sprite.transform.DOComplete();
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
        Instantiate(explosionPrefab, transform.position, Quaternion.identity, gameObject.transform);
        GameStateManager.instance.UpdateState(GameStateManager.PlayerState.Dead);
    }
}
