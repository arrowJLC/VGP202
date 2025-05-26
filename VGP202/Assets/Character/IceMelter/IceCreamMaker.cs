using System;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class IceCreamMaker : MonoBehaviour
{
    [SerializeField] private PlayerController playerPrefab;
    [SerializeField] private float fireRate = 2;
    public Vector2 initialShotVelocity;
    public Transform spawnPointLeft;
    public LaserScript projectilePrefab;
    private float shootDis = 20;
    private float timeSinceFire = 0;

    private void Start()
    {
        spawnPointLeft = GameObject.Find("FirePoint").transform;
        //playerPrefab = FindFirstObjectByType<PlayerController>();
    }
    private void Update()
    {
        float distance = Vector2.Distance (transform.position, playerPrefab.transform.position);

        if (distance <= shootDis)
        {
            //if needed later
            //sr.color = Color.red;

            if (Time.time >= timeSinceFire + fireRate)
            {
                iceMelter();
                timeSinceFire = Time.time;
            }
        }
        //else
        //    sr.color = Color.white;
    }

    private void iceMelter()
    {
        Vector2 shotVelocity = initialShotVelocity;

        shotVelocity.x = -Mathf.Abs(initialShotVelocity.x);
        LaserScript curProjectile = Instantiate(projectilePrefab, spawnPointLeft.position, spawnPointLeft.rotation);
        curProjectile.SetVelocity(shotVelocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border") || collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }
}
