using System;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine;

public class UnityAdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public string GAME_ID = "5943166";     //"3003911"; //replace with your gameID from dashboard. note: will be different for each platform.

    private string BANNER_PLACEMENT = "Banner_Android";
    private string VIDEO_PLACEMENT = "Interstital_Android";
    private string REWARDED_VIDEO_PLACEMENT = "Rewarded_Android";

    [SerializeField] private BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER;

    private bool testMode = true;
    private bool showBanner = false;

    //utility wrappers for debuglog
    public delegate void DebugEvent(string msg);
    public static event DebugEvent OnDebugLog;

    public void Initialize()
    {
        if (Advertisement.isSupported)
        {
            DebugLog(Application.platform + " supported by Advertisement");
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            GAME_ID = "5943167";

            BANNER_PLACEMENT = "Banner_iOS";
            VIDEO_PLACEMENT = "Interstital_iOS";
            REWARDED_VIDEO_PLACEMENT = "Rewarded_iOS";
        }

            Advertisement.Initialize(GAME_ID, testMode, this);
    }

    public void ToggleBanner() 
    {
        showBanner = !showBanner;

        if (showBanner)
        {
            Advertisement.Banner.SetPosition(bannerPosition);

            //not the best place to put this
            //BannerOptions bannerOptions = new BannerOptions
            //{
            //    clickCallback = () => DebugLog("Banner Clicked"),
            //    showCallback = () => DebugLog("Banner Shown"),
            //    hideCallback = () => DebugLog("Banner Hidden")
            //};

            Advertisement.Banner.Show(BANNER_PLACEMENT/*, bannerOptions*/);
        }
        else
        {
            Advertisement.Banner.Hide(false);
        }
    }

    public void LoadRewardedAd()
    {
        Advertisement.Load(REWARDED_VIDEO_PLACEMENT, this);
    }

    public void ShowRewardedAd()
    {
        //Advertisement.Show(REWARDED_VIDEO_PLACEMENT, this);
    }

    public void LoadNonRewardedAd()
    {
        Advertisement.Load(VIDEO_PLACEMENT, this);
    }

    public void ShowNonRewardedAd()
    {
        Advertisement.Show(VIDEO_PLACEMENT, this);
    }

    #region Interface Implementations
    public void OnInitializationComplete()
    {
        DebugLog("Init Success");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        DebugLog($"Init Failed: [{error}]: {message}");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        DebugLog($"Load Success: {placementId}");
        Advertisement.Show(REWARDED_VIDEO_PLACEMENT, this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        DebugLog($"Load Failed: [{error}:{placementId}] {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        DebugLog($"OnUnityAdsShowFailure: [{error}]: {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        DebugLog($"OnUnityAdsShowStart: {placementId}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        DebugLog($"OnUnityAdsShowClick: {placementId}");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        DebugLog($"OnUnityAdsShowComplete: [{showCompletionState}]: {placementId}");
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            DebugLog("Ad completed - grant reward to player");
            //grant reward
        }

        else if (showCompletionState == UnityAdsShowCompletionState.SKIPPED)
        {
            DebugLog("Ad was skipped - no reward granted");
        }

        else if (showCompletionState == UnityAdsShowCompletionState.UNKNOWN)
        {
            DebugLog("Ad did not complete - no reward");
        }
    }
    #endregion

    public void OnGameIDFieldChanged(string newInput)
    {
        GAME_ID = newInput;
    }

    public void ToggleTestMode(bool isOn)
    {
        testMode = isOn;
    }

    //wrapper around debug.log to allow broadcasting log strings to the UI
    void DebugLog(string msg)
    {
        OnDebugLog?.Invoke(msg);
        Debug.Log(msg);
    }
}
