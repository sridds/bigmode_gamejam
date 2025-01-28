using UnityEngine;

public class Shield : MonoBehaviour
{
    Vector2 startPos;
    [SerializeField] Transform enemy;

    enum Direction { Left, Right, Up, Down }
    [SerializeField] Direction shieldDirection;
    Transform player;
    [SerializeField] BoxCollider2D colliderRef;
    private void Start()
    {
        startPos = transform.localPosition;
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null)
        {
            Destroy(gameObject);
        }
        transform.position = startPos + (Vector2)enemy.position;
        switch (shieldDirection)
        {
            case Direction.Left:
                if (player.transform.position.x < transform.position.x)
                {
                    colliderRef.enabled = true;
                }
                else
                {
                    colliderRef.enabled = false;
                }
                break;
            case Direction.Right:
                if (player.transform.position.x > transform.position.x)
                {
                    colliderRef.enabled = true;
                }
                else
                {
                    colliderRef.enabled = false;
                }
                break;
            case Direction.Up:
                if (player.transform.position.y > transform.position.y)
                {
                    colliderRef.enabled = true;
                }
                else
                {
                    colliderRef.enabled = false;
                }
                break;
            case Direction.Down:
                if (player.transform.position.y < transform.position.y)
                {
                    colliderRef.enabled = true;
                }
                else
                {
                    colliderRef.enabled = false;
                }
                break;
        }

    }
}
