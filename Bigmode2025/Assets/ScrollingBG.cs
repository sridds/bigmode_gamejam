using UnityEngine;
using UnityEngine.UI;

public class ScrollingBG : MonoBehaviour
{
    [SerializeField]
    private Vector2 _scrollSpeed;
    [SerializeField]
    private RawImage _rawImage;

    private Vector2 rect;

    void Update()
    {
        rect.x += Time.deltaTime * _scrollSpeed.x;
        rect.y += Time.deltaTime * _scrollSpeed.y;

        _rawImage.uvRect = new Rect(rect.x, rect.y, _rawImage.uvRect.width, _rawImage.uvRect.height);
    }
}
