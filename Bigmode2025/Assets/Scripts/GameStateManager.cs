using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance {  get; private set; }
    
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
        playing,
        dead,
        paused
    }

    [SerializeField] public PlayerState currentState;

    public void UpdateState(PlayerState newState)
    {
        currentState = newState;

        if (currentState == PlayerState.dead)
        {
            TimescaleManager.instance.Freeze();
        }
        else if (currentState == PlayerState.paused)
        {
            TimescaleManager.instance.Freeze();
        }
        else if (currentState == PlayerState.playing)
        {
            TimescaleManager.instance.Unfreeze();
        }
    }
}
