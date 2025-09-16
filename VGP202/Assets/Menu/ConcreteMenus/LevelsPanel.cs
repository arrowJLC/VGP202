using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsPanel : MonoBehaviour
{
    public Button[] levelButtons;
    public Button levelReset;
    public GameObject moreLevelButtons;

    private void Awake()
    {
        buttonsToArray();

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            levelButtons[i].interactable = true;
        }
    }
    public void OpenLevel(int levelId)
    {
        string levelName = "Level " + levelId;
        SceneManager.LoadScene(levelName);
    }
    
    void buttonsToArray()
    {
        int childCount = moreLevelButtons.transform.childCount;
        levelButtons = new Button[childCount];
        for (int i = 0; i < childCount; i++)
        {
            levelButtons[i] = moreLevelButtons.transform.GetChild(i).gameObject.GetComponent<Button>();
        }
    }

    //public static void ResetProgress()
    //{    
    //    PlayerPrefs.SetInt("UnlockedLevel", 1); 
    //    PlayerPrefs.SetInt("ReachedIndex", 0); 
    //    PlayerPrefs.Save();
    //}
}






























//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class LevelsPanel : MonoBehaviour
//{
//    public Button[] levelButtons;
//    public GameObject moreLevelButtons;
//    public Button resetProgressButton; // assign in inspector

//    private void Awake()
//    {
//        buttonsToArray();

//        // hook up reset button
//        if (resetProgressButton != null)
//            resetProgressButton.onClick.AddListener(ResetProgress);

//        UpdateLevelButtons();
//    }

//    public void OpenLevel(int levelId)
//    {
//        string levelName = "Level " + levelId;
//        SceneManager.LoadScene(levelName);
//    }

//    void buttonsToArray()
//    {
//        int childCount = moreLevelButtons.transform.childCount;
//        levelButtons = new Button[childCount];
//        for (int i = 0; i < childCount; i++)
//        {
//            levelButtons[i] = moreLevelButtons.transform.GetChild(i).gameObject.GetComponent<Button>();
//        }
//    }

//    void UpdateLevelButtons()
//    {
//        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

//        // lock all buttons
//        for (int i = 0; i < levelButtons.Length; i++)
//        {
//            levelButtons[i].interactable = false;
//        }

//        // unlock available ones
//        for (int i = 0; i < unlockedLevel; i++)
//        {
//            levelButtons[i].interactable = true;
//        }
//    }

//    void ResetProgress()
//    {
//        PlayerPrefs.SetInt("UnlockedLevel", 1);
//        PlayerPrefs.SetInt("ReachedIndex", 0);
//        PlayerPrefs.Save();
//        Debug.Log("Progress reset: only Level 1 unlocked.");

//        UpdateLevelButtons();
//    }
//}