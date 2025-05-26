using UnityEngine;

public class GravityConroller : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerController pc;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.gravityScale *= -1;
        }
    }
}
