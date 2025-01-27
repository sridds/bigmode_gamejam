using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    [SerializeField]
    private Sprite _neutralFace;

    [SerializeField]
    private Sprite _shockedFace;

    [SerializeField]

    private Sprite _angryFace;

    [SerializeField]
    private Sprite _worriedFace;

    [SerializeField]
    private Sprite _decapitatedFace;

    [SerializeField]
    private Sprite _concernedFace;

    [SerializeField]
    private SpriteRenderer _renderer;

    private PlayerMovement player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        if(Vector3.Distance(player.transform.position, transform.position) < 5.0f)
        {
            _renderer.sprite = _shockedFace;
        }
        else
        {
            _renderer.sprite = _neutralFace;
        }
    }
}

// hey, can we talk? we had an agreement to respect eachothers space and i don't think my space has been respected recently. are you available to talk sometime soon in person?
