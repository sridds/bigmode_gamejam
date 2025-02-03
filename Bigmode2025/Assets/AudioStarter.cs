using UnityEngine;

public class AudioStarter : MonoBehaviour
{
    public bool isShopTheme;
    public bool isSpartacusLevel;

    private void Start()
    {

        if (isShopTheme) AudioManager.instance.PlayShopTheme();
        else
        {
            AudioManager.instance.StartStageTheme();
            AudioManager.instance.SwitchToMain();
        }

        if (isSpartacusLevel)
        {
            AudioManager.instance.StopAll();
        }
    }
}
