using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuObject;

    public bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }

        if (isPaused)
        {
            pauseMenuObject.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseMenuObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Unpause()
    {
        if (isPaused)
        {
            isPaused = false;
            pauseMenuObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

}
