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

    public void Freeze()
    {
        Time.timeScale = 0;
    }

    public void Unfreeze()
    {
        Time.timeScale = 1;
    }
}
