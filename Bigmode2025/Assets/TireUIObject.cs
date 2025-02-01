using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class TireUIObject : MonoBehaviour
{
    [SerializeField]
    private Image tireImage;

    [SerializeField]
    private Sprite tireSpriteFilled;

    [SerializeField]
    private Sprite tireSpriteEmpty;

    public void Shake(float intensity)
    {
        tireImage.transform.DOShakePosition(0.5f, intensity, 45, 90, false, true, ShakeRandomnessMode.Full);
    }

    public void MakeEmpty()
    {
        tireImage.sprite = tireSpriteEmpty;
    }

    public void MakeFilled()
    {
        tireImage.sprite = tireSpriteFilled;
    }
}
