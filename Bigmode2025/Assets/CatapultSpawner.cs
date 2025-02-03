using UnityEngine;

public class CatapultSpawner : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    GameObject player;
    Rigidbody2D playerRb;


    [SerializeField] float maxTimeBetweenProjectiles;
    [SerializeField] float minTimeBetweenProjectiles;
    [SerializeField] float timer = 0 ;
    [SerializeField] float targetTimer = 7;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > targetTimer)
        {
            SpawnProjectile();
            timer = 0;
            targetTimer = Random.Range(minTimeBetweenProjectiles, maxTimeBetweenProjectiles);
        }
    }
    
    void SpawnProjectile()
    {
        Vector3 spawnPos = Vector2.zero;
        spawnPos = (Vector2)player.transform.position + Random.insideUnitCircle * 2;

        Instantiate(projectile, spawnPos, Quaternion.identity) ;
    }
}
