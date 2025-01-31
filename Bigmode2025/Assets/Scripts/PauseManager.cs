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

            if (isPaused)
            {
                pauseMenuObject.SetActive(true);
                GameStateManager.instance.UpdateState(GameStateManager.PlayerState.Paused);
            }
            else if (GameStateManager.instance.currentState != GameStateManager.PlayerState.Dead)
            {
                pauseMenuObject.SetActive(false);
                GameStateManager.instance.UpdateState(GameStateManager.PlayerState.Playing);
            }
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
