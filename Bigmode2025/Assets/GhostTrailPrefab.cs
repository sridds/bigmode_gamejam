using UnityEngine;
using DG.Tweening;

public class GhostTrailPrefab : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _renderer;

    [SerializeField]
    private float _trailDestroyTime = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, _trailDestroyTime);
    }

    public void SetGhostTrail(SpriteRenderer renderer)
    {
        _renderer.sprite = renderer.sprite;
        _renderer.sortingLayerName = renderer.sortingLayerName;
        _renderer.sortingOrder = renderer.sortingOrder - 1;

        _renderer.DOFade(0.0f, _trailDestroyTime);
    }
}
