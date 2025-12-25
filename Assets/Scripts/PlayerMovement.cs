using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  [Header("Movement Settings")]
  public float moveSpeed = 8f;
  public float jumpForce = 15f;
  public float bounceForce = 12f;

  [Header("Detection")]
  public Transform groundCheck;
  public float checkRadius = 0.4f; // 调大到 0.4 解决跳离感
  public LayerMask groundLayer;

  private Rigidbody2D rb;
  private bool isGrounded;
  private float moveInput;
  private bool jumpPressed;

  // 平台相关
  private Rigidbody2D currentPlatformRb;
  private Vector2 lastPlatformPos;
  private Vector2 platformVelocity;
  private bool onPlatform;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    // 建议在这里强制设置，防止编辑器里忘改
    rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    rb.gravityScale = 4f; // 提高重力感
  }

  void Update()
  {
    moveInput = Input.GetAxis("Horizontal");
    if (Input.GetKeyDown(KeyCode.Space)) jumpPressed = true;

    isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
  }

  void FixedUpdate()
  {
    // 1. 获取平台速度
    if (onPlatform && currentPlatformRb != null)
    {
      platformVelocity = (currentPlatformRb.position - lastPlatformPos) / Time.fixedDeltaTime;
      lastPlatformPos = currentPlatformRb.position;
    }
    else
    {
      platformVelocity = Vector2.zero;
    }

    // 2. 执行移动
    Move();

    // 3. 执行跳跃
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
      // 只要是在石头上（法线向上），就持续锁定平台
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

  // 踩头逻辑保持不变...
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