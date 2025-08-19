using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField, Range(0.01f, 1)] private float groundCheckRadius = 0.02f;
    [SerializeField] private LayerMask groundCheckLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        if (!groundCheck)
        {
            Debug.Log("Hey, no ground check set - creating one assuming that the pivot is bottom center");
            GameObject newGameObject = new GameObject();
            newGameObject.transform.SetParent(transform);
            newGameObject.transform.localPosition = Vector3.zero;
            newGameObject.name = "GroundCheck";
            groundCheck = newGameObject.transform;
        }
    }
    public bool IsGrounded()
    {
        if (!groundCheck) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundCheckLayerMask);
    }
}
