using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using System.Collections;

public class TapDetection : MonoBehaviour
{
    public PlayerController pc;
    //public PlayerControllerL playTwo;
    PauseMenu pause;
    
    float distThresold = 0.5f;
    float dirThresold = 0.9f;

    float tapTimeout = 0.2f;
    float swipeTimeout = 0.9f;
    float startTime = 0;
    float endTime = 0;

    Vector2 startPos;
    Vector2 endPos;

    public bool inputEnabled = true;
    private bool ignoreNextTouch = false;

    //23:52

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // im= Bootstrapper.Instance.InputManager;
        pc = GetComponent<PlayerController>();
        //playTwo = GetComponent<PlayerControllerL>();
        //playTwo = Object.FindFirstObjectByType<PlayerControllerL>();

        //PlayerController pu = Object.FindFirstObjectByType<PlayerController>();
         //pc = Object.FindFirstObjectByType<PlayerController>();

        GameMaster.Instance.OnPlayerSpawned += SetPlayer;
        
        StartCoroutine(AssignPlayerNextFrame());

        //var existingPlayer = GameObject.FindWithTag("Player");

        //if (existingPlayer != null)
        //{
        //    pc = existingPlayer.GetComponent<PlayerController>();
        //}

        InputManager.Instance.OnTouchBegin += TouchStarted;
        InputManager.Instance.OnTouchEnd += TouchEnded;
    }

    private void SetPlayer(PlayerController newPlayer)
    {
        pc = newPlayer;
    }

    private void TouchStarted()
    {
        if (!InputManager.Instance.IsInputEnabled()) return;
        startPos = InputManager.Instance.PrimaryPosition();
        startTime = Time.time;
        //startPos = InputManager.Instance.PrimaryPosition();
        //startTime = Time.time;
    }
    private void TouchEnded()
    {
        if (!InputManager.Instance.IsInputEnabled()) return;
        endTime = Time.time;
        endPos = InputManager.Instance.PrimaryPosition();
        DetectInput();
        //endTime = Time.time;
        //endPos = InputManager.Instance.PrimaryPosition();
        //DetectInput();
    }

    private void OnTouchStart()
    {
        if (ignoreNextTouch)
        {
            ignoreNextTouch = false; // consume the first touch after resume
            return;
        }

        startTime = Time.time;
        startPos = InputManager.Instance.PrimaryPosition();
    }

    private void OnTouchEnd()
    {
        if (ignoreNextTouch)
        {
            ignoreNextTouch = false; // consume the first touch after resume
            return;
        }

        endTime = Time.time;
        endPos = InputManager.Instance.PrimaryPosition();
        DetectInput();
    }
    public void ResetTouch()
    {
        startTime = 0f;
        endTime = 0f;
        startPos = Vector2.zero;
        endPos = Vector2.zero;
    }


    public void IgnoreNextTouch()
    {
        ignoreNextTouch = true;
    }

    private void DetectInput()
    {

        if (inputEnabled)
        {

            float totalTime = endTime - startTime;

            if (totalTime > swipeTimeout)
            {
                Debug.Log("Not Tap/Swipe");
                return;
            }

            if (totalTime < tapTimeout)
            {
                Tap();
                return;
            }

            else checkSwipe();
        }
    }

    public virtual void Tap()
    {
        Debug.Log($"Tap at {InputManager.Instance.PrimaryPosition()}");

       if (pc != null)
        {
            if (pc.top == false)
            {
                if (pc != null)
                {
                    pc.playerJump();
                }
            }

            if (pc.top != false)
            {
                if (pc != null)
                {
                    pc.playerNegativeJump();
                }
            }
        }
        
    }
    private void checkSwipe()
    {
        float distance = Vector2.Distance( startPos, endPos );
        if (distance < distThresold) return;

        Vector2 dir = (endPos - startPos).normalized;

        float checkDown = Vector2.Dot(Vector2.down, dir);
        float checkLeft = Vector2.Dot(Vector2.left, dir);

        if (checkDown >= dirThresold)
        {
            Debug.Log("Swipe Down");
            pc.RotationB();
            
            return;
        }

        if (checkDown <= -dirThresold)
        {
            Debug.Log($"Swipe Up: {checkDown}");
            pc.RotationT();
            return;
        }

        if (checkLeft >= dirThresold)
        {
            // may use for powerups later
            Debug.Log("Swipe Left");
            pc.snowBall();
            return;
        }

        if (checkLeft <= -dirThresold)
        {
            //may use for powerups later
            
            Debug.Log("Swipe Right");
            return;
        }
    }

    private void OnDestroy()
    {
        if (GameMaster.Instance != null)
            GameMaster.Instance.OnPlayerSpawned -= SetPlayer;

        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnTouchBegin -= TouchStarted;
            InputManager.Instance.OnTouchEnd -= TouchEnded;
        }
    }

    IEnumerator AssignPlayerNextFrame()
    {
        yield return new WaitForEndOfFrame(); // wait 1 frame

        if (pc == null)
        {
            var existingPlayer = GameObject.FindWithTag("Player");
            if (existingPlayer != null)
            {
                pc = existingPlayer.GetComponent<PlayerController>();
                Debug.Log("Fallback assigned player on Start.");
            }
        }
    }

}





//using UnityEngine;
//using UnityEngine.InputSystem.LowLevel;
//using UnityEngine.UI;
//using System.Collections;

//public class TapDetection : MonoBehaviour
//{
//    public PlayerController pc;
//    //public PlayerControllerL playTwo;
//    PauseMenu pause;

//    float distThresold = 0.5f;
//    float dirThresold = 0.9f;

//    float tapTimeout = 0.2f;
//    float swipeTimeout = 0.9f;
//    float startTime = 0;
//    float endTime = 0;

//    Vector2 startPos;
//    Vector2 endPos;

//    public bool inputEnabled = true;
//    private bool ignoreNextTouch = false;

//    //23:52

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        // im= Bootstrapper.Instance.InputManager;
//        pc = GetComponent<PlayerController>();
//        //playTwo = GetComponent<PlayerControllerL>();
//        //playTwo = Object.FindFirstObjectByType<PlayerControllerL>();

//        //PlayerController pu = Object.FindFirstObjectByType<PlayerController>();
//        //pc = Object.FindFirstObjectByType<PlayerController>();

//        GameMaster.Instance.OnPlayerSpawned += SetPlayer;

//        StartCoroutine(AssignPlayerNextFrame());

//        //var existingPlayer = GameObject.FindWithTag("Player");

//        //if (existingPlayer != null)
//        //{
//        //    pc = existingPlayer.GetComponent<PlayerController>();
//        //}

//        InputManager.Instance.OnTouchBegin += TouchStarted;
//        InputManager.Instance.OnTouchEnd += TouchEnded;
//    }

//    private void SetPlayer(PlayerController newPlayer)
//    {
//        pc = newPlayer;
//    }

//    //private void TouchStarted()
//    //{
//    //    if (!InputManager.Instance.IsInputEnabled()) return;
//    //    startPos = InputManager.Instance.PrimaryPosition();
//    //    startTime = Time.time;
//    //    //startPos = InputManager.Instance.PrimaryPosition();
//    //    //startTime = Time.time;
//    //}
//    //private void TouchEnded()
//    //{
//    //    if (!InputManager.Instance.IsInputEnabled()) return;
//    //    endTime = Time.time;
//    //    endPos = InputManager.Instance.PrimaryPosition();
//    //    DetectInput();
//    //    //endTime = Time.time;
//    //    //endPos = InputManager.Instance.PrimaryPosition();
//    //    //DetectInput();
//    //}

//    private void OnTouchStart()
//    {
//        if (ignoreNextTouch)
//        {
//            ignoreNextTouch = false; // consume the first touch after resume
//            return;
//        }

//        startTime = Time.time;
//        startPos = InputManager.Instance.PrimaryPosition();
//    }

//    private void OnTouchEnd()
//    {
//        if (ignoreNextTouch)
//        {
//            ignoreNextTouch = false; // consume the first touch after resume
//            return;
//        }

//        endTime = Time.time;
//        endPos = InputManager.Instance.PrimaryPosition();
//        DetectInput();
//    }
//    public void ResetTouch()
//    {
//        startTime = 0f;
//        endTime = 0f;
//        startPos = Vector2.zero;
//        endPos = Vector2.zero;
//    }


//    public void IgnoreNextTouch()
//    {
//        ignoreNextTouch = true;
//    }

//    private void DetectInput()
//    {

//        if (inputEnabled)
//        {

//            float totalTime = endTime - startTime;

//            if (totalTime > swipeTimeout)
//            {
//                Debug.Log("Not Tap/Swipe");
//                return;
//            }

//            if (totalTime < tapTimeout)
//            {
//                Tap();
//                return;
//            }

//            else checkSwipe();
//        }
//    }

//    public virtual void Tap()
//    {
//        Debug.Log($"Tap at {InputManager.Instance.PrimaryPosition()}");

//        if (pc != null)
//        {
//            if (pc.top == false)
//            {
//                if (pc != null)
//                {
//                    pc.playerJump();
//                }
//            }

//            if (pc.top != false)
//            {
//                if (pc != null)
//                {
//                    pc.playerNegativeJump();
//                }
//            }
//        }

//    }
//    private void checkSwipe()
//    {
//        float distance = Vector2.Distance(startPos, endPos);
//        if (distance < distThresold) return;

//        Vector2 dir = (endPos - startPos).normalized;

//        float checkDown = Vector2.Dot(Vector2.down, dir);
//        float checkLeft = Vector2.Dot(Vector2.left, dir);

//        if (checkDown >= dirThresold)
//        {
//            Debug.Log("Swipe Down");
//            pc.RotationB();

//            return;
//        }

//        if (checkDown <= -dirThresold)
//        {
//            Debug.Log($"Swipe Up: {checkDown}");
//            pc.RotationT();
//            return;
//        }

//        if (checkLeft >= dirThresold)
//        {
//            // may use for powerups later
//            Debug.Log("Swipe Left");
//            pc.snowBall();
//            return;
//        }

//        if (checkLeft <= -dirThresold)
//        {
//            //may use for powerups later

//            Debug.Log("Swipe Right");
//            return;
//        }
//    }

//    private void OnDestroy()
//    {
//        if (GameMaster.Instance != null)
//            GameMaster.Instance.OnPlayerSpawned -= SetPlayer;

//        if (InputManager.Instance != null)
//        {
//            InputManager.Instance.OnTouchBegin -= TouchStarted;
//            InputManager.Instance.OnTouchEnd -= TouchEnded;
//        }
//    }

//    IEnumerator AssignPlayerNextFrame()
//    {
//        yield return new WaitForEndOfFrame(); // wait 1 frame

//        if (pc == null)
//        {
//            var existingPlayer = GameObject.FindWithTag("Player");
//            if (existingPlayer != null)
//            {
//                pc = existingPlayer.GetComponent<PlayerController>();
//                Debug.Log("Fallback assigned player on Start.");
//            }
//        }
//    }

//}
