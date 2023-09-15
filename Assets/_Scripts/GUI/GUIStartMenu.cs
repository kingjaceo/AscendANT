using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUIStartMenu : MonoBehaviour
{
    public Button _playButton;
    public Button _quitButton;

    [SerializeField] private TMP_Text _numAdaptationsText;
    private int _numAdaptations;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        _playButton.onClick.AddListener(() => { GameManager.Instance.ChangeState(GameState.ChooseVictoryCondition); }); 

        _quitButton.onClick.AddListener( () => { GameManager.Instance.ChangeState(GameState.Quitting); } );
    }

    public void IncreaseAdaptations()
    {
        Debug.Log("Increasing adaptations!");
        _numAdaptations++;
        _numAdaptationsText.text = _numAdaptations.ToString();
    }
}
