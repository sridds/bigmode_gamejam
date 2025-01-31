using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance {  get; private set; }

    private float currentTime;
    private int maxCombo;

    public float CurrentTime { get { return currentTime; } }
    public int MaxCombo { get { return maxCombo; } }

    public delegate void GameStateUpdate(PlayerState lastState, PlayerState newState);
    public GameStateUpdate OnGameStateUpdated;

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

    public enum PlayerState
    {
        Playing,
        Dead,
        Paused,
        Winscreen,
        Cutscene,
        Pregame
    }

    [SerializeField] public PlayerState currentState;

    private PlayerState lastState;

    public void UpdateState(PlayerState newState)
    {
        lastState = currentState;
        currentState = newState;

        OnGameStateUpdated?.Invoke(lastState, newState);

        if (currentState == PlayerState.Dead)
        {
            StartCoroutine(FindObjectOfType<LevelTransitions>().DeathAnimation(0.2f, 5));
            TimescaleManager.instance.Slow();
        }
        else if (currentState == PlayerState.Paused)
        {
            TimescaleManager.instance.Freeze();
        }
        else if (currentState == PlayerState.Playing)
        {
            TimescaleManager.instance.Unfreeze();
        }
    }

    private void Update()
    {
        // update timer
        if(currentState == PlayerState.Playing)
        {
            currentTime += Time.deltaTime;
        }
    }
}
