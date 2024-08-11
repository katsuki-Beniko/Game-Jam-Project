using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlow : MonoBehaviour
{
    private Animator animator;
    // Force with which the fan will blow the player
    [SerializeField] private float blowForce = 10f;

    // Direction in which the fan will blow the player
    [SerializeField] private Vector2 blowDirection = Vector2.right;

    // Time between each force application (to simulate continuous force)
    [SerializeField] private float forceInterval = 0.1f;

    private bool isPlayerInRange = false;
    private Rigidbody2D playerRb;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                isPlayerInRange = true;
                StartCoroutine(ApplyBlowForce());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            StopCoroutine(ApplyBlowForce());
        }
    }

    private IEnumerator ApplyBlowForce()
    {
        while (isPlayerInRange)
        {
            animator.Play("Fan-ani");
            // Apply force in the specified direction
            playerRb.AddForce(blowDirection.normalized * blowForce, ForceMode2D.Force);

            // Wait for the next interval before applying force again
            yield return new WaitForSeconds(forceInterval);
        }
    }

    // Optionally, visualize the fan's blow direction and area in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)blowDirection.normalized * 2f);
    }
}
