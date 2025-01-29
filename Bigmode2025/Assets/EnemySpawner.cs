using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int enemiesThisWave;
    int enemiesSpawned;
    public GameObject[] EnemyTypes;
    public GameObject[] SpawnPoints;
    public Transform enemyContainer;
    public int maxEnemies;
    Transform playerTransform;
    void Awake()
    {
        enemyContainer = GameObject.Find("EnemyContainer").transform;
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (enemiesSpawned < enemiesThisWave)
        {
            if (enemyContainer.childCount < maxEnemies)
            {
                int targetSpawnpoint = Random.Range(0, SpawnPoints.Length);
                if (Mathf.Abs((SpawnPoints[targetSpawnpoint].transform.position - playerTransform.position).magnitude) > 26)
                {
                    int enemyType = Random.Range(0, EnemyTypes.Length);
                    Instantiate(EnemyTypes[enemyType], SpawnPoints[targetSpawnpoint].transform.position, Quaternion.identity, enemyContainer);
                }
                enemiesSpawned++;
            }
        }

    }
}
