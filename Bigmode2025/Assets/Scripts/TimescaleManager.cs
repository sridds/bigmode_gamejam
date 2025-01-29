using UnityEngine;

public class TimescaleManager : MonoBehaviour
{
    public static TimescaleManager instance { get; private set; }

    public bool isDead = false;
    public bool isPaused = false;

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

    private void Update()
    {
        if (isDead)
        {
            Time.timeScale = 0;
        }
        else if (isPaused)
        {
            Time.timescale = 0;
        }
        else
        {
            Time.timescale = 1;
        }
    }
}
