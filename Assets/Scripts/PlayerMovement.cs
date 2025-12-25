using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public float moveSpeed = 8f;
  public float jumpForce = 15f;
  public float bounceForce = 12f;

  public Transform groundCheck;
  public float checkRadius = 0.2f;
  public LayerMask groundLayer;

  private Rigidbody2D rb;
  private bool isGrounded;

  private float moveInput;
  private bool jumpPressed;
  private Rigidbody2D currentPlatformRb;
  private Vector2 lastPlatformPos;
  private bool onPlatform;
  private Vector2 platformVelocity;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
  }

  void Update()
  {
    moveInput = Input.GetAxis("Horizontal");

    if (Input.GetKeyDown(KeyCode.Space))
      jumpPressed = true;

    isGrounded = Physics2D.OverlapCircle(
        groundCheck.position,
        checkRadius,
        groundLayer
    );
  }

  void FixedUpdate()
  {
    // 第一步：先更新平台数据
    UpdatePlatformData();
    // 第二步：执行移动（整合了平台速度）
    Move();
    // 第三步：跳跃
    Jump();
  }

  void UpdatePlatformData()
  {
    if (!onPlatform || currentPlatformRb == null)
    {
      platformVelocity = Vector2.zero;
      return;
    }

    // 计算平台的实时速度
    platformVelocity = (currentPlatformRb.position - lastPlatformPos) / Time.fixedDeltaTime;
    lastPlatformPos = currentPlatformRb.position;
  }

  void Move()
  {
    // 基础输入速度
    float targetXVelocity = moveInput * moveSpeed;

    // 如果在平台上，叠加平台的水平速度
    float finalX = targetXVelocity + platformVelocity.x;

    // Y轴速度逻辑
    float finalY = rb.linearVelocity.y;

    if (onPlatform && !jumpPressed)
    {
      // 关键：如果平台在动，强制同步Y速度以消除“跳离”感
      // 不管是上升还是下降，只要在平台上，我们就强制同步Y速度
      finalY = platformVelocity.y - 0.01f;
    }

    rb.linearVelocity = new Vector2(finalX, finalY);
  }

  void Jump()
  {
    if (jumpPressed && isGrounded)
    {
      Vector2 velocity = rb.linearVelocity;
      velocity.y = jumpForce;
      rb.linearVelocity = velocity;
    }

    jumpPressed = false;
  }

  void ApplyPlatformMovement()
  {
    if (!onPlatform || currentPlatformRb == null)
    {
      platformVelocity = Vector2.zero;
      return;
    }

    // 计算这一帧平台的真实物理速度
    platformVelocity = (currentPlatformRb.position - lastPlatformPos) / Time.fixedDeltaTime;

    // 同步位置差
    Vector2 platformDelta = currentPlatformRb.position - lastPlatformPos;
    rb.MovePosition(rb.position + platformDelta);

    lastPlatformPos = currentPlatformRb.position;
  }

  private void OnCollisionStay2D(Collision2D collision)
  {
    if (!collision.gameObject.CompareTag("MovingStone"))
      return;

    foreach (ContactPoint2D contact in collision.contacts)
    {
      if (contact.normal.y > 0.5f)
      {
        if (!onPlatform)
        {
          currentPlatformRb = collision.rigidbody;
          lastPlatformPos = currentPlatformRb.position;
          onPlatform = true;
        }
        return;
      }
    }
  }

  private void OnCollisionExit2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("MovingStone"))
    {
      currentPlatformRb = null;
      onPlatform = false;
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (!collision.gameObject.CompareTag("Enemy"))
      return;

    foreach (ContactPoint2D contact in collision.contacts)
    {
      if (contact.normal.y > 0.5f)
      {
        Bounce(collision.gameObject);
        return;
      }
    }

    GameManager.instance.TakeDamage(1);
  }

  void Bounce(GameObject enemy)
  {
    Vector2 velocity = rb.linearVelocity;
    velocity.y = bounceForce;
    rb.linearVelocity = velocity;

    Destroy(enemy);
    Debug.Log("Enemy destroyed!");
  }

  private void OnDrawGizmos()
  {
    if (groundCheck == null) return;

    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
  }
}
