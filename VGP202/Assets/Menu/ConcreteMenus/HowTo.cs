using UnityEngine;
using UnityEngine.UI;

public class HowTo : BaseMenu
{
    public Button backButton;

    public override void InitState(MenuController context)
    {
        base.InitState(context);
        state = MenuController.MenuStates.HowTo;

        backButton.onClick.AddListener(JumpBack);

    }
}





