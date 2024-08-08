using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    [SerializeField ] GameObject Particles;
    public float destryTime ;
    // This function is called when another collider enters the trigger collider attached to the object this script is attached to.
    void OnCollisionEnter(Collision collision)
    {
        // Check if the object colliding with this one is the player.
        if (collision.gameObject.tag == "Player")
        {
            // Destroy this game object after certian time .
            Destroy(gameObject, destryTime);
             
            }        
    }
}
