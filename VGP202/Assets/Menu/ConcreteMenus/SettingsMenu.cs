using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : BaseMenu
{
    public AudioMixer mixer;

    public Button backButton;
    public Button creditsButton;
    public Button howToButton;


    public Slider masterVolSlider;
    public TMP_Text masterVolText;

    public Slider musicVolSlider;
    public TMP_Text musicVolText;

    public Slider sfxVolSlider;
    public TMP_Text sfxVolText;

    public override void InitState(MenuController context)
    {
        base.InitState(context);
        state = MenuController.MenuStates.Settings;

        backButton.onClick.AddListener(JumpBack);
        creditsButton.onClick.AddListener(() => SetNextMenu(MenuController.MenuStates.Credits));
        howToButton.onClick.AddListener(() => SetNextMenu(MenuController.MenuStates.HowTo));

        if (AudioSettingsManager.Instance != null)
        {
            AudioSettingsManager.Instance.RegisterSlider("MasterVol", masterVolSlider, masterVolText);
            AudioSettingsManager.Instance.RegisterSlider("MusicVol", musicVolSlider, musicVolText);
            AudioSettingsManager.Instance.RegisterSlider("SFXVol", sfxVolSlider, sfxVolText);
        }
    }

    public override void EnterState()
    {
        base.EnterState();

        if (AudioSettingsManager.Instance != null)
        {
            // Register pause menu sliders with the AudioSettingsManager
            AudioSettingsManager.Instance.RegisterSlider("MasterVol", masterVolSlider, masterVolText);
            AudioSettingsManager.Instance.RegisterSlider("MusicVol", musicVolSlider, musicVolText);
            AudioSettingsManager.Instance.RegisterSlider("SFXVol", sfxVolSlider, sfxVolText);

            // Refresh with saved values
            AudioSettingsManager.Instance.LoadAll();
        }
    }
}