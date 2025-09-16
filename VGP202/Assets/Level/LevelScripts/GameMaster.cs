using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameMaster : MonoBehaviour //sig
{
    private static GameMaster instance = null;

    [SerializeField] private GameObject playerPrefab;
    private GameObject currentPlayer;
    public int gamePlayed = 1;

    public bool isRewarded = false;

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

        StartCoroutine(displayBanner());
    }

    private IEnumerator displayBanner()
    {
        yield return new WaitForSeconds(1f);
        AdsManager.Instance.bannerAds.ShowBannerAd();
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //private void Update()
    //{
    //    isRewarded = false;
    //    if (gamePlayed % 5 == 0)
    //    {
    //        AdsManager.Instance.interstitialAds.ShowInterstitialAd();
    //    }
    //}
    public void roundEnded()
    {
        if (gamePlayed > 0 && gamePlayed % 3 == 0)
        {
            AdsManager.Instance.interstitialAds.ShowInterstitialAd();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //SpawnPlayer();

        if (scene.name == "Infinite")
        {
            gamePlayed++;
            SpawnPlayer();
            AdsManager.Instance.bannerAds.HideBannerAd();
        }

        if (scene.name.StartsWith("Level"))
        {
            gamePlayed++;
            SpawnPlayer();
            AdsManager.Instance.bannerAds.HideBannerAd();
        }

        if (scene.name.StartsWith("Main Menu"))
        {
            AdsManager.Instance.bannerAds.ShowBannerAd();
            roundEnded();
        }
    }

    public void SpawnPlayer()
    {
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }

        //Vector3 spawnPosition = Vector3.zero;
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
