using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : BaseMenu
{
    public Button playGame;
    public Button returnToMenu;

    InputManager inputManager;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
    }
    public override void InitState(MenuController context)
    {
        base.InitState(context);
        state = MenuController.MenuStates.Death;

        returnToMenu.onClick.AddListener(() => SceneManager.LoadScene("Main Menu"));
        playGame.onClick.AddListener(() => SceneManager.LoadScene("Infinite"));
    }

    public override void EnterState()
    {
        base.EnterState();
        Time.timeScale = 0.0f;
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

