//using System;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class Bootstrapper : Singleton<Bootstrapper>
//{
//    public InputManager InputManager {  get; private set; }

//    #region Bootstrap
//    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
//    public static void BootstrapGame() => CheckScene("Bootstrapper"); //menu
    
//    public static void CheckScene(string sceneName)
//    {
//        for (int i = 0; i < SceneManager.sceneCount; i++)
//        {
//            Scene scene = SceneManager.GetSceneAt(i);

//            if (scene.name == sceneName)
//                return;
//        }

//        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
//    }

//    #endregion

//    protected override void Awake()
//    {
//        base.Awake();

//        InputManager = gameObject.AddComponent<InputManager>();

//        CheckScene("Game");//infite
//    }
//}

