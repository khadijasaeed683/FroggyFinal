using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    public float waterLevel = 0.0f; // Set this to the y-coordinate of your water plane
    public float floatStrength = 10.0f; // Adjust this to change how strongly the object floats

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (transform.position.y < waterLevel)
        {
            float forceAmount = (waterLevel - transform.position.y) * floatStrength;
            rb.AddForce(Vector3.up * forceAmount);
        }
    }
}
