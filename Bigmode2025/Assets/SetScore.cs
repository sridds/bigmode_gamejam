using TMPro;
using UnityEngine;

public class SetScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI thisText;
    // Update is called once per frame
    void Update()
    {
        thisText.text = GameStateManager.instance.Score.ToString();
    }
}
