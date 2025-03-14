using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private bool isFacingRight = true;
    private float shootDirectionLockTimer = 0f;

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

        // ✅ Reduce shooting lock timer
        if (shootDirectionLockTimer > 0)
        {
            shootDirectionLockTimer -= Time.deltaTime;
        }
        else
        {
            // ✅ Restore facing based on movement after timer ends
            HandleMovementBasedFlipping();
        }
    }

    private void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // ✅ Flip based on movement only if NOT locked by shooting
        if (shootDirectionLockTimer <= 0)
        {
            if (moveInput > 0)
            {
                isFacingRight = true;
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (moveInput < 0)
            {
                isFacingRight = false;
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }
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

        anim.SetBool("isRunning", isMoving && isGrounded);
        anim.SetBool("isJumping", !isGrounded);
    }

    // ✅ Lock facing direction for shooting
    public void LockFacingDirection(float duration, bool facingRight)
    {
        shootDirectionLockTimer = duration;
        GetComponent<SpriteRenderer>().flipX = !facingRight;
    }

    private void HandleMovementBasedFlipping()
    {
        float moveInput = Input.GetAxis("Horizontal");

        if (moveInput > 0)
        {
            isFacingRight = true;
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (moveInput < 0)
        {
            isFacingRight = false;
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
