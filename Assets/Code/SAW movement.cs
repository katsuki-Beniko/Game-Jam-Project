using UnityEngine;

public class SAWmovement : MonoBehaviour
{
    // Reference to the Animator component
    public Animator Sawanimation;

    // Speed of the object's movement, adjustable in the Inspector
    public float speed = 5f;

    // Direction of the movement, adjustable in the Inspector
    public Vector3 direction = Vector3.right;

    void Start()
    {
        // Play the "ActivateTrap" animation when the script starts
        Sawanimation.Play("Saw-ani");
    }

    void Update()
    {
        // Move the object in the specified direction at the specified speed
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }
}
