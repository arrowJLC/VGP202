//using System;
//using System.Collections;
//using Unity.VisualScripting;
//using UnityEngine;
//using static TileLevelGen;


//public class PlayerControllerL : MonoBehaviour
//{

//    TapDetection TapDetection;
//    private Rigidbody2D rb;
//    Animator anim;

//    public float jumpForce = 06f;
//    public float moveSpeed;
//    private int gravity = 1;
//    public LayerMask groundLayer;
//    public Transform groundCheck;
//    public float groundCheckRadius = 0.2f;
//    public Vector2 initialShotVelocity;
//    public Transform spawnPoint;
//    public LaserScript projectilePrefab;

//    private float playerLife = 100;
//    private float projectileCount = 3;

  
//    public Sprite[] frames;        // Assign in Inspector
//    public float frameRate = 12f;  // Frames per second
//    private SpriteRenderer sr;
//    private int currentFrame;

//    [Header("Audio")]
//    public AudioClip jumpSound;
//    public AudioClip gravitySound;
//    public AudioClip deathSound;
//    public AudioClip fireSound;

//    AudioSource audioSource;

//    bool pressedJump = false;
//    bool canJump = true;
//    public bool top;

//    private bool isGrounded;

//    public GameObject playerGoal;

//    public MenuController currentMenuController;
//    InGameMenu IGM;
//    public bool hasAwakeded = false;

//    // public Sprite[] frames;            // Assign your 18 frames here
//    public float stepDuration = 1f;   // Seconds per countdown step (adjust speed)

//    //private SpriteRenderer sr;
//   // private int currentFrame = 0;

//    // Countdown variables
//    private int currentJ = 90;
//    private bool isCountingDown = false;
//    private Coroutine countdownCoroutine;

//    void Awake()
//    {
//        sr = GetComponent<SpriteRenderer>();
//        if (frames != null && frames.Length > 0)
//            SetFrame(0);
//    }

//    // Start automatic countdown
//    public void StartCountdown()
//    {
//        if (countdownCoroutine != null) StopCoroutine(countdownCoroutine);
//        isCountingDown = true;
//        countdownCoroutine = StartCoroutine(CountdownCoroutine());
//    }

//    // Pause countdown
//    public void PauseCountdown()
//    {
//        isCountingDown = false;
//        if (countdownCoroutine != null)
//            StopCoroutine(countdownCoroutine);
//    }

//    // Resume countdown
//    public void ResumeCountdown()
//    {
//        if (!isCountingDown)
//        {
//            isCountingDown = true;
//            countdownCoroutine = StartCoroutine(CountdownCoroutine());
//        }
//    }

//    // Set the countdown to a specific value (0-90)
//    public void SetCurrentJ(int value)
//    {
//        currentJ = Mathf.Clamp(value, 0, 90);
//        UpdateFrameFromJ();
//    }

//    // Coroutine driving the countdown
//    private IEnumerator CountdownCoroutine()
//    {
//        while (currentJ > 0)
//        {
//            UpdateFrameFromJ();

//            yield return new WaitForSeconds(stepDuration);

//            currentJ--;
//        }

//        // Reached 0 - player death logic here
//        UpdateFrameFromJ(); // Final frame update (probably death frame)
//        Debug.Log("Player Death");
//        isCountingDown = false;
//    }

//    // Calculate which frame corresponds to currentJ, then display it
//    private void UpdateFrameFromJ()
//    {
//        if (frames == null || frames.Length == 0) return;

//        // 18 segments from 90 down to 0 means each segment covers 5 steps (90 / 18 = 5)
//        // So map currentJ to frame index:
//        int segmentSize = 5;

//        int segmentIndex = (90 - currentJ) / segmentSize; // 0 to 17

//        // Clamp to last frame if somehow out of bounds
//        segmentIndex = Mathf.Clamp(segmentIndex, 0, frames.Length - 1);

//        SetFrame(segmentIndex);
//    }

//    // Set the displayed frame by index
//    private void SetFrame(int frameIndex)
//    {
//        currentFrame = frameIndex;
//        sr.sprite = frames[currentFrame];
//    }



//void Start()
//    {
//        TapDetection = GetComponent<TapDetection>();
//        rb = GetComponent<Rigidbody2D>();
//        audioSource = GetComponent<AudioSource>();
//        IGM = FindObjectOfType<InGameMenu>();
//        anim = GetComponent<Animator>();

//        StartCountdown();

//        hasAwakeded = true;
//    }


//// Update is called once per frame
//    void Update()
//    {
//       //maybe incress garivty to like 70 or so
//       //movespeed 9.5
//    }

//    private void FixedUpdate()
//    {
//        //Vector2 velocity = rb.linearVelocity;

//        //velocity.x = moveSpeed;
//        //rb.linearVelocity = Vector2.right * moveSpeed;
//    }

//    public void playerJump()
//    {
//        if (canJump)
//        {
//            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
//            audioSource.PlayOneShot(jumpSound);
//        }
//    }

//    public void playerNegativeJump()
//    {
//        if (canJump)
//        {
//            rb.linearVelocity = Vector2.zero;
//            rb.AddForce(Vector2.up * -jumpForce, ForceMode2D.Impulse);
//            audioSource.PlayOneShot(jumpSound);
//        }
//    }

//    public void RotationB()
//    {
//        if (top == true)
//        {
//            top = false;
//            transform.eulerAngles = new Vector3(0f, 0f, 0f);
//            rb.gravityScale = Mathf.Abs(rb.gravityScale) * gravity;
//            audioSource.PlayOneShot(gravitySound);
//        }
//        else transform.eulerAngles = Vector3.zero;
//    }

//    public void RotationT()
//    {
//        if (top == false)
//        {
//            top = true;
//            transform.eulerAngles = new Vector3(0f, 180f, -180f);
//            rb.gravityScale = -1;
//            audioSource.PlayOneShot(gravitySound);

//        }
//        else transform.eulerAngles = Vector3.zero;
        

//    }
//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.collider.CompareTag("Block"))
//        {
//            audioSource.PlayOneShot(deathSound);
//            Destroy(gameObject, deathSound.length);
//        }
//        if (collision.collider.CompareTag("EnemyProjectile"))
//        {
//            audioSource.PlayOneShot(deathSound);
//            Destroy(gameObject, deathSound.length);
//        }
//    }

//    public void snowBall()
//    {
//      if (projectileCount <=1)
//        {
//            audioSource.PlayOneShot(fireSound);
//            Vector2 shotVelocity = initialShotVelocity;

//            shotVelocity.x = Mathf.Abs(initialShotVelocity.x);
//            LaserScript curProjectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
//            curProjectile.SetVelocity(shotVelocity);
//        }
//    }

//    public void onGameOver()
//    {

//    }

//    public void OnResumeGame()
//    {
//        canJump = false;
//        StartCoroutine(EnableJumpAfterDelay());
//    }



//    private IEnumerator EnableJumpAfterDelay()
//    {
//        yield return new WaitForSecondsRealtime(0.5f); // Slight delay after resume
//        canJump = true;
//    }
//}







////// Go to next frame (loops around)
////public void NextFrame()
////{
////    currentFrame = (currentFrame + 1) % frames.Length;
////    sr.sprite = frames[currentFrame];
////}

////// Go to previous frame (loops around)
////public void PreviousFrame()
////{
////    currentFrame = (currentFrame - 1 + frames.Length) % frames.Length;
////    sr.sprite = frames[currentFrame];
////}

//// Change animation at runtime
////public void SetAnimation(Sprite[] newFrames, float newRate = 12f)
////{
////    frames = newFrames;
////    frameRate = newRate;
////    currentFrame = 0;
////    if (frames.Length > 0)
////        sr.sprite = frames[0];
////}




////private void startPlayerDeath()
////{
////    //SpriteAnimator spriteAnimator = GetComponent<SpriteAnimator>();

////    //for (int j = 90; j >= 0; j--)
////    //{
////    //    if (j >= 75)
////    //    {
////    //        // range 90–75

////    //    }
////    //    else if (j >= 60)
////    //    {
////    //        // range 74–60
////    //    }
////    //    else if (j >= 45)
////    //    {
////    //        // range 59–45
////    //    }
////    //    else if (j >= 30)
////    //    {
////    //        // range 44–30
////    //    }
////    //    else if (j >= 15)
////    //    {
////    //        // range 29–15
////    //    }
////    //    else if (j >= 1)
////    //    {
////    //        // range 14–1
////    //    }
////    //    else
////    //    {
////    //        // j == 0 -> player death
////    //    }
////    //}
////}
//////priteAnimator.SetFrame(0);
//////public void SetFrame(int frameIndex)
//////{
//////    if (frames == null || frames.Length == 0) return;
//////    frameIndex = Mathf.Clamp(frameIndex, 0, frames.Length - 1);
//////    currentFrame = frameIndex;
//////    sr.sprite = frames[currentFrame];
//////}

//////    SetCurrentJ(currentJ + 3);

//////    using UnityEngine;
//////using System.Collections;

//////public class SpriteAnimatorCountdown : MonoBehaviour
//////{