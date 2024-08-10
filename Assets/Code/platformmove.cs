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
        // Play the "ActivateTrap" animation when the script starts
        Sawanimation.Play("moving-platform-animation");
    }

    void Update()
    {
        // Move the object in the specified direction at the specified speed
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object collided with a Tilemap that has the "Ground" layer
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            // Reverse the direction
            direction = -direction;
        }
    }
}
