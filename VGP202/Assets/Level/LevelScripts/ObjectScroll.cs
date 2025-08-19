//using System;
//using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D), (typeof(BoxCollider2D)))]

//public class ObjectScroll : MonoBehaviour
//{
//    public enum levelSpeed { slow = 2, normal = 4, fast = 5, insane = 7 }

//    levelSpeed currentSpeed = levelSpeed.normal;
    


//    Rigidbody2D rb;
//    BoxCollider2D bc;

//    void Awake()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        bc = GetComponent<BoxCollider2D>();

//        rb.bodyType = RigidbodyType2D.Kinematic;
//    }

//    public float GetExtent() // => bc.bounds.extents.x;
//    {
//        return bc.bounds.extents.x;
//    }

//    public Vector2 GetTopLeftCorner() => new Vector2(bc.bounds.min.x, bc.bounds.max.y);

//    public Vector2 GetBottomLeftCorner() => new Vector2(bc.bounds.min.x, bc.bounds.min.y);

//    public Vector2 GetBottomRightCorner() => new Vector2(bc.bounds.max.x, bc.bounds.min.y);

//    public Vector2 GetTopRightCorner() => new Vector2(bc.bounds.max.x, bc.bounds.max.y);

//    void FixedUpdate()
//    {
//        //currentSpeed = levelSpeed.fast;

//        rb.MovePosition(transform.position + (Vector3.left * ((float)currentSpeed * Time.fixedDeltaTime)));
//    }
//    private void LateUpdate()
//    {
//        if (transform.position.x <= -15)
//        {
//            CleanUpPiece();
//        }
//    }

//    private void CleanUpPiece()
//    {
//        LevelGen.Instance.spawnLevelPiece();
//        LevelGen.Instance.currentPieces.Remove(this);
//        Destroy(gameObject);    
//    }
//}
