using System.Collections;

using UnityEngine;

public class DestroySpecificColliderOnTrigger : MonoBehaviour
{
    // Assignable in the Inspector: the collider that will trigger the destruction
    public Collider2D triggeringCollider;

    // The specific collider that will be destroyed when the trigger occurs
    public Collider2D targetColliderToDestroy;

    // This will be configurable in the Unity Inspector
    public float destroyDelay = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the triggering collider is the one that entered the collider
        if (other == triggeringCollider && targetColliderToDestroy != null)
        {
            // Start the coroutine to destroy the specific Collider2D component after the delay
            StartCoroutine(DestroyColliderAfterDelay());
        }
    }

    // Coroutine to destroy the specific Collider2D component after a delay
    private IEnumerator DestroyColliderAfterDelay()
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(destroyDelay);

        // Destroy the specified Collider2D component
        Destroy(targetColliderToDestroy);
        Debug.Log("collider is destory");
    }
}
