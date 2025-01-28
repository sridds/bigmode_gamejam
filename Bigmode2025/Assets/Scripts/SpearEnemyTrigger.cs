using UnityEngine;

public class SpearEnemyTrigger : MonoBehaviour
{
    [SerializeField] SpearEnemyAIScript spearEnemyAIScript;

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
        if (other.gameObject.CompareTag("Player") && spearEnemyAIScript.canLunge && !spearEnemyAIScript.isTerrified && !spearEnemyAIScript.knockedBack)
        {
            StartCoroutine(spearEnemyAIScript.LungeAttack());
        }
    }

}
