using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Button _playButton;
    public Button _quitButton;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        // _playButton.onClick.AddListener( () => { GameManager.Instance.ChangeState(GameState.WorldGeneration); } ); 

        // _quitButton.onClick.AddListener( () => { GameManager.Instance.ChangeState(GameState.Quitting); } );
    }
}