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
    bool hasLanded;
    Vector2 targetStartPos;
    [SerializeField] CircleCollider2D hitbox;
    void Awake()
    {
        hitbox.enabled = false;
        projectile.localPosition = new Vector2(0, height);
        targetStartPos = target.transform.localScale;
        boulderSprite.enabled = true;
        targetSprite.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasLanded)
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
        hasLanded = true;
        hitbox.enabled = true;
        Destroy(projectile.gameObject);
        Destroy(target.gameObject);
        yield return new WaitForSeconds(0.2f);
        hitbox.enabled = false;
        Destroy(gameObject);
    }
}
