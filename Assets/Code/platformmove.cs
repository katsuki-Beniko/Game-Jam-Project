using UnityEngine;

public class platformmove : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;

    // Reference to the Animator component
    public Animator Sawanimation;

    // Speed of the object's movement, adjustable in the Inspector
    public float speed = 5f;

    // Direction of the movement, adjustable in the Inspector
    public Vector3 direction = Vector3.right;

    void Start()
    {
        // Play the "moving-platform-animation" when the script starts
        Sawanimation.Play("moving-platform-animation");
    }

    void Update()
    {
        // Move the object in the specified direction at the specified speed
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object collided with the ground layer
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            // Reverse the direction
            direction = -direction;
        }

        // Check if the player has collided with the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            // Make the player a child of the platform so it moves with the platform
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the player has exited the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            // Unparent the player when it leaves the platform
            collision.gameObject.transform.SetParent(null);
        }
    }
}
