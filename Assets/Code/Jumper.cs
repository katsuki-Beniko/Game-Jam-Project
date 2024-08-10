using UnityEngine;

public class Jumper : MonoBehaviour
{
    public float jumpForce = 20f; // Adjust this value to control how high the player jumps

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player has collided with the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the Rigidbody2D component of the player
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Apply an upward force to the player
                playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
            }
        }
    }
}

