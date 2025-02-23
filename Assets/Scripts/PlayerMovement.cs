using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private int groundContacts = 0;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        Jump();
        HandleAnimations();
    }

    private void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Flip sprite based on movement direction
        if (moveInput > 0) GetComponent<SpriteRenderer>().flipX = false;
        else if (moveInput < 0) GetComponent<SpriteRenderer>().flipX = true;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void HandleAnimations()
    {
        bool isMoving = Mathf.Abs(rb.velocity.x) > 0.1f;

        // Handle shooting logic
        if (anim.GetBool("isShooting") && (isMoving || !isGrounded))
        {
            anim.SetBool("isShooting", false);
        }

        // Set animation parameters
        anim.SetBool("isRunning", isMoving && isGrounded);
        anim.SetBool("isJumping", !isGrounded);

        // Shooting input
        if (Input.GetKeyDown(KeyCode.F) && isGrounded && !isMoving)
        {
            anim.SetBool("isShooting", true);
            Invoke(nameof(ResetShooting), 0.2f);
        }
    }

    private void ResetShooting() => anim.SetBool("isShooting", false);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundContacts++;
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundContacts--;
            isGrounded = groundContacts > 0;
        }
    }
}