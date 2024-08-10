using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;
using UnityEngine.InputSystem;

public class PLayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 10f;
    private bool isFacingRight = true;
    private Vector2 moveInputValue;
    private Animator animator;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private PlayerInput playerInput;
    private InputAction jumpAction;
    private InputAction ducking;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        jumpAction = playerInput.actions["Jump"];
        ducking = playerInput.actions["Ducking"];
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();  // Assign the Animator component
    }

    void Update()
    {
        OnJump();
        Flip();
        HandleJumping();
        HandleRunning();
        HandleDucking();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
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
            animator.Play("Character-Ducking");  // Start jump animation
        }
        else if (ducking.WasReleasedThisFrame())
        {
            animator.SetBool("isDucking", false);  // Stop jump animation
        }
    }
}
