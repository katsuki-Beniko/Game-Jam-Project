using System.Collections;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    public Animator spikeAnimator;  // Reference to the spike's Animator component
    public Collider2D targetColliderToDestroy;
    public float destroyDelay = 2f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player has collided with the ani-trigger collider
        if (collision.CompareTag("Player"))
        {
            // Trigger the spike trap animation
            if (spikeAnimator != null)
            {
                spikeAnimator.SetTrigger("ActivateTrap");
                StartCoroutine(DestroyColliderAfterDelay());
            }
        }
    }
    private IEnumerator DestroyColliderAfterDelay()
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(destroyDelay);

        // Destroy the specified Collider2D component
        Destroy(targetColliderToDestroy);
        Debug.Log("collider is destory");
    }
}

