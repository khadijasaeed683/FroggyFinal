using UnityEngine;

public class RotateBird : MonoBehaviour
{
    public Transform centerPoint; // The point around which the bird will rotate
    public float radius = 5.0f;   // The radius of the circular path
    public float speed = 1.0f;    // The speed of rotation

    private float angle; // Current angle of rotation

    void Start()
    {
        if (centerPoint == null)
        {
            // If no center point is specified, use the object's initial position as the center
            centerPoint = new GameObject("CenterPoint").transform;
            centerPoint.position = Vector3.zero;
        }

        // Initialize the angle based on the bird's current position
        Vector3 offset = transform.position - centerPoint.position;
        angle = Mathf.Atan2(offset.z+2, offset.x);
    }

    void Update()
    {
        // Update the angle based on the speed and time
        angle -= speed * Time.deltaTime; // Change to clockwise rotation

        // Calculate the new position using the angle and radius
        float x = centerPoint.position.x + Mathf.Cos(angle) * radius;
        float z = centerPoint.position.z + Mathf.Sin(angle) * radius;

        // Update the bird's position
        transform.position = new Vector3(x, transform.position.y, z);

        // Rotate the bird to face the direction of movement
        Vector3 direction = new Vector3(Mathf.Cos(angle+10), 0, Mathf.Sin(angle));
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
