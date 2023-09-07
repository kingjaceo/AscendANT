using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour 
{
    public static MenuManager Instance;

    [SerializeField] private GameObject _startMenuCanvas;

   void Awake()
   {
      GameManager.OnAfterStateChanged += GameManager_OnAfterStateChanged;
   }

   void OnDestroy()
   {
      GameManager.OnAfterStateChanged -= GameManager_OnAfterStateChanged;
   }

    private void GameManager_OnAfterStateChanged(GameState state)
    {
        // switch (state)
        // {
        //     case GameState.MainMenu:
        //         Debug.Log("MenuManager turns on Start Menu");
        //         _startMenuCanvas.SetActive(true);
        //         break;
        //     case GameState.InProgress:
        //         Debug.Log("MenuManager turns off Start Menu");
        //         _startMenuCanvas.SetActive(false);
        //         break;
        //     default:
        //         break;
        // }
    }
}