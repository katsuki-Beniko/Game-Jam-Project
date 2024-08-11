using UnityEngine;

public class Jumper : MonoBehaviour
{
    private Animator animator;
    public float jumpForce = 20f; // Adjust this value to control how high the player jumps

    private void Awake()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();

        // Check if the Animator component is assigned
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on this GameObject.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player has collided with the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the Rigidbody2D component of the player
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (animator != null)
            {
                // Play the animation
                animator.SetBool("ishitting", true);
            }

            if (playerRb != null)
            {
                // Apply an upward force to the player
                playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the player has exited the collision with the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            if (animator != null)
            {
                // Stop the animation
                animator.SetBool("ishitting", false);
            }
        }
    }
}
