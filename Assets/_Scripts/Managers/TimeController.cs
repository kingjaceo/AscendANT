using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance;

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
            ToggleTime();
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SetTimeScale(1);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SetTimeScale(2);
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            SetTimeScale(3);
        }
    }

    private void ToggleTime()
    {
        if (_state == TimeState.Paused)
        {
            _state = _previousState;
            SetTimeScale((float) _state);
        }
        else
        {
            _state = TimeState.Paused;
            Time.timeScale = 0;
        }
    }

    private void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    private enum TimeState
    {
        Paused      = 0, 
        Speed1x     = 1,
        Speed2x     = 2,
        Speed3x     = 3
    }

    public void Activate()
    {
        _receivingInput = true;
    }

    public void Deactivate()
    {
        _receivingInput = false;
    }
}
