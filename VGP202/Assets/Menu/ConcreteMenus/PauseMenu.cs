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
    public bool isPaused = false;

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
        isPaused = true;

        //InputManager.Instance.SetInputEnabled(false);

        // InputManager.Instance.DisableTouch();

        if (InputManager.Instance != null)
            InputManager.Instance.SetInputEnabled(false);
        //InputManager.Instance.inputEnabled = false;

        // Clear any ongoing touch so resume won't process it
        //var tapDetector = FindObjectOfType<TapDetection>();
        //if (tapDetector != null)
        //{
        //    tapDetector.ResetTouch();
        //}
    }

    public override void ExitState()
    {
        base.ExitState();
        Time.timeScale = 1.0f;
        isPaused = false;

        InputManager.Instance.SetInputEnabled(true);
        //// InputManager.Instance.EnableTouch();
        ///
        //Time.timeScale = 1.0f;
        //isPaused = false;

        //if (InputManager.Instance != null)
        //    InputManager.Instance.inputEnabled = true;

        //// Tell tap detection to ignore the next touch
        //var tapDetector = FindObjectOfType<TapDetection>();
        //if (tapDetector != null)
        //{
        //    tapDetector.IgnoreNextTouch();
        //}
    }

    public void OnDestroy()
    {
        Time.timeScale = 1.0f;
    }
}