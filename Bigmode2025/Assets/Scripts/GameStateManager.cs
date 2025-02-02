using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using Unity.VisualScripting;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance {  get; private set; }

    private float currentTime;
    private int maxCombo;

    public float CurrentTime { get { return currentTime; } }
    public int MaxCombo { get { return maxCombo; } }

    public delegate void GameStateUpdate(PlayerState lastState, PlayerState newState);
    public GameStateUpdate OnGameStateUpdated;


    public int Score;
    TextMeshProUGUI scoreUI;
    RectTransform scoreUIContainer;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    

    public void AddScore(int ScoreToAdd)
    {
        scoreUIContainer.DOComplete();
        scoreUIContainer.DOShakePosition(0.3f, 15, 140);
        if (EnemyManager.instance != null)
        {
            if (EnemyManager.instance.Combo > 1)
            {
                Score += ScoreToAdd + (EnemyManager.instance.Combo + 1) * 10;
            }
            else
            {
                Score += ScoreToAdd;
            }
        }
        else
        {
            Score += ScoreToAdd;
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
            StartCoroutine(FindFirstObjectByType<LevelTransitions>().DeathAnimation(1));
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

        if (currentState == PlayerState.Dead)
        {
            Score = 0;
        }
        if (currentState == PlayerState.Playing)
        {
        }
    }
}
