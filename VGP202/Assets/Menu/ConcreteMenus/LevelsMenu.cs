using UnityEngine;

using UnityEngine.UI;

public class LevelsMenu : BaseMenu
{
    public Button backButton;
    public Button quitButton;

    public override void InitState(MenuController context)
    {
        base.InitState(context);
        state = MenuController.MenuStates.Levels;

        backButton.onClick.AddListener(JumpBack);
        quitButton.onClick.AddListener(QuitGame);
    }
}

