using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsPanel : MonoBehaviour
{
    public Button[] levelButtons;
    public GameObject moreLevelButtons;

    private void Awake()
    {
        buttonsToArray();

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 2);
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
    
}
