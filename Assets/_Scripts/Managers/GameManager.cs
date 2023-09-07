using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GameObject _victoryChoiceCanvas;
    [SerializeField] private GameObject _mainGUICanvas;
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private GameObject _cameraController;
    [SerializeField] private GameObject _victoryScreen;

    public static GameManager Instance;

    public GameState State { get; private set; }

    void Awake() 
    {
        Instance = this;
    }

    void Start() => ChangeState(GameState.LoadingScreen);
    // void Start()
    // {
    //     WorldManager.Instance.World.Create(new Vector3(50f, 2f, 50f));
    // }

    public void ChangeState(GameState newState)
    {
        OnBeforeStateChanged?.Invoke(newState);
        // Debug.Log($"GameManager has new state: { newState }");

        State = newState;
        switch (newState) 
        {
            case GameState.LoadingScreen:
                HandleLoading();
                break;
            case GameState.MainMenu:
                ShowMainMenu();
                break;
            case GameState.ChooseVictoryCondition:
                ShowVictoryConditions();
                break;
            case GameState.StartRound:
                StartRound();
                break;
            case GameState.InRound:
                break;
            case GameState.VictoryScreen:
                ShowVictoryScreen();
                break;
            case GameState.Quitting:
                HandleQuitting();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);
    }

    private async void HandleLoading() 
    {
        CreateBackgroundWorld();

        // show a loading screen and wait
        _loadingScreen.SetActive(true);
        await Task.Delay(2000);
        _loadingScreen.SetActive(false);

        // advance to MainMenu state
        ChangeState(GameState.MainMenu);
    }

    private void CreateBackgroundWorld()
    {
        WorldManager.Instance.CreateNewWorld();
    }

    private void ShowMainMenu()
    {
        _victoryChoiceCanvas.SetActive(false);
        _mainGUICanvas.SetActive(false);
        _mainMenuCanvas.SetActive(true);
    }

    private void ShowVictoryConditions()
    {
        _mainMenuCanvas.SetActive(false);
        _victoryChoiceCanvas.SetActive(true);
    }

    private void StartRound()
    {
        _victoryChoiceCanvas.SetActive(false);
        _mainMenuCanvas.SetActive(false);
        _mainGUICanvas.SetActive(true);
        WorldManager.Instance.CreateNewWorld();
        WorldManager.Instance.ConnectToGUI();

        ChangeState(GameState.InRound);
    }

    private void ShowVictoryScreen()
    {
        Destroy(WorldManager.Instance.World.gameObject);
        _victoryChoiceCanvas.SetActive(false);
        _mainMenuCanvas.SetActive(false);
        _mainGUICanvas.SetActive(false);
        _victoryScreen.SetActive(true);
    }

    private void HandleQuitting()
    {
        Debug.Log("GameManager quits out of the game.");
        Application.Quit();
    }
}

public enum GameState
{
    LoadingScreen,
    MainMenu,
    ChooseVictoryCondition,
    StartRound,
    InRound,
    VictoryScreen,
    Win,
    Lose,
    Quitting
}