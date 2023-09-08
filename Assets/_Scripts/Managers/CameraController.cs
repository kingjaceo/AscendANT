using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour 
{
 
    /*
    Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
    Converted to C# 27-02-13 - no credit wanted.
    Simple flycam I made, since I couldn't find any others made public.  
    Made simple to use (drag and drop, done) for regular keyboard layout  
    wasd : basic movement
    shift : Makes camera accelerate
    space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/
    public static CameraController Instance; 
    [SerializeField] private GameObject _camera;
    private Transform _cameraTransform;
    private bool _receivingInput = true;
        
    float mainSpeed = 10.0f; //regular speed
    float shiftAdd = 25.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 100.0f; //Maximum speed when holdin gshift
    float minHeight = 3;
    float maxHeight = 100;
    private float totalRun= 1.0f;

    [SerializeField] private float _cameraHeight = 20;
    public int interpolationFramesCount = 45;
    int elapsedFrames = 0;
        
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _cameraTransform = _camera.transform;
    }

    void Update () 
    {
        if (_receivingInput)
        {
            InputUpdate();
        }
    }

    void InputUpdate()
    {
        // control camera translation
        CameraTranslation();

        // control camera zoom
        CameraZoom();
    }

    private void CameraTranslation()
    {
        //Keyboard commands
        Vector3 p = GetBaseInput();
        if (p.sqrMagnitude > 0)
        { // only move while a direction key is pressed
            if (Keyboard.current.shiftKey.IsPressed())
            {
                totalRun += Time.unscaledDeltaTime;
                p  = p * totalRun * shiftAdd;
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            } 
            else 
            {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * mainSpeed;
            }

                p = p * Time.unscaledDeltaTime;
                // Vector3 newPosition = _cameraTransform.position;
 
                _cameraTransform.Translate(p);
        }
    }

    private void CameraZoom()
    {
        if (Mouse.current.scroll.IsActuated())
        {
            float zoomScale = 0.01f;

            if (Keyboard.current.shiftKey.IsPressed()) 
            {
                totalRun += Time.unscaledDeltaTime;
                zoomScale = zoomScale * totalRun * shiftAdd;
            }
        
            float newScrollValue = Mouse.current.scroll.ReadValue().y;

            _cameraHeight = Mathf.Clamp(_cameraHeight - newScrollValue * zoomScale, minHeight, maxHeight);

            ZoomSmooth();
        }
    }
        
    private Vector3 GetBaseInput() { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Keyboard.current.wKey.IsPressed())
        {
            p_Velocity += new Vector3(0, 0 , 1);
        }
        if (Keyboard.current.sKey.IsPressed())
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Keyboard.current.aKey.IsPressed())
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Keyboard.current.dKey.IsPressed())
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }

    private void ZoomSmooth()
    {
        float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;
        Vector3 newPosition = new Vector3(_cameraTransform.position.x, _cameraHeight, _cameraTransform.position.z);
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, newPosition, interpolationRatio);
        elapsedFrames = (elapsedFrames + 1) % (interpolationFramesCount + 1);
    }
}