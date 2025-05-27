using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameMenu : BaseMenu
{
    public override void InitState(MenuController context)
    {
        base.InitState(context);
        state = MenuController.MenuStates.InGame;

    }
}