using UnityEngine;

public class AudioStarter : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.StartStageTheme();
        AudioManager.instance.SwitchToMain();
    }
}
