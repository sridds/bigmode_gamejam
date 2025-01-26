using UnityEngine;

public class EnemySpriteLook : MonoBehaviour
{
    GameObject player;
    [SerializeField] SpearEnemyAIScript spearEnemyScript;

    [SerializeField] float rotationSpeed = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        LookAtPlayer();
    }

    public void LookAtPlayer()
    {
        if (!spearEnemyScript.lunging)
        {
            float angle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

    }


}
