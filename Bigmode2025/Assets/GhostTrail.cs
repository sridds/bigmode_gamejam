using UnityEngine;

public class GhostTrail : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] _renderers;

    [SerializeField]
    private GhostTrailPrefab _ghostTrailPrefab;

    [SerializeField]
    private float _trailInterval = 0.2f;

    float timer = 0.0f;
    bool ghostTrailEnabled = false;

    void Update()
    {
        if (!ghostTrailEnabled)
        {
            timer = 0.0f;
            return;
        }

        timer += Time.deltaTime;

        if(timer > _trailInterval)
        {
            CreateGhostTrail();
            timer = 0.0f;
        }
    }

    public void CreateGhostTrail()
    {
        foreach(SpriteRenderer renderer in _renderers)
        {
            GhostTrailPrefab prefab = Instantiate(_ghostTrailPrefab, renderer.transform.position, renderer.transform.rotation);
            prefab.SetGhostTrail(renderer);
            prefab.transform.localScale = renderer.gameObject.transform.localScale;
        }
    }

    public void SetGhostTrailEnabled(bool enabled)
    {
        ghostTrailEnabled = enabled;
    }
}
