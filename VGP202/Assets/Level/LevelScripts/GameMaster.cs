using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameMaster : MonoBehaviour //sig
{
    private static GameMaster instance = null;

    [SerializeField] private GameObject playerPrefab;
    private GameObject currentPlayer;

    //public static GameMaster Instance
    //{
    //    get
    //    {
    //        if (!instance)
    //            instance = new GameObject("GameMaster").AddComponent<GameMaster>();
    //        return instance;
    //    }
    //}

    public event System.Action<PlayerController> OnPlayerSpawned;
    public static GameMaster Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("GameMaster instance not found in scene.");
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance && instance.GetInstanceID() != GetInstanceID())
        {
            Destroy(gameObject); // Prevent duplicates
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SpawnPlayer();

        //if (scene.name == "Infinite") // Replace with any relevant scene name
        //{
        //    SpawnPlayer();
        //}
    }

    public void SpawnPlayer()
    {
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }

        //Vector3 spawnPosition = Vector3.zero; // Adjust as needed
        Vector3 spawnPosition = new Vector3(-6, 1, 1);
        currentPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        OnPlayerSpawned?.Invoke(currentPlayer.GetComponent<PlayerController>());
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}















//public enum GameStates
//{
//    Infinite, Levels, Tap
//}

//public GameStates state;


//protected virtual void InfiniteBehavior()
//{

//}
//protected virtual void LevelsBehavior()
//{

//}
//protected virtual void TapBehavior()
//{

//}

//using UnityEngine;
//using UnityEngine.SceneManagement;

//[DefaultExecutionOrder(-1)]
//public class GameMaster : MonoBehaviour
//{
//    public static GameMaster Instance { get; private set; }

//    [SerializeField] private GameObject playerPrefab;
//    private GameObject currentPlayer;

//    private void Awake()
//    {
//        if (Instance && Instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }

//        Instance = this;
//        DontDestroyOnLoad(gameObject);
//    }

//    private void Start()
//    {
//        SceneManager.sceneLoaded += OnSceneLoaded;
//    }

//    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//    {
//        Debug.Log("Scene Loaded: " + scene.name);
//        if (scene.name == "Infinite")
//        {
//            SpawnPlayer();
//        }
//    }

//    private void SpawnPlayer()
//    {
//        if (playerPrefab == null)
//        {
//            Debug.LogError("Player prefab is not assigned.");
//            return;
//        }

//        if (currentPlayer != null)
//        {
//            Destroy(currentPlayer);
//        }

//        Vector3 spawnPosition = Vector3.zero;
//        currentPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
//    }

//    public void ExitGame()
//    {
//        Application.Quit();
//    }
//}
