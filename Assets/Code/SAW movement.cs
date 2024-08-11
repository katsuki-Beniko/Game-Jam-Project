using System.Collections;
using UnityEngine;

public class TriggerAreaSAW : MonoBehaviour
{       // Reference to the spike's Animator component
    public Animator sawAnimator;               // Reference to the Saw's Animator component
    public GameObject saw;                     // Reference to the saw object that will move
    public float sawSpeed = 5f;                // Speed of the saw's movement
    public Vector3 sawDirection = Vector3.right; // Direction of the saw's movement
    public float Wait = 0.5f;
    private bool isTriggered = false;          // Flag to check if the trigger has been activated

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player has collided with the trigger collider
        if (collision.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;

            // Start the saw movement
            StartCoroutine(StartSawMovement());

        }
    }

    private IEnumerator StartSawMovement()
    {
        // Play the saw's animation
        if (sawAnimator != null)
        {
            sawAnimator.Play("Saw-ani");
        }

        // Wait for a brief moment before the saw starts moving (optional)
        yield return new WaitForSeconds(Wait);

        // Move the saw object
        while (true)
        {
            saw.transform.Translate(sawDirection.normalized * sawSpeed * Time.deltaTime);
            yield return null;
        }
    }
}

