using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AudioSettingsManager : MonoBehaviour
{
    public static AudioSettingsManager Instance;

    [Header("Audio")]
    public AudioMixer mixer;

    private Dictionary<string, (Slider slider, TMP_Text text)> registeredSliders = new();

    [Header("Mute Buttons")]
    public Button muteMasterButton;
    public Button muteMusicButton;
    public Button muteSFXButton;

    [Header("Mute Indicators")]
    public GameObject masterMutedIcon;
    public GameObject musicMutedIcon;
    public GameObject sfxMutedIcon;

    // Store last non-muted values
    private float previousMaster = 1f;
    private float previousMusic = 1f;
    private float previousSFX = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        muteMasterButton?.onClick.AddListener(ToggleMasterMute);
        muteMusicButton?.onClick.AddListener(ToggleMusicMute);
        muteSFXButton?.onClick.AddListener(ToggleSFXMute);

        LoadAll();
    }

    public void RegisterSlider(string parameter, Slider slider, TMP_Text text)
    {
        if (slider == null) return;

        registeredSliders[parameter] = (slider, text);

        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(v => SetVolume(parameter, v, text));
    }

    public void SetVolume(string parameter, float value, TMP_Text text)
    {
        float dbValue = (value <= 0.0001f) ? -80f : Mathf.Log10(value) * 20f;
        mixer.SetFloat(parameter, dbValue);

        if (text != null)
            text.text = (value <= 0.0001f) ? "0%" : $"{(int)(value * 100)}%";

        PlayerPrefs.SetFloat(parameter, value);
        PlayerPrefs.Save();


        if (parameter == "MasterVol" && masterMutedIcon != null)
            masterMutedIcon.SetActive(value <= 0.0001f);

        if (parameter == "MusicVol" && musicMutedIcon != null)
            musicMutedIcon.SetActive(value <= 0.0001f);

        if (parameter == "SFXVol" && sfxMutedIcon != null)
            sfxMutedIcon.SetActive(value <= 0.0001f);
    }

    public void LoadAll()
    {
        foreach (var kvp in registeredSliders)
        {
            string parameter = kvp.Key;
            var (slider, text) = kvp.Value;

            float val = PlayerPrefs.GetFloat(parameter, 1f);
            slider.value = val;
            SetVolume(parameter, val, text);
        }
    }


    private void ToggleMasterMute()
    {
        ToggleMute("MasterVol", ref previousMaster, masterMutedIcon);
    }

    private void ToggleMusicMute()
    {
        ToggleMute("MusicVol", ref previousMusic, musicMutedIcon);
    }

    private void ToggleSFXMute()
    {
        ToggleMute("SFXVol", ref previousSFX, sfxMutedIcon);
    }

    private void ToggleMute(string parameter, ref float prevValue, GameObject muteIcon)
    {
        if (!registeredSliders.ContainsKey(parameter)) return;

        var (slider, text) = registeredSliders[parameter];

        if (slider.value > 0.0001f)
        {
            prevValue = slider.value;
            slider.value = 0f;
            SetVolume(parameter, 0f, text);
        }
        else 
        {
            slider.value = prevValue <= 0.0001f ? 1f : prevValue;
            SetVolume(parameter, slider.value, text);
        }

        if (muteIcon != null)
            muteIcon.SetActive(slider.value <= 0.0001f);
    }
}
