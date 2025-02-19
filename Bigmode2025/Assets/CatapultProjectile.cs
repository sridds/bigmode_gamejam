using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class CatapultProjectile : MonoBehaviour
{
    [SerializeField] Transform projectile;
    [SerializeField] GameObject target;
    [SerializeField] float speed;
    [SerializeField] float height;
    [SerializeField] SpriteRenderer targetSprite;
    [SerializeField] SpriteRenderer boulderSprite;

    [SerializeField] private AudioClip[] impactSounds;
    [SerializeField] private AudioClip[] whistleSounds;

    [SerializeField] ParticleSystem boulderParticlePrefab;
    bool hasLanded;
    Vector2 targetStartPos;
    [SerializeField] CircleCollider2D hitbox;
    void Awake()
    {
        hitbox.enabled = false;
        projectile.localPosition = new Vector2(0, height);
        targetStartPos = target.transform.localScale;

    }

    private void Start()
    {
        boulderSprite.enabled = true;
        targetSprite.enabled = true;
        AudioManager.instance.PlaySound(whistleSounds[Random.Range(0, whistleSounds.Length)], 0.8f, 0.95f, 1.1f);
    }

    // Update is called once per frame
    void Update()
    {

        if (!hasLanded)
        {
            projectile.transform.localPosition -= new Vector3(0, speed * Time.deltaTime, 0);
            float distanceFromTarget = projectile.transform.localPosition.y - target.transform.localPosition.y;
            target.transform.localScale = Vector3.Lerp(targetStartPos, Vector2.zero, distanceFromTarget / height);
            if (distanceFromTarget <= 0)
            {
                StartCoroutine(Explode());
            }
        }
    }

    IEnumerator Explode()
    {
        //EffectController.instance.InstantScreenShake(0.8f, 30, 40, true);
        EffectController.instance.StartCoroutine(EffectController.instance.InstantScreenShake(0.5f, 20, 200, true));
        AudioManager.instance.PlaySound(impactSounds[Random.Range(0, impactSounds.Length)], 1.0f, 0.95f, 1.1f);

        hasLanded = true;
        hitbox.enabled = true;
        Destroy(projectile.gameObject);
        Destroy(target.gameObject);
        Instantiate(boulderParticlePrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.05f);
        hitbox.enabled = false;
        Destroy(gameObject);
    }
}
