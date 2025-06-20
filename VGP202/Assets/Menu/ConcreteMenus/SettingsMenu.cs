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
    public Button muteButton;
    public Button muteSFXButton;
    public Button muteMusicButton;

    public GameObject SFXisMuted;
    public GameObject MusicisMuted;
    public GameObject MasterisMuted;

    private bool isMute = false;

    private float previousMasterVol = 1f;
    private float previousMusicVol = 1f;
    private float previousSFXVol = 1f;

    public Slider masterVolSlider;
    public TMP_Text masterVolSliderText;

    public Slider musicVolSlider;
    public TMP_Text musicVolSliderText;

    public Slider sfxVolSlider;
    public TMP_Text sfxVolSliderText;


    public override void InitState(MenuController context)
    {
        base.InitState(context);
        state = MenuController.MenuStates.Settings;

        backButton.onClick.AddListener(JumpBack);
        creditsButton.onClick.AddListener(() => SetNextMenu(MenuController.MenuStates.Credits));
        howToButton.onClick.AddListener(() => SetNextMenu(MenuController.MenuStates.HowTo));
        muteButton.onClick.AddListener(() => MuteAllSound());
        muteMusicButton.onClick.AddListener(() => MuteSound());
        muteSFXButton.onClick.AddListener(() => MuteSFXSound());

        SetupSliderInformation(masterVolSlider, masterVolSliderText, "MasterVol");
        SetupSliderInformation(musicVolSlider, musicVolSliderText, "MusicVol");
        SetupSliderInformation(sfxVolSlider, sfxVolSliderText, "SFXVol");

        SFXisMuted.SetActive(false);
        MusicisMuted.SetActive(false);
        MasterisMuted.SetActive(false);
    }

    void SetupSliderInformation(Slider mySlider, TMP_Text myText, string parameterName)
    {
        mySlider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, myText, parameterName, mySlider));
        float newVal = (mySlider.value == 0.0f) ? -80.0f : 20.0f * Mathf.Log10(mySlider.value);

        mixer.SetFloat(parameterName, newVal);

        //myText.text = (newVal == -80.0f) ? "0%" : (int)(mySlider.value * 10) + "%";

        //hisham updated 
        //sliderText.text = (value == -80.0f) ? "0%" : $"{(int)(slider.value * 100)}%";
        myText.text = (newVal == -80.0f) ? "0%" :  $"{(int)(mySlider.value * 100)}%";
        // 0 to 1 in the volume slider
    }

    void OnSliderValueChanged(float value, TMP_Text myText, string parameterName, Slider mySlider)
    {
        value = (value == 0.0f) ? -80.0f : 20.0f * Mathf.Log10(mySlider.value);
        //myText.text = (value == -80.0f) ? "0%" : (int)(mySlider.value * 10) + "%";
        myText.text = (value == -80.0f) ? "0%" : $"{(int)(mySlider.value * 100)}%";
        mixer.SetFloat(parameterName, value);
    }

    void MuteAllSound()
    {
        if (!isMute)
        {
            previousMasterVol = masterVolSlider.value;
            previousMusicVol = musicVolSlider.value;
            previousSFXVol = sfxVolSlider.value;

            mixer.SetFloat("MasterVol", -80f);
            mixer.SetFloat("MusicVol", -80f);
            mixer.SetFloat("SFXVol", -80f);

            masterVolSlider.value = 0f;
            musicVolSlider.value = 0f;
            sfxVolSlider.value = 0f;

            masterVolSliderText.text = "0%";
            musicVolSliderText.text = "0%";
            sfxVolSliderText.text = "0%";

            MasterisMuted.SetActive(true);
        }
        else
        {
            masterVolSlider.value = previousMasterVol;
            musicVolSlider.value = previousMusicVol;
            sfxVolSlider.value = previousSFXVol;

            SetupSliderInformation(masterVolSlider, masterVolSliderText, "MasterVol");
            SetupSliderInformation(musicVolSlider, musicVolSliderText, "MusicVol");
            SetupSliderInformation(sfxVolSlider, sfxVolSliderText, "SFXVol");

            MasterisMuted.SetActive(false);
        }

        isMute = !isMute;
    }

    void MuteSFXSound()
    {
        if (!isMute)
        {
            previousSFXVol = sfxVolSlider.value;

            mixer.SetFloat("SFXVol", -80f);

            sfxVolSlider.value = 0f;

            sfxVolSliderText.text = "0%";
            SFXisMuted.SetActive(true);
        }
        else
        {
            sfxVolSlider.value = previousSFXVol;

            SetupSliderInformation(sfxVolSlider, sfxVolSliderText, "SFXVol");

            SFXisMuted.SetActive(false);
        }

        isMute = !isMute;
    }

    void MuteSound()
    {
        if (!isMute)
        {
            previousMusicVol = musicVolSlider.value;

            mixer.SetFloat("MusicVol", -80f);

            musicVolSlider.value = 0f;

            musicVolSliderText.text = "0%";

            MusicisMuted.SetActive(true);
        }
        else
        {
            musicVolSlider.value = previousMusicVol;

            SetupSliderInformation(musicVolSlider, musicVolSliderText, "MusicVol");

            MusicisMuted?.SetActive(false);
        }

        isMute = !isMute;
    }
}