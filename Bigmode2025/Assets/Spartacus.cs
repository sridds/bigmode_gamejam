using UnityEngine;
using DG.Tweening;

public class Spartacus : MonoBehaviour
{
    public SpriteRenderer spartacusName;
    public float spartacusBlinkInterval;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if(timer > spartacusBlinkInterval)
        {
            spartacusName.enabled = !spartacusName.enabled;
            timer = 0.0f;
        }
    }

    public void KillSpartacus()
    {

    }
}
