using UnityEngine;
using DG.Tweening;

public class ScreenShakeMainMenu : MonoBehaviour
{
    public void ScreenShake()
    {
        Camera.main.DOShakePosition(0.5f, 2.5f, 9, 0, true);
    }
    public void ScreenShake2()
    {
        Camera.main.DOShakePosition(1.8f, 0.8f, 7, 0, true);
    }
}
