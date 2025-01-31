using UnityEngine;

public class ArcherEnemyTrigger : MonoBehaviour
{
    [SerializeField] ArcherEnemyAI archerEnemyAIScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && archerEnemyAIScript.canLunge && !archerEnemyAIScript.isTerrified && !archerEnemyAIScript.knockedBack)
        {
            archerEnemyAIScript.StartCoroutine(archerEnemyAIScript.LungeAttack());
        }
    }
}
