using UnityEngine;

public class ShopTransition : MonoBehaviour
{
    [SerializeField] LevelTransitions levelTransitionRef;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            levelTransitionRef.StartCoroutine(levelTransitionRef.StartWipe(true, false));
        }
    }
}
