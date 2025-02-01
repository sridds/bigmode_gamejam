using UnityEngine;

public class AudioStarter : MonoBehaviour
{
    public bool isShopTheme;

    private void Start()
    {

        if (isShopTheme) AudioManager.instance.PlayShopTheme();
        else
        {
            AudioManager.instance.StartStageTheme();
            AudioManager.instance.SwitchToMain();
        }
    }
}
