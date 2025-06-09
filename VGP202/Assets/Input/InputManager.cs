using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class InputManager : Singleton<InputManager>
{
    PlayerControls input;
    Camera mainCamera;

    public event System.Action OnTouchBegin;
    public event System.Action OnTouchEnd;

    public Button pauseButton;
    public MenuController currentMenuController;

    
    protected override void Awake()
    {
        base.Awake();
        input = new PlayerControls();
        mainCamera = Camera.main;
        pauseButton.onClick.AddListener(() => currentMenuController.SetActiveState(MenuController.MenuStates.Pause));

    }
    

    private void OnEnable()
    {
        input.Enable();
        input.Screen.Touch.started += ctx => OnTouchBegin?.Invoke();
        input.Screen.Touch.canceled += ctx => OnTouchEnd?.Invoke();
    }

    private void OnDisable()
    {
        
        input.Screen.Touch.started -= ctx => OnTouchBegin?.Invoke();
        input.Screen.Touch.canceled -= ctx => OnTouchBegin?.Invoke();
        input.Disable();
    }
       
    public Vector2 PrimaryPosition()
    {
        Vector2 touchPos = input.Screen.PrimaryPosition.ReadValue<Vector2>();

        return mainCamera.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, mainCamera.nearClipPlane));

        //return mainCamera.ScreenToWorldPoint(new Vector3(touchPos.x, mainCamera.nearClipPlane, touchPos.y));
    }

}




























//using UnityEngine;
//using UnityEngine.InputSystem;

//public class InputManager : Singleton<InputManager>
//{
//    PlayerControls input;
//    Camera mainCamera;

//    public event System.Action OnTouchBegin;
//    public event System.Action OnTouchEnd;

//    //private System.Action<InputAction.CallbackContext> touchStartCallback;
//    //private System.Action<InputAction.CallbackContext> touchEndCallback;

//    protected override void Awake()
//    {
//        base.Awake();
//        input = new PlayerControls();
//        mainCamera = Camera.main;

//    }

//    private void OnEnable()
//    {
//        input.Enable();
//        //touchStartCallback = ctx => OnTouchBegin?.Invoke();
//        //touchEndCallback = ctx => OnTouchEnd?.Invoke();
//        //input.Screen.Touch.started += touchStartCallback;
//        //input.Screen.Touch.canceled += touchEndCallback;
//        input.Screen.Touch.started += ctx => OnTouchBegin?.Invoke();
//        input.Screen.Touch.canceled += ctx => OnTouchEnd?.Invoke();
//    }

//    private void OnDisable()
//    {

//        input.Screen.Touch.started -= ctx => OnTouchBegin?.Invoke();
//        input.Screen.Touch.canceled -= ctx => OnTouchBegin?.Invoke();
//        input.Disable();
//    }

//    public Vector2 PrimaryPosition()
//    {
//        Vector2 touchPos = input.Screen.PrimaryPosition.ReadValue<Vector2>();

//        return mainCamera.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, mainCamera.nearClipPlane));

//        //return mainCamera.ScreenToWorldPoint(new Vector3(touchPos.x, mainCamera.nearClipPlane, touchPos.y));
//    }

//}
