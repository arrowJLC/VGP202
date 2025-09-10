using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

//[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    PlayerControls input;
    Camera mainCamera;

    public event System.Action OnTouchBegin;
    public event System.Action OnTouchEnd;

    public Button pauseButton;
    public MenuController currentMenuController;

    private bool isInputEnabled = true;

    private System.Action<UnityEngine.InputSystem.InputAction.CallbackContext> touchStarted;
    private System.Action<UnityEngine.InputSystem.InputAction.CallbackContext> touchCanceled;

    protected override void Awake()
    {
        base.Awake();
        input = new PlayerControls();
        mainCamera = Camera.main;

        touchStarted = ctx => OnTouchBegin?.Invoke();
        touchCanceled = ctx => OnTouchEnd?.Invoke();

        // Subscribe once only
        input.Screen.Touch.started += touchStarted;
        input.Screen.Touch.canceled += touchCanceled;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnEnable()
    {
        input?.Enable();
    }

    private void OnDisable()
    {
        input?.Disable();
    }

    private void OnDestroy()
    {
        if (input != null)
        {
            input.Disable();
            input.Screen.Touch.started -= touchStarted;
            input.Screen.Touch.canceled -= touchCanceled;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //protected override void Awake()
    //{
    //    base.Awake();
    //    input = new PlayerControls();
    //    mainCamera = Camera.main;

    //    touchStarted = ctx => OnTouchBegin?.Invoke();
    //    touchCanceled = ctx => OnTouchEnd?.Invoke();

    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //private void OnEnable()
    //{
    //    if (input == null)
    //        input = new PlayerControls();

    //    input.Enable();
    //    input.Screen.Touch.started += touchStarted;
    //    input.Screen.Touch.canceled += touchCanceled;
    //    //input.Screen.Touch.started += ctx => OnTouchBegin?.Invoke();
    //    //input.Screen.Touch.canceled += ctx => OnTouchEnd?.Invoke();
    //}

    //private void OnDisable()
    //{
    //    if (input != null)
    //    {
    //        input.Disable();
    //        input.Screen.Touch.started -= touchStarted;
    //        input.Screen.Touch.canceled -= touchCanceled;
    //        //input.Screen.Touch.started -= ctx => OnTouchBegin?.Invoke();
    //        //input.Screen.Touch.canceled -= ctx => OnTouchEnd?.Invoke();
    //    }
    //}

    //private void OnDestroy()
    //{
    //    // Always unsubscribe to prevent leaks
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    public Vector2 PrimaryPosition()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogWarning("Main camera not found!");
                return Vector2.zero;
            }
        }

        Vector2 touchPos = input.Screen.PrimaryPosition.ReadValue<Vector2>();
        return mainCamera.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, mainCamera.nearClipPlane));
    }

    public void SetInputEnabled(bool enabled)
    {
        isInputEnabled = enabled;
    }

    public bool IsInputEnabled()
    {
        return isInputEnabled;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mainCamera = Camera.main;
        //StartCoroutine(AssignPauseButtonWhenReady(scene.name));

        currentMenuController = FindFirstObjectByType<MenuController>();
        pauseButton = GameObject.Find("PauseButton")?.GetComponent<Button>();

        if (pauseButton != null && currentMenuController != null)
        {
            pauseButton.onClick.RemoveAllListeners();
            pauseButton.onClick.AddListener(() =>
                currentMenuController.SetActiveState(MenuController.MenuStates.Pause)
            );

            Debug.Log($"Pause button in scene: {scene.name}");
        }

        else
        {
            Debug.LogWarning($"PauseButton or MenuController not found in scene: {scene.name}");
            // pauseButton = GameObject.Find("PauseButton")?.GetComponent<Button>();
            StartCoroutine(AssignPause(scene.name));
        }
    }

    private IEnumerator AssignPause(string sceneName)
    {
        yield return null;
        yield return null;

        currentMenuController = FindObjectOfType<MenuController>();
        pauseButton = GameObject.Find("PauseButton")?.GetComponent<Button>();

        if (pauseButton != null && currentMenuController != null)
        {
            pauseButton.onClick.RemoveAllListeners();
            pauseButton.onClick.AddListener(() =>
                currentMenuController.SetActiveState(MenuController.MenuStates.Pause)
            );

            Debug.Log($"Pause button assigned in scene: {sceneName}");
        }
        else
        {
            Debug.LogWarning($"PauseButton or MenuController not found in scene: {sceneName}");
        }
    }
}















































//private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//{
//    mainCamera = Camera.main;

//    // player = GameObject.FindWithTag("Player")?.transform;
//    Transform existingPlayer = transform.Find("Player");
//    if (existingPlayer != null)
//    {
//        Destroy(existingPlayer.gameObject);
//    }



//    if (pauseButton == null)
//        pauseButton = GameObject.Find("PauseButton")?.GetComponent<Button>();

//    if (pauseButton != null)
//    {
//        pauseButton.onClick.AddListener(() =>
//        {
//            if (currentMenuController != null)
//                currentMenuController.SetActiveState(MenuController.MenuStates.Pause);
//        });
//    }

//    if (currentMenuController == null)
//    {
//        currentMenuController = FindObjectOfType<MenuController>();


//    }
//}

//private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//{
//    mainCamera = Camera.main;
//    currentMenuController = FindObjectOfType<MenuController>();
//    pauseButton = GameObject.Find("PauseButton")?.GetComponent<Button>();

//    if (pauseButton != null && currentMenuController != null)
//    {
//        pauseButton.onClick.RemoveAllListeners();
//        pauseButton.onClick.AddListener(() =>
//            currentMenuController.SetActiveState(MenuController.MenuStates.Pause)
//        );
//    }
//    else
//    {
//        Debug.LogWarning("PauseButton or MenuController not found in " + scene.name);
//    }
//}

//private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//{
//    mainCamera = Camera.main;

//    //currentMenuController = FindObjectOfType<MenuController>();

//    Transform existingPlayer = transform.Find("Player");
//    if (existingPlayer != null)
//    {
//        Destroy(existingPlayer.gameObject);
//    }

//    // Only assign when in the first game mode
//    if (scene.name.StartsWith("Level")) // or your first mode scene name pattern
//    {
//        currentMenuController = FindObjectOfType<MenuController>();
//        pauseButton = GameObject.Find("PauseButton")?.GetComponent<Button>();

//        if (pauseButton != null && currentMenuController != null)
//        {
//            pauseButton.onClick.RemoveAllListeners(); // avoid duplicate listeners
//            pauseButton.onClick.AddListener(() =>
//                currentMenuController.SetActiveState(MenuController.MenuStates.Pause)
//            );
//        }
//        else
//        {
//            Debug.LogWarning("PauseButton or MenuController not found in scene: " + scene.name);
//        }
//    }

//    if (scene.name == "Infinite")
//    {
//        currentMenuController = FindObjectOfType<MenuController>();
//        pauseButton = GameObject.Find("PauseButton")?.GetComponent<Button>();

//        if (pauseButton != null && currentMenuController != null)
//        {
//            pauseButton.onClick.RemoveAllListeners(); // avoid duplicate listeners
//            pauseButton.onClick.AddListener(() =>
//                currentMenuController.SetActiveState(MenuController.MenuStates.Pause)
//            );
//        }
//        else
//        {
//            Debug.LogWarning("PauseButton or MenuController not found in scene: " + scene.name);
//        }
//    }
//}

//private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//{
//    mainCamera = Camera.main;

//    // Always reassign MenuController first
//    currentMenuController = FindObjectOfType<MenuController>();

//    // Find PauseButton only if it exists in the scene
//    pauseButton = GameObject.Find("PauseButton")?.GetComponent<Button>();

//    if (pauseButton != null && currentMenuController != null)
//    {
//        pauseButton.onClick.RemoveAllListeners(); // avoid duplicate listeners
//        pauseButton.onClick.AddListener(() =>
//            currentMenuController.SetActiveState(MenuController.MenuStates.Pause)
//        );
//    }
//    else
//    {
//        Debug.LogWarning($"PauseButton or MenuController not found in scene: {scene.name}");
//    }
//}

//private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//{
//    mainCamera = Camera.main;

//    // Always reassign MenuController first
//    currentMenuController = FindObjectOfType<MenuController>();

//    // Find PauseButton only if it exists in the scene
//    pauseButton = GameObject.Find("PauseButton")?.GetComponent<Button>();

//    if (pauseButton != null && currentMenuController != null)
//    {
//        pauseButton.onClick.RemoveAllListeners(); // avoid duplicate listeners
//        pauseButton.onClick.AddListener(() =>
//            currentMenuController.SetActiveState(MenuController.MenuStates.Pause)
//        );
//    }
//    else
//    {
//        Debug.LogWarning($"PauseButton or MenuController not found in scene: {scene.name}");
//    }
//}



//protected override void Awake()
//{
//    base.Awake();
//    input = new PlayerControls();
//    mainCamera = Camera.main;

//    SceneManager.sceneLoaded += OnSceneLoaded;

//    //pauseButton.onClick.AddListener(() => currentMenuController.SetActiveState(MenuController.MenuStates.Pause));
//    //if (pauseButton == null)
//    //{
//    //    pauseButton = GameObject.Find("PauseButton")?.GetComponent<Button>();
//    //}

//    //if (pauseButton != null && currentMenuController != null)
//    //{
//    //    pauseButton.onClick.AddListener(() =>
//    //        currentMenuController.SetActiveState(MenuController.MenuStates.Pause)
//    //    );
//    //}
//}


//private void OnEnable()
//{
//    input.Enable();
//    input.Screen.Touch.started += ctx => OnTouchBegin?.Invoke();
//    input.Screen.Touch.canceled += ctx => OnTouchEnd?.Invoke();

//    SceneManager.sceneLoaded += OnSceneLoaded;
//}

//private void OnDisable()
//{
//    input.Screen.Touch.started -= ctx => OnTouchBegin?.Invoke();
//    input.Screen.Touch.canceled -= ctx => OnTouchBegin?.Invoke();
//    input.Disable();

//    SceneManager.sceneLoaded -= OnSceneLoaded;
//}

//public Vector2 PrimaryPosition()
//{
//    Vector2 touchPos = input.Screen.PrimaryPosition.ReadValue<Vector2>();

//    return mainCamera.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, mainCamera.nearClipPlane));

//    //return mainCamera.ScreenToWorldPoint(new Vector3(touchPos.x, mainCamera.nearClipPlane, touchPos.y));
//}

//private void OnDestroy()
//{
//    SceneManager.sceneLoaded -= OnSceneLoaded;
//}























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


//using System;
//using System.Collections;
//using TMPro;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class InputManager : Singleton<InputManager>
//{
//    private PlayerControls input;
//    private Camera mainCamera;

//    public event Action OnTouchBegin;
//    public event Action OnTouchEnd;

//    public Button pauseButton;
//    public MenuController currentMenuController;
//    //private PlayerControls input;

//    private bool isInputEnabled = true;

//    // Keep references so we can unsubscribe reliably
//    private Action<InputAction.CallbackContext> touchStarted;
//    private Action<InputAction.CallbackContext> touchCanceled;

//    protected override void Awake()
//    {
//        base.Awake();

//        // create generated input wrapper
//        //input = new PlayerControls();
//        if (input == null)
//            input = new PlayerControls();
//        mainCamera = Camera.main;

//        // store delegates (not anonymous inline lambdas)
//        touchStarted = ctx =>
//        {
//            if (isInputEnabled)
//                OnTouchBegin?.Invoke();
//        };
//        touchCanceled = ctx =>
//        {
//            if (isInputEnabled)
//                OnTouchEnd?.Invoke();
//        };

//        // Subscribe once (to the generated wrapper's actions)
//        input.Screen.Touch.started += touchStarted;
//        input.Screen.Touch.canceled += touchCanceled;

//        // Scene load callback
//        SceneManager.sceneLoaded += OnSceneLoaded;
//    }

//    private void OnEnable()
//    {
//        // safe to call even if already enabled
//        input?.Enable();
//    }

//    private void OnDisable()
//    {
//        input?.Disable();
//    }

//    private void OnDestroy()
//    {
//        // Unsubscribe to be tidy (only happens when app quits or manager destroyed)
//        if (input != null)
//        {
//            input.Screen.Touch.started -= touchStarted;
//            input.Screen.Touch.canceled -= touchCanceled;
//            input?.Disable();
//        }

//        SceneManager.sceneLoaded -= OnSceneLoaded;
//    }

//    public Vector2 PrimaryPosition()
//    {
//        if (mainCamera == null)
//        {
//            mainCamera = Camera.main;
//            if (mainCamera == null)
//            {
//                Debug.LogWarning("Main camera not found!");
//                return Vector2.zero;
//            }
//        }

//        Vector2 touchPos = input.Screen.PrimaryPosition.ReadValue<Vector2>();
//        var world = mainCamera.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, mainCamera.nearClipPlane));
//        return world;
//    }

//    public void SetInputEnabled(bool enabled)
//    {
//        isInputEnabled = enabled;
//    }

//    public bool IsInputEnabled() => isInputEnabled;

//    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//    {
//        mainCamera = Camera.main;

//        // Ensure input is enabled after a scene load — call Enable() (idempotent)
//        input?.Enable();

//        // Extra safety: re-enable next frame (covers late UI / timing problems)
//        StartCoroutine(ReEnableInputNextFrame());

//        // Re-assign pause button/menu references (your existing logic)
//        currentMenuController = FindObjectOfType<MenuController>();
//        pauseButton = GameObject.Find("PauseButton")?.GetComponent<Button>();

//        if (pauseButton != null && currentMenuController != null)
//        {
//            pauseButton.onClick.RemoveAllListeners();
//            pauseButton.onClick.AddListener(() => currentMenuController.SetActiveState(MenuController.MenuStates.Pause));
//            Debug.Log($"Pause button in scene: {scene.name}");
//        }
//        else
//        {
//            Debug.LogWarning($"PauseButton or MenuController not found in scene: {scene.name} — trying again next frame");
//            StartCoroutine(AssignPause(scene.name));
//        }
//    }

//    private IEnumerator ReEnableInputNextFrame()
//    {
//        yield return null;
//        input?.Enable();
//    }

//    private IEnumerator AssignPause(string sceneName)
//    {
//        // wait a couple frames in case UI instantiation is delayed
//        yield return null;
//        yield return null;

//        currentMenuController = FindObjectOfType<MenuController>();
//        pauseButton = GameObject.Find("PauseButton")?.GetComponent<Button>();

//        if (pauseButton != null && currentMenuController != null)
//        {
//            pauseButton.onClick.RemoveAllListeners();
//            pauseButton.onClick.AddListener(() => currentMenuController.SetActiveState(MenuController.MenuStates.Pause));
//            Debug.Log($"Pause button assigned in scene: {sceneName}");
//        }
//        else
//        {
//            Debug.LogWarning($"PauseButton or MenuController still not found in scene: {sceneName}");
//        }
//    }
//}


//using System;
//using TMPro;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using System.Collections;

//public class InputManager : MonoBehaviour
//{
//    private PlayerControls input;
//    private Camera mainCamera;

//    public event Action OnTouchBegin;
//    public event Action OnTouchEnd;

//    public Button pauseButton;
//    public MenuController currentMenuController;

//    private bool isInputEnabled = true;

//    private Action<InputAction.CallbackContext> touchStarted;
//    private Action<InputAction.CallbackContext> touchCanceled;

//    private void Awake()
//    {
//        input = new PlayerControls();

//        touchStarted = ctx => { if (isInputEnabled) OnTouchBegin?.Invoke(); };
//        touchCanceled = ctx => { if (isInputEnabled) OnTouchEnd?.Invoke(); };

//        SceneManager.sceneLoaded += OnSceneLoaded;
//    }

//    private void OnEnable()
//    {
//        input.Enable();
//        input.Screen.Touch.started += touchStarted;
//        input.Screen.Touch.canceled += touchCanceled;
//    }

//    private void OnDisable()
//    {
//        input.Screen.Touch.started -= touchStarted;
//        input.Screen.Touch.canceled -= touchCanceled;
//        input.Disable();
//    }

//    private void OnDestroy()
//    {
//        SceneManager.sceneLoaded -= OnSceneLoaded;
//    }

//    public Vector2 PrimaryPosition()
//    {
//        if (mainCamera == null)
//            mainCamera = Camera.main;

//        if (mainCamera == null)
//            return Vector2.zero;

//        Vector2 touchPos = input.Screen.PrimaryPosition.ReadValue<Vector2>();
//        return mainCamera.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, mainCamera.nearClipPlane));
//    }

//    public void SetInputEnabled(bool enabled) => isInputEnabled = enabled;
//    public bool IsInputEnabled() => isInputEnabled;

//    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//    {
//        mainCamera = Camera.main;
//        currentMenuController = FindFirstObjectByType<MenuController>();
//        pauseButton = GameObject.Find("PauseButton")?.GetComponent<Button>();

//        if (pauseButton != null && currentMenuController != null)
//        {
//            pauseButton.onClick.RemoveAllListeners();
//            pauseButton.onClick.AddListener(() =>
//                currentMenuController.SetActiveState(MenuController.MenuStates.Pause)
//            );
//            Debug.Log($"Pause button in scene: {scene.name}");
//        }
//        else
//        {
//            StartCoroutine(AssignPause(scene.name));
//        }
//    }

//    private IEnumerator AssignPause(string sceneName)
//    {
//        yield return null; // wait 1 frame
//        currentMenuController = FindFirstObjectByType<MenuController>();
//        pauseButton = GameObject.Find("PauseButton")?.GetComponent<Button>();

//        if (pauseButton != null && currentMenuController != null)
//        {
//            pauseButton.onClick.RemoveAllListeners();
//            pauseButton.onClick.AddListener(() =>
//                currentMenuController.SetActiveState(MenuController.MenuStates.Pause)
//            );
//            Debug.Log($"Pause button assigned in scene: {sceneName}");
//        }
//    }
//}