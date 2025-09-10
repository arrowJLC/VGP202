using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : BaseMenu
{
    public Button resumeGame;
    public Button returnToMenu;
    public Button quitGame;
    public bool isPaused = false;

    public Slider pauseMasterSlider;
    public TMP_Text pauseMasterText;

    public Slider pauseMusicSlider;
    public TMP_Text pauseMusicText;

    public Slider pauseSFXSlider;
    public TMP_Text pauseSFXText;

    public override void InitState(MenuController context)
    {
        base.InitState(context);
        state = MenuController.MenuStates.Pause;

        //returnToMenu.onClick.AddListener(() => SceneManager.LoadScene("Main Menu"));
        returnToMenu.onClick.AddListener(ReturnToMainMenu);

        //resumeGame.onClick.AddListener(() => SetNextMenu(MenuController.MenuStates.InGame));
        resumeGame.onClick.AddListener(() =>
        {
            SetNextMenu(MenuController.MenuStates.InGame);

            var player = GameObject.FindWithTag("Player")?.GetComponent<PlayerController>();
            player?.OnResumeGame();
        });
        quitGame.onClick.AddListener(QuitGame);

        if (AudioSettingsManager.Instance != null)
        {
            AudioSettingsManager.Instance.RegisterSlider("MasterVol", pauseMasterSlider, pauseMasterText);
            AudioSettingsManager.Instance.RegisterSlider("MusicVol", pauseMusicSlider, pauseMusicText);
            AudioSettingsManager.Instance.RegisterSlider("SFXVol", pauseSFXSlider, pauseSFXText);
        }
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

        AudioSettingsManager.Instance?.LoadAll();
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

    private void ReturnToMainMenu()
    {
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Destroy(player);
        }

        //Destroy(GameMaster.Instance.gameObject);
        SceneManager.LoadScene("Main Menu");
    }
}