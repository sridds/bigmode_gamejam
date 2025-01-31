using TMPro;
using UnityEngine;

public class FontOutline : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fontToOutline;
    [SerializeField] TextMeshProUGUI thisText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        thisText.text = fontToOutline.text;
    }
}
