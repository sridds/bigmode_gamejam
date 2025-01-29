using UnityEngine;

public class CatapultProjectile : MonoBehaviour
{
    [SerializeField] Transform projectile;
    [SerializeField] GameObject target;
    [SerializeField] float speed;
    float projectileStartPos;
    Vector2 targetStartPos;
    void Start()
    {
        projectileStartPos = projectile.position.y;
        targetStartPos = target.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        projectile.transform.localPosition -= new Vector3(0, speed * Time.deltaTime, 0);
        float distanceFromTarget = projectile.transform.position.y - target.transform.position.y;
        target.transform.localScale = Vector3.Lerp(targetStartPos, Vector2.zero, distanceFromTarget/projectileStartPos);
        if (distanceFromTarget <= 0 )
        {
            Explode();
        }
    }

    void Explode()
    {
        Destroy(gameObject);
    }
}
