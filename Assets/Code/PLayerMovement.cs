using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // For scene management
using UnityEngine.InputSystem;
using UnityEngine.UI;  // For UI elements

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 12f;
    private float climbSpeed = 5f;

    private Animator animator;
    private bool isDead = false;
    private bool isClimbing = false;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask ladderLayer;

    private PlayerInput playerInput;
    private InputAction jumpAction;
    private InputAction ducking;
    private InputAction climbAction;
    private InputAction climbDownAction;
    private InputAction climbRightAction;
    private InputAction climbLeftAction;

    // Audio variables
    private AudioSource audioSource;
    public AudioClip steppingSound;  // Reference to the stepping sound
    public AudioClip deathSound;  // Reference to the death sound

    // UI elements
    public GameObject deathPanel;  // Panel that contains the death prompt

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        jumpAction = playerInput.actions["Jump"];
        ducking = playerInput.actions["Ducking"];
        climbAction = playerInput.actions["Climbing"];
        climbDownAction = playerInput.actions["Climb down"];
        climbRightAction = playerInput.actions["Climb right"];
        climbLeftAction = playerInput.actions["Climb left"];

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();  // Get the AudioSource component

        // Ensure the death panel is hidden at the start
        deathPanel.SetActive(false);
    }

    void Update()
    {
        if (isDead) return;

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
        if (isDead) return;

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
            rb.gravityScale = 0f;
            animator.SetBool("isClimbing", true);
        }
        else if (climbDown != 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, climbDown * climbSpeed * -1);
            rb.gravityScale = 0f;
            animator.SetBool("isClimbing", true);
        }
        else if (climbRight != 0f)
        {
            rb.velocity = new Vector2(climbRight * climbSpeed, rb.velocity.y);
            animator.SetBool("isClimbing", true);
        }
        else if (climbLeft != 0f)
        {
            rb.velocity = new Vector2(climbLeft * climbSpeed * -1, rb.velocity.y);
            animator.SetBool("isClimbing", true);
        }
        else
        {
            rb.velocity = new Vector2(0f, 0f);
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

            // Play the stepping sound if not already playing
            if (!audioSource.isPlaying)
            {
                audioSource.clip = steppingSound;
                audioSource.Play();
            }
        }
        else
        {
            animator.SetBool("isRunning", false);

            // Stop the stepping sound when not running
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
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
            animator.Play("Character-Ducking");
        }
        else if (ducking.WasReleasedThisFrame())
        {
            animator.SetBool("isDucking", false);
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
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.gravityScale = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ladder"))
        {
            isClimbing = false;
            rb.gravityScale = 1f;
            animator.SetBool("isClimbing", false);
        }
    }

    private void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        animator.Play("Death");
        playerInput.enabled = false;
        audioSource.Stop(); // Stop any sound effects when the player dies
        audioSource.PlayOneShot(deathSound);  // Play death sound

        // Activate the death panel to show the prompt
        deathPanel.SetActive(true);
    }

    // Button methods
 
}
