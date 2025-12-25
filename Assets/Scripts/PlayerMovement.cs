using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  [SerializeField] private float moveSpeed = 8f;
  [SerializeField] private float jumpForce = 15f;

  private Rigidbody2D rb;
  private bool isGrounded;

  private float xPosLastFrame;
    
  [SerializeField] private Animator animator; 
  [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
  {
    rb = GetComponent<Rigidbody2D>();
  }

  void Update()
  {
    Move();
    Jump();
    FlipCharacterX();
  }

  void Move()
  {
    float moveInput = Input.GetAxis("Horizontal");
    rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput != 0)
        {
            animator.SetBool("isRunning", true);

        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        
  }

  void Jump()
  {
    if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
    {
      rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("Ground"))
    {
      isGrounded = true;
    }
    if (collision.gameObject.CompareTag("Enemy"))
    {
      Debug.Log("Game Over");
      Time.timeScale = 0f;
    }
  }

  private void OnCollisionExit2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("Ground"))
    {
      isGrounded = false;
    }
  }

    private void FlipCharacterX()
    {
        if (transform.position.x > xPosLastFrame)
        {
            spriteRenderer.flipX = true;
        }
        else if (transform.position.x < xPosLastFrame)
        {
            spriteRenderer.flipX = false;
        }
        
        xPosLastFrame = transform.position.x;

    }
}
