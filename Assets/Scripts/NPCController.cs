using UnityEngine;

public class NPCController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    public float jumpForce = 10f;  // Zýplama kuvveti
    private bool isGrounded = false;
    private bool canJump = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // NPC otomatik zýplama
        if (canJump)
        {
            Jump();
        }
    }

    private void Jump()
    {
        animator.SetBool("IsJumping", true);
        animator.SetBool("IsGrounded", false);
        rb.velocity = Vector2.up * jumpForce;
        canJump = false;
    }

    private void PlayLandingAnimation()
    {
        animator.SetBool("IsGrounded", true);
        animator.SetBool("IsJumping", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && rb.velocity.y <= 0)
        {
            isGrounded = true;
            PlayLandingAnimation();
            Invoke("EnableJump", 0.583f); // Landing animasyonu süresi kadar bekle
        }
    }

    private void EnableJump()
    {
        if (isGrounded)
        {
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsGrounded", false);
        }
    }
}
