using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 12f;
    private float climbSpeed = 5f;

    private Animator animator;
    private bool isDead = false; // To track if the player is dead
    private bool isClimbing = false; // To track if the player is climbing
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask ladderLayer;

    private PlayerInput playerInput;
    private InputAction jumpAction;
    private InputAction ducking;
    private InputAction climbAction;
    private InputAction climbDownAction; // New action for climbing down
    private InputAction climbRightAction; // New action for climbing right
    private InputAction climbLeftAction; // New action for climbing left

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        jumpAction = playerInput.actions["Jump"];
        ducking = playerInput.actions["Ducking"];
        climbAction = playerInput.actions["Climbing"];
        climbDownAction = playerInput.actions["Climb down"]; // Assign the climb down action
        climbRightAction = playerInput.actions["Climb right"]; // Assign the climb right action
        climbLeftAction = playerInput.actions["Climb left"]; // Assign the climb left action

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();  // Assign the Animator component
    }

    void Update()
    {
        if (isDead) return; // Prevent any updates if the player is dead

        if (isClimbing)
        {
            HandleClimbing();
        }
        else
        {
            OnJump();
            Flip();
            HandleJumping();
            HandleRunning();
            HandleDucking();
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return; // Prevent any movement if the player is dead

        if (!isClimbing)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnJump()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (jumpAction.IsPressed() && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (jumpAction.WasReleasedThisFrame() && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void HandleClimbing()
    {
        float vertical = climbAction.ReadValue<float>();
        float climbDown = climbDownAction.ReadValue<float>();
        float climbRight = climbRightAction.ReadValue<float>();
        float climbLeft = climbLeftAction.ReadValue<float>();

        if (vertical != 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, vertical * climbSpeed);
            rb.gravityScale = 0f; // Disable gravity while climbing
            animator.SetBool("isClimbing", true);
        }
        else if (climbDown != 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, climbDown * climbSpeed * -1); // Climb down
            rb.gravityScale = 0f; // Disable gravity while climbing down
            animator.SetBool("isClimbing", true);
        }
        else if (climbRight != 0f)
        {
            rb.velocity = new Vector2(climbRight * climbSpeed, rb.velocity.y); // Move right while climbing
            animator.SetBool("isClimbing", true);
        }
        else if (climbLeft != 0f)
        {
            rb.velocity = new Vector2(climbLeft * climbSpeed * -1, rb.velocity.y); // Move left while climbing
            animator.SetBool("isClimbing", true);
        }
        else
        {
            rb.velocity = new Vector2(0f, 0f); // Stop all movement
            animator.SetBool("isClimbing", false);
        }
    }

    private void HandleJumping()
    {
        bool grounded = isGrounded();

        if (!grounded)
        {
            animator.Play("Character-Jumping");
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }

    private void HandleRunning()
    {
        if (isGrounded() && Mathf.Abs(horizontal) > 0f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (isGrounded() && horizontal == 0f)
        {
            animator.SetBool("isIdle", true);
        }
        else
        {
            animator.SetBool("isIdle", false);
        }
    }

    private void HandleDucking()
    {
        if (ducking.IsPressed())
        {
            animator.Play("Character-Ducking");  // Start ducking animation
        }
        else if (ducking.WasReleasedThisFrame())
        {
            animator.SetBool("isDucking", false);  // Stop ducking animation
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ladder"))
        {
            isClimbing = true;
            rb.velocity = new Vector2(rb.velocity.x, 0f); // Stop any vertical momentum
            rb.gravityScale = 0f; // Disable gravity while on the ladder
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ladder"))
        {
            isClimbing = false;
            rb.gravityScale = 1f; // Re-enable gravity when leaving the ladder
            animator.SetBool("isClimbing", false); // Stop climbing animation
        }
    }

    private void Die()
    {
        isDead = true; // Mark the player as dead
        rb.velocity = Vector2.zero; // Stop the player from moving
        rb.isKinematic = true; // Make the Rigidbody kinematic so that physics won't affect it anymore
        animator.Play("Death"); // Play the death animation
        playerInput.enabled = false; // Disable player input
    }
}
