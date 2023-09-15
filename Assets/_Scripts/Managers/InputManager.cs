using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [SerializeField] private GameObject _escapeMenu;

    private bool _receivingInput;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (_receivingInput)
        {
            InputUpdate();
        }
    }

    void InputUpdate()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ToggleEscapeMenu();
        }
    }

    void ToggleEscapeMenu()
    {
        _escapeMenu.SetActive(!_escapeMenu.activeSelf);

        if (_escapeMenu.activeSelf)
        {
            TimeController.Instance.PauseTime();
            TimeController.Instance.Deactivate();
        }
        else TimeController.Instance.Activate();
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