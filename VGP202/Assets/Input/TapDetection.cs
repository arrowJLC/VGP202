using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using System.Collections;

public class TapDetection : MonoBehaviour
{
    public PlayerController pc;
    Rigidbody2D rb;
    InputManager im;
    
    float distThresold = 0.5f;
    float dirThresold = 0.9f;

    float tapTimeout = 0.2f;
    float swipeTimeout = 0.9f;
    float startTime = 0;
    float endTime = 0;

    Vector2 startPos;
    Vector2 endPos;
    
    //23:52
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // im= Bootstrapper.Instance.InputManager;
        pc = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

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
        startPos = InputManager.Instance.PrimaryPosition();
        startTime = Time.time;
    }
    private void TouchEnded()
    {
        endTime = Time.time;
        endPos = InputManager.Instance.PrimaryPosition();
        DetectInput();
    }

    private void DetectInput()
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

    public virtual void Tap()
    {
        Debug.Log($"Tap at {InputManager.Instance.PrimaryPosition()}");

        if (pc.top == false)
        {
            if (pc != null)
            {
                pc.playerJump();
            }
        }
      
        if(pc.top != false)
        {
            if(pc != null)
            {
                pc.playerNegativeJump();
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
