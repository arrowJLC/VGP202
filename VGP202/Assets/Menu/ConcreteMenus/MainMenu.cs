using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : BaseMenu
{
    public Button playButton;
    public Button levelsButton;
    public Button settingsButton;
    public Button creditsButton;
    public Button quitButton;

    public override void InitState(MenuController context)
    {
        base.InitState(context);
        state = MenuController.MenuStates.MainMenu;

        playButton.onClick.AddListener(() => SceneManager.LoadScene("Infinite"));
        levelsButton.onClick.AddListener(() => SetNextMenu(MenuController.MenuStates.Levels));
        settingsButton.onClick.AddListener(() => SetNextMenu(MenuController.MenuStates.Settings));
        quitButton.onClick.AddListener(QuitGame);

        creditsButton.onClick.AddListener(() => SetNextMenu(MenuController.MenuStates.Credits));
    }
}