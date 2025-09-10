using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("General")]
    TapDetection td;
    InGameMenu IGM;
    private Rigidbody2D rb;
    public MenuController currentMenuController;
    public LevelTimer levelTimer;

    private int gravity = 1;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public Vector2 initialShotVelocity;
    public Transform spawnPoint;
    public LaserScript projectilePrefab;
    GroundCheck gc;

    bool pressedJump = false;
    bool canJump = true;
    public bool top;

    private bool isGrounded = false;

   // public GameObject playerGoal;



    [Header("Levels")]
    private float playerLife = 100;
    private float projectileCount = 3;

    public Sprite[] frames;        // Assign in Inspector
    public float frameRate = 12f;  // Frames per second
    private SpriteRenderer sr;
    private int currentFrame;
    public float stepDuration = 1f;
    private int currentJ = 90;
    private bool isCountingDown = false;
    private Coroutine countdownCoroutine;

    [Header("Infinity")]

    public float jumpForce = 6f;
    

    [Header("Audio")]
    public AudioClip jumpSound;
    public AudioClip gravitySound;
    public AudioClip deathSound;
    public AudioClip fireSound;

    AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        td = GetComponent<TapDetection>();
        rb = GetComponent<Rigidbody2D>();
        gc = GetComponent<GroundCheck>();
        audioSource = GetComponent<AudioSource>();
        IGM = FindObjectOfType<InGameMenu>();
        levelTimer = FindAnyObjectByType<LevelTimer>();

        if (sceneName.StartsWith("Level"))
        {
            StartCountdown();

            Physics2D.gravity = new Vector2(0, -70f);
            jumpForce = 20;
        }

        if (sceneName == "Infinite")
        {
            Physics2D.gravity = new Vector2(0, -9.81f);
            jumpForce = 6;
        }
    }

    void Awake()
    {
       // string sceneName = SceneManager.GetActiveScene().name;

        sr = GetComponent<SpriteRenderer>();
        if (frames != null && frames.Length > 0)
            SetFrame(0);
    }

    void Update()
    {
        checkIsGrounded();
        // Check if the player is on the ground
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    currentMenuController.SetActiveState(MenuController.MenuStates.Pause);
        //}
    }

    private void checkIsGrounded()
    {
        //Debug.Log("OPlayer is grounded");
        //if (!isGrounded)
        //{
        //    Debug.Log("O");
        //    //if (rb.linearVelocity.y <= 0) isGrounded = gc.IsGrounded();
        //}
        //else isGrounded = gc.IsGrounded();

        bool groundedNow = gc.IsGrounded();

        if (groundedNow && !isGrounded)
        {
            Debug.Log("Player just landed");
        }
        else if (!groundedNow && isGrounded)
        {
            Debug.Log("Player just left the ground");
        }

        isGrounded = groundedNow;
    }

    private void FixedUpdate()
    {
        //if (sceneName.StartsWith("Level"))
        //{
        //    //Vector2 velocity = rb.linearVelocity;

        //    //velocity.x = moveSpeed;
        //    //rb.linearVelocity = Vector2.right * moveSpeed;
        //}

        // transform.position += Vector3.right * 9.5f *Time.deltaTime
    }

    public void StartCountdown()
    {
        if (countdownCoroutine != null) StopCoroutine(countdownCoroutine);
        isCountingDown = true;
        countdownCoroutine = StartCoroutine(CountdownCoroutine());
    }

    public void SetCurrentJ(int value)
    {
        currentJ = Mathf.Clamp(value, 0, 90);
        UpdateFrameFromJ();
    }

    // Coroutine driving the countdown
    private IEnumerator CountdownCoroutine()
    {
        while (currentJ > 0)
        {
            UpdateFrameFromJ();

            yield return new WaitForSeconds(stepDuration);

            currentJ--;
        }

        // Reached 0 - player death logic here
        UpdateFrameFromJ(); // Final frame update (probably death frame)
        Debug.Log("Player Death");
        isCountingDown = false;
    }

    // Calculate which frame corresponds to currentJ, then display it
    private void UpdateFrameFromJ()
    {
        if (frames == null || frames.Length == 0) return;

        // 18 segments from 90 down to 0 means each segment covers 5 steps (90 / 18 = 5)
        // So map currentJ to frame index:
        int segmentSize = 5;

        int segmentIndex = (90 - currentJ) / segmentSize; // 0 to 17

        // Clamp to last frame if somehow out of bounds
        segmentIndex = Mathf.Clamp(segmentIndex, 0, frames.Length - 1);

        SetFrame(segmentIndex);
    }

    // Set the displayed frame by index
    private void SetFrame(int frameIndex)
    {
        currentFrame = frameIndex;
        sr.sprite = frames[currentFrame];
    }

    public void playerJump()
    {
        if (canJump )//&& isGrounded)
        {
            //rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            // rb.linearVelocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //rb.linearVelocityY =jumpForce;

            isGrounded = false;
            audioSource.PlayOneShot(jumpSound);
        }
    }

    public void playerNegativeJump()
    {
        if (canJump)
        {
            // rb.linearVelocity = new Vector2(rb.linearVelocity.x, -jumpForce);
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(Vector2.up * -jumpForce, ForceMode2D.Impulse);
            audioSource.PlayOneShot(jumpSound);
        }
    }

    public void RotationB()
    {
        Debug.Log("B");
        if (top == true)
        {
            top = false;
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            rb.gravityScale = Mathf.Abs(rb.gravityScale) * gravity;
            //rb.gravityScale = 1;
            audioSource.PlayOneShot(gravitySound);
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }
        //rb.facingRight = 
        //top = !top;
    }

    public void RotationT()
    {
        Debug.Log("T");
        if (top == false)
        {
            top = true;
            transform.eulerAngles = new Vector3(0f, 180f, -180f);
            rb.gravityScale = -1;
            audioSource.PlayOneShot(gravitySound);

        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }
        //rb.facingRight = 
        //top = !top;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Block"))
        {
            Debug.Log("player hit ob");
            // fix death sound not playing
            audioSource.PlayOneShot(deathSound);
            Destroy(gameObject, deathSound.length);
            //playerGoal.SetActive(true);
            //onGameOver();
            //IGM.OnDeath();
            levelTimer.stopTimer(); // Stop the timer
            string finalTime = levelTimer.getTime();

            IGM.OnDeath(finalTime);
        }
        if (collision.collider.CompareTag("EnemyProjectile"))
        {
            audioSource.PlayOneShot(deathSound);
            Destroy(gameObject, deathSound.length);

            levelTimer.stopTimer(); // Stop the timer
            string finalTime = levelTimer.getTime();

            IGM.OnDeath(finalTime);

            Debug.Log("Game Over");

            // add method so ads arnt spam
            UnityAdsManager ads = FindFirstObjectByType<UnityAdsManager>();
            if (ads != null)
            {
                ads.LoadNonRewardedAd();
                ads.ShowNonRewardedAd();
            }
        }

        //if (collision.collider.CompareTag("Finish"))
        //{

        //}

        //if (collision.collider.CompareTag("SnowPatch"))
        //{
        //    //StartCoroutine(hasBeenLongenogh());

        //}

    }

    public void snowBall()
    {
        audioSource.PlayOneShot(fireSound);
        Vector2 shotVelocity = initialShotVelocity;

        shotVelocity.x = Mathf.Abs(initialShotVelocity.x);
        LaserScript curProjectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        curProjectile.SetVelocity(shotVelocity);
    }

    public void onGameOver()
    {

        //Destroy(gameObject, deathSound.length);
        SceneManager.LoadScene("GameOver");
    }

    public void OnResumeGame()
    {
        // Clear jump input or debounce logic here
        canJump = false;
        StartCoroutine(EnableJumpAfterDelay());
    }

    private IEnumerator EnableJumpAfterDelay()
    {
        yield return new WaitForSecondsRealtime(0.5f); // Slight delay after resume
        canJump = true;
    }
    //bool onGround()
    //{
    //    //return Physics2D.OverlapCircle(GroundCheckTransform.position, groundCheckRadius, GroundMask)
    //   // return Physics2D.OverlapBox(GroundCheckTransform.position, Vector2.right * 1.1f + Vector2.up * groundCheckRadius, 0, GroundMask)
    //}
}
