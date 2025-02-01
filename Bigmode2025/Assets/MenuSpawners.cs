using UnityEngine;

public class MenuSpawners : MonoBehaviour
{
    [SerializeField]
    private GameObject dummyToSpawn;

    [SerializeField]
    private float minSpawnInterval;

    [SerializeField]
    private float maxSpawnInterval;

    [SerializeField]
    private float lifetime = 10.0f;

    float timer;
    float nextSpawnTime;
    bool doSpawning;

    public void StartSpawning()
    {
        doSpawning = true;
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        if (!doSpawning) return;

        timer += Time.deltaTime;

        if(timer > nextSpawnTime)
        {
            GameObject g = Instantiate(dummyToSpawn, transform.position + new Vector3(70, Random.Range(-17, 4)), Quaternion.identity);
            Destroy(g, lifetime);

            timer = 0.0f;
            nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }
}
