using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance;

    [SerializeField] private GameObject _pauseDisplay;

    private bool _receivingInput = false;
    private TimeState _state = TimeState.Paused;
    private TimeState _previousState = TimeState.Speed1x;
    
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (_receivingInput)
        {
            InputUpdate();
        }
    }

    void InputUpdate()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // Debug.Log("TIME: Spacebar pressed, toggling time scale");
            ToggleTime();
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            _state = TimeState.Speed1x;
            SetTimeScale(1);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            _state = TimeState.Speed2x;
            SetTimeScale(2);
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            _state = TimeState.Speed3x;
            SetTimeScale(3);
        }

        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            _state = TimeState.SpeedxX;
            SetTimeScale(10);
        }
    }

    private void ToggleTime()
    {
        _pauseDisplay.SetActive(!_pauseDisplay.activeSelf);
        if (_state == TimeState.Paused)
        {
            _state = _previousState;
            SetTimeScale((float) _state);
            
        }
        else
        {
            _previousState = _state;
            _state = TimeState.Paused;
            Time.timeScale = 0;
        }
    }

    private void SetTimeScale(float scale)
    {
        _pauseDisplay.SetActive(false);
        Time.timeScale = scale;
    }

    private enum TimeState
    {
        Paused      = 0, 
        Speed1x     = 1,
        Speed2x     = 2,
        Speed3x     = 3,
        SpeedxX     = 10
    }

    public void Activate()
    {
        _receivingInput = true;
        SetTimeScale(0);
        _pauseDisplay.SetActive(true);
    }

    public void Deactivate()
    {
        _receivingInput = false;
    }
}
