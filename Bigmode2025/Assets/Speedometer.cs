using UnityEngine;

public class Speedometer : MonoBehaviour
{
    [SerializeField]
    private float minAngle;

    [SerializeField]
    private float maxAngle;

    [SerializeField] Transform arrow;

    Rigidbody2D playerRigidbody;
    float z = 0;

    private void Awake()
    {
        playerRigidbody = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 targetSize;
        targetSize = new Vector3(0, 0, Mathf.Lerp(minAngle, maxAngle, playerRigidbody.linearVelocity.magnitude / 75));
        z += Random.Range(-2, 2);
        z = Mathf.Lerp(z,  targetSize.z, Time.deltaTime * 20);
        arrow.transform.eulerAngles = new Vector3(0, 0, z);
    }

}
