using UnityEngine;

public class ScreenOrientationm : MonoBehaviour
{
    void Start()
    {
        EnableAutoRotation();

        //// Force screen to portrait mode on scene load
        //Screen.orientation = ScreenOrientation.LandscapeLeft;

        //// (Optional) Disable auto-rotation to lock in portrait
        //Screen.autorotateToPortrait = false;
        //Screen.autorotateToPortraitUpsideDown = false;
        //Screen.autorotateToLandscapeLeft = true;
        //Screen.autorotateToLandscapeRight = true;
    }
    public void EnableAutoRotation()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }
}


