using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{

    [SerializeField] PauseManager pauseManager;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        pauseManager.Unpause();
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        pauseManager.Unpause();
    }

}
