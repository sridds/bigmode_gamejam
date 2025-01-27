using UnityEngine;

public class TrailHelper : MonoBehaviour
{
    [SerializeField] private float _trailSpeed = 0.3f;
    [SerializeField] private float _trailAmplitude = 0.01f;
    [SerializeField] private float _trailOffset = 5f;
    [SerializeField] private GameObject _trail;

    private void Update()
    {
        _trail.transform.localPosition = new Vector3(
            Mathf.Cos((Time.time + _trailOffset) * _trailSpeed) * _trailAmplitude,
            Mathf.Sin((Time.time + _trailOffset) * _trailSpeed) * _trailAmplitude);
    }
}
