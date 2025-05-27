using TMPro;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class tempCanCon : MonoBehaviour
{
    public Button returnToMenu;

    void Start()
    {
       if (returnToMenu) returnToMenu.onClick.AddListener(() => SceneManager.LoadScene("Main Menu"));
    }
}
