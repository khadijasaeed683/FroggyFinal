using UnityEngine;

public class DragAndShoot : MonoBehaviour
{
    public float power = 10f; // Force applied to the player
    private Vector3 startDragPosition; // Where the drag starts
    private Vector3 endDragPosition; // Where the drag ends
    private Rigidbody rb; // Rigidbody component of the player

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
    }

    void OnMouseDown()
    {
        startDragPosition = Input.mousePosition; // Record the starting position
    }

    void OnMouseUp()
    {
        endDragPosition = Input.mousePosition; // Record the ending position
        Vector3 force = (startDragPosition - endDragPosition) * power; // Calculate the force vector
        rb.AddForce(new Vector3(force.x, force.y, force.x)); // Apply the force to the player
    }
}
