using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public enum GameStates
    {
        Infinite, Levels, Tap
    }

    public GameStates state;


    protected virtual void InfiniteBehavior()
    {

    }
    protected virtual void LevelsBehavior()
    {

    }
    protected virtual void TapBehavior()
    {

    }
}

/* using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public AudioMixerGroup SFXGroup;
    public AudioMixerGroup MusicGroup;
    [SerializeField] private AudioClip playerDeathSound;
    private AudioSource audioSource;


    [HideInInspector] public Action<int> OnLifeValueChanged;
    private static GameManager _instance;
    public static GameManager Instance => _instance;


    //GAME PROPERTIES
    [SerializeField] private int maxLives = 5;
    private int _lives;
    public int lives
    {
        get => _lives;
        set
        {
            //do valid checking
            if (value < _lives)
            {
                // Play death sound each time a life is lost
                if (playerDeathSound != null && audioSource != null)
                {
                    Debug.Log("Playing death sound");
                    audioSource.PlayOneShot(playerDeathSound);
                }
            }

            // If the lives drop below 0, trigger GameOver
            if (value <= 0)
            {
                GameOver();
                return;
            }

            if (_lives > value) Respawn();

            _lives = value;
            Debug.Log($"{_lives} lives left");
            OnLifeValueChanged?.Invoke(_lives);
        }
    }
    private int _score;
    public int score
    {
        get => _score;
        set
        {
            //this can't happen - the score can't be lower than zero so stop this from setting the score
            if (value > 0) return;

            _score = value;
            Debug.Log($"Current Score: {_score}");
        }
    }
    //GAME PROPERTIES

    //Player Controller information
    [SerializeField] private NewBehaviourScript playerPrefab;
    [HideInInspector] public NewBehaviourScript PlayerInstance => _playerInstance;
    private NewBehaviourScript _playerInstance;
    //Player Controller information

    private Transform currentCheckpoint;

    [HideInInspector]
    public MenuController currentMenuController;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            return;
        }

        Destroy(gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (maxLives <= 0) maxLives = 5;

        _lives = maxLives;

       
        //ResetPlayerStats();

    }

    // Update is called once per frame
    //DELEE UPDATE
    //void Update()
    //{
    //    if (SceneManager.GetActiveScene().name == "Main Menu" && Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        SceneManager.LoadScene("Level");
    //    }
    //    else if (SceneManager.GetActiveScene().name == "Game Over" && Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        SceneManager.LoadScene("Main Menu");
    //    }
    //}
    

    void GameOver()
    {
        
        Debug.Log("Game Over Should Go Here");

        if (playerDeathSound != null && audioSource != null)
        {
            Debug.Log("Playing 2 player death sound...");
            audioSource.PlayOneShot(playerDeathSound);
        }

        SceneManager.LoadScene("Game Over");



        //string sceneName = (SceneManager.GetActiveScene().name == "Game Over") ? "Main Menu" : "Game Over";
        //SceneManager.LoadScene(sceneName);

        //if (SceneManager.GetActiveScene().name == "Game Over" && Input.GetKeyDown(KeyCode.Escape))
        //{
        //   string sName = (SceneManager.GetActiveScene().name == "Main Menu") ? "Main Menu" : "Game Over";
        //   SceneManager.LoadScene(sName);
        //    _lives = 3;
        //    maxLives = 3;
        //    _lives = maxLives;

        //}

    }

    void Respawn()
    {
        //we need some animation and then the respawn happens
        _playerInstance.transform.position = currentCheckpoint.position;
    }

    public void SpawnPlayer(Transform spawnLocation)
    {
        _playerInstance = Instantiate(playerPrefab, spawnLocation.position, Quaternion.identity);
        currentCheckpoint = spawnLocation;
    }

    public void UpdateCheckpoint(Transform updatedCheckpoint)
    {
        currentCheckpoint = updatedCheckpoint;
        Debug.Log("Checkpoint updated");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            currentMenuController.SetActiveState(MenuController.MenuStates.Pause);
        }
    }

    //public void ResetPlayerStats()
    //{
    //    _lives = maxLives; 
    //    _score = 0;          
    //    Debug.Log("Player stats reset.");
    //}

    //// Start a new game (called from Main Menu, for example)
    //public void StartNewGame()
    //{
    //    ResetPlayerStats();  
    //    SceneManager.LoadScene("Level"); 
    //}

}

   
         
    
*/