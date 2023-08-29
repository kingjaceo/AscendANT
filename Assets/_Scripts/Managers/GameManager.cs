using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public static GameManager Instance;

    public GameState State { get; private set; }

    void Awake() 
    {
        Instance = this;
    }

    void Start() => ChangeState(GameState.Starting);

    public void ChangeState(GameState newState)
    {
        OnBeforeStateChanged?.Invoke(newState);
        Debug.Log($"GameManager has new state: { newState }");

        State = newState;
        switch (newState) 
        {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.Waiting:
                break;
            case GameState.WorldGeneration:
                HandleWorldGeneration();
                break;
            case GameState.ColonyGeneration:
                HandleColonyGeneration();
                break; 
            case GameState.InProgress:
                break;
            case GameState.InStartMenu:
                break;
            case GameState.Quitting:
                HandleQuitting();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);
    }

    private async void HandleStarting() 
    {
        await Task.Delay(1000);

        ChangeState(GameState.WorldGeneration);
    }


    private async void HandleWorldGeneration()
    {
        // produce the world and environment
        await Task.Delay(1000);

        ChangeState(GameState.ColonyGeneration);
    }

    private async void HandleColonyGeneration()
    {
        // produce the colony
        await Task.Delay(1000);

        ChangeState(GameState.InProgress);
    }

    private void HandleQuitting()
    {
        Debug.Log("GameManager quits out of the game.");
        Application.Quit();
    }
}

public enum GameState
{
    Starting,
    Waiting,
    WorldGeneration,
    ColonyGeneration,
    InProgress,
    InStartMenu,
    Win,
    Lose,
    Quitting
}