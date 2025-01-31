using UnityEngine;

public class TimescaleManager : MonoBehaviour
{
    public static TimescaleManager instance { get; private set; }

   private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void Freeze()
    {
        Time.timeScale = 0;
    }

    public void Slow()
    {
        Time.timeScale = 0.2f;
    }

    public void Unfreeze()
    {
        Time.timeScale = 1;
    }
}
