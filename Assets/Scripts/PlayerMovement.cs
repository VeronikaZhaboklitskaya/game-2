using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  [Header("Movement Settings")]
  public float moveSpeed = 8f;
  public float jumpForce = 15f;
  public float bounceForce = 12f;

  [Header("Detection")]
  public Transform groundCheck;
  public float checkRadius = 0.4f;
  public LayerMask groundLayer;

  private Rigidbody2D rb;
  private bool isGrounded;
  private float moveInput;
  private bool jumpPressed;

  private Rigidbody2D currentPlatformRb;
  private Vector2 lastPlatformPos;
  private Vector2 platformVelocity;
  private bool onPlatform;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    rb.gravityScale = 4f;
  }

  void Update()
  {
    moveInput = Input.GetAxis("Horizontal");
    if (Input.GetKeyDown(KeyCode.Space)) jumpPressed = true;

    isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
  }

  void FixedUpdate()
  {
    if (onPlatform && currentPlatformRb != null)
    {
      platformVelocity = (currentPlatformRb.position - lastPlatformPos) / Time.fixedDeltaTime;
      lastPlatformPos = currentPlatformRb.position;
    }
    else
    {
      platformVelocity = Vector2.zero;
    }

    Move();

    if (jumpPressed)
    {
      if (isGrounded)
      {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
      }
      jumpPressed = false;
    }
  }

  void Move()
  {
    float targetX = (moveInput * moveSpeed) + platformVelocity.x;
    float targetY = rb.linearVelocity.y;

    if (onPlatform && !jumpPressed && rb.linearVelocity.y <= 0.1f)
    {
      targetY = platformVelocity.y - 0.05f;
    }

    rb.linearVelocity = new Vector2(targetX, targetY);
  }

  private void OnCollisionStay2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("MovingStone"))
    {
      if (collision.contacts[0].normal.y > 0.5f)
      {
        currentPlatformRb = collision.rigidbody;
        if (!onPlatform)
        {
          lastPlatformPos = currentPlatformRb.position;
          onPlatform = true;
        }
      }
    }
  }

  private void OnCollisionExit2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("MovingStone"))
    {
      onPlatform = false;
      currentPlatformRb = null;
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("Enemy"))
    {
      if (collision.contacts[0].normal.y > 0.5f)
      {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
        Destroy(collision.gameObject);
      }
      else
      {
        GameManager.instance.TakeDamage(1);
      }
    }
  }
}