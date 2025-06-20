using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : BaseMenu
{
    public Button playGame;
    public Button returnToMenu;

    public TMP_Text timeDisplay;

    InputManager inputManager;

    private MenuController menuController;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
    }
    public override void InitState(MenuController context)
    {
        base.InitState(context);
        state = MenuController.MenuStates.Death;


        menuController = context;

        returnToMenu.onClick.AddListener(() => SceneManager.LoadScene("Main Menu"));
        playGame.onClick.AddListener(() => SceneManager.LoadScene("Infinite"));
        GameMaster.Instance.SpawnPlayer();
    }

    public override void EnterState()
    {
        base.EnterState();
        Time.timeScale = 0.0f;
        
        Debug.Log("GameOverMenu showing time: " + menuController.GetDeathTime());

        string finalTime = menuController.GetDeathTime();
        timeDisplay.text = finalTime;
    }

    public override void ExitState()
    {
        base.ExitState();
        Time.timeScale = 1.0f;
    }

    public void OnDestroy()
    {
        Time.timeScale = 1.0f;
    }
}

