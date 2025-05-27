using UnityEngine;

public class SnowmanMover : MonoBehaviour
{
    public float speed = 4f;
    public float resetPositionX = 10f; 
    public float startPositionX = -10f;

    void Update()
    {
        // Move object to the right over time
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // If it's past the reset position, move it back to the start
        if (transform.position.x > resetPositionX)
        {
            Vector3 newPosition = transform.position;
            newPosition.x = startPositionX;
            transform.position = newPosition;
        }
    }
}

