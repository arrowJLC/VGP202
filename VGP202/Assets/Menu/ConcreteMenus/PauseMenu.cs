using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : BaseMenu
{
    public Button resumeGame;
    public Button returnToMenu;
    public Button quitGame;

    InputManager inputManager;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
    }
    public override void InitState(MenuController context)
    {
        base.InitState(context);
        state = MenuController.MenuStates.Pause;

        returnToMenu.onClick.AddListener(() => SceneManager.LoadScene("Main Menu"));
        //resumeGame.onClick.AddListener(() => SetNextMenu(MenuController.MenuStates.InGame));
        resumeGame.onClick.AddListener(() =>
        {
            SetNextMenu(MenuController.MenuStates.InGame);

            var player = GameObject.FindWithTag("Player")?.GetComponent<PlayerController>();
            player?.OnResumeGame(); // Clear any jump state
        });
        quitGame.onClick.AddListener(QuitGame);
    }

    public override void EnterState()
    {
        base.EnterState();
        Time.timeScale = 0.0f;
        //inputManager.stopTimer();
    }

    public override void ExitState()
    {
        base.ExitState();
        Time.timeScale = 1.0f;
        //inputManager.startTimer();
    }

    public void OnDestroy()
    {
        Time.timeScale = 1.0f;
    }
}