using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    TapDetection td;
    private Rigidbody2D rb;
    public float jumpForce = 06f;
    private int gravity = 1;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    public Vector2 initialShotVelocity;
    public Transform spawnPoint;
    public LaserScript projectilePrefab;

    bool pressedJump = false;
    public bool top;

    private bool isGrounded;

    public GameObject playerGoal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        td = GetComponent<TapDetection>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check if the player is on the ground
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
       
    }
    
    public void playerJump()
    {
        //rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
       // rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //rb.linearVelocityY =jumpForce;
        
    }

    public void playerNegativeJump()
    {
        // rb.linearVelocity = new Vector2(rb.linearVelocity.x, -jumpForce);
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector2.up * -jumpForce, ForceMode2D.Impulse);
        
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
            playerGoal.SetActive(true);
            //onGameOver();
        }
        if (collision.collider.CompareTag("EnemyProjectile"))
        {
            Destroy(gameObject);
            //run end game script here
            //onGameOver();
            Debug.Log("Game Over");
        }

    }

    public void snowBall()
    {
        Vector2 shotVelocity = initialShotVelocity;

        shotVelocity.x = Mathf.Abs(initialShotVelocity.x);
        LaserScript curProjectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        curProjectile.SetVelocity(shotVelocity);
    }

    public void onGameOver()
    {

    }
    //bool onGround()
    //{
    //    //return Physics2D.OverlapCircle(GroundCheckTransform.position, groundCheckRadius, GroundMask)
    //   // return Physics2D.OverlapBox(GroundCheckTransform.position, Vector2.right * 1.1f + Vector2.up * groundCheckRadius, 0, GroundMask)
    //}
}
