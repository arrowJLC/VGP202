using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameMenu : BaseMenu
{
    private MenuController menuController;
    public override void InitState(MenuController context)
    {
        base.InitState(context);
        state = MenuController.MenuStates.InGame;

        menuController = context;
    }
    public void OnDeath(string finalTime)
    {
       // Debug.Log("On Death Triggered");
        menuController.SetDeathTime(finalTime);
        menuController.SetActiveState(MenuController.MenuStates.Death);
    }
}