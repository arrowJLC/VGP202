using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LaserScript : MonoBehaviour
{
    PlayerController pc;

    private void Start()
    {
        pc = GetComponent<PlayerController>();
        pc = FindAnyObjectByType(typeof(PlayerController)) as PlayerController;
    }
    public void SetVelocity(Vector2 velocity)
    {
        GetComponent<Rigidbody2D>().linearVelocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Border"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Block"))
        {  Destroy(gameObject);}

        if (collision.gameObject.CompareTag("Player") && CompareTag("EnemyProjectile"))
        {
            Destroy(gameObject);
            pc.onGameOver();
        }
    }

}
