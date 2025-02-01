using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CinematicBarController : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    public void Focus(float newHeight, float time, Ease easing, float angle)
    {
        _image.DOKill(true);
        _image.rectTransform.DOSizeDelta(new Vector2(_image.rectTransform.sizeDelta.x, newHeight), time).SetEase(easing);
        _image.rectTransform.DOLocalRotate(new Vector3(0, 0, angle), time, RotateMode.WorldAxisAdd);
    }
}
