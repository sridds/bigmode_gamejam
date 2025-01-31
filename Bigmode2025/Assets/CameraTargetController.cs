using UnityEngine;
using Unity.Cinemachine;
//using Unity

public class CameraTargetController : MonoBehaviour
{
    //Cinem
    [SerializeField]
    private CinemachineCamera camera1;

    [SerializeField]
    private CinemachineCamera camera2;

    [SerializeField]
    private CinemachineMixingCamera mixer;


    float defaultWeight0;
    float defaultWeight1;

    private void Start()
    {
        defaultWeight0 = mixer.Weight0;
        defaultWeight1 = mixer.Weight1;
    }

    public void SetFocus(Transform obj, float weight0, float weight1)
    {
        mixer.Weight0 = weight0;
        mixer.Weight1 = weight1;

        camera1.Target = new CameraTarget() { TrackingTarget = obj };
    }

    public void SetToDefault()
    {
        mixer.Weight0 = defaultWeight0;
        mixer.Weight1 = defaultWeight1;

        camera1.Target = new CameraTarget() { TrackingTarget = FindObjectOfType<PlayerMovement>().transform };
    }
}
