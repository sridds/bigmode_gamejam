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
