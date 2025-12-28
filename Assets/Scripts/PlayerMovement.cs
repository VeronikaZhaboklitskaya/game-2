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
  public float climbSpeed = 5f;
  private bool isAttachedToTree;
  private Rigidbody2D currentTreeRb;
  private float originalGravityScale;


  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    rb.gravityScale = 8f;
    originalGravityScale = rb.gravityScale;
  }

  void Update()
  {
    moveInput = Input.GetAxis("Horizontal");
    if (Input.GetKeyDown(KeyCode.Space)) jumpPressed = true;

    isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

    if (isAttachedToTree && jumpPressed)
    {
      DetachFromTree();
    }
  }

  void FixedUpdate()
  {
    if (isAttachedToTree)
    {
      ApplyTreeAttachment();
      return;
    }

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

    float finalX = (moveInput * moveSpeed) + platformVelocity.x;
    float finalY = rb.linearVelocity.y;

    if (onPlatform && !jumpPressed && rb.linearVelocity.y <= 0.1f)
    {
      finalY = platformVelocity.y - 0.05f;
    }

    rb.linearVelocity = new Vector2(finalX, finalY);
  }

  void ApplyTreeAttachment()
  {
    // 1. 关闭重力并清除物理速度（防止物理引擎干扰手动位移）
    rb.gravityScale = 0f;
    rb.linearVelocity = Vector2.zero;

    // 2. 获取垂直输入 (W/S 或 上下箭头)
    float verticalInput = Input.GetAxisRaw("Vertical");

    // 3. 计算爬行位移
    Vector2 climbMovement = Vector2.up * verticalInput * climbSpeed * Time.fixedDeltaTime;

    // 4. 同步树本身的位移 (如果树是移动平台)
    Vector2 treeDelta = Vector2.zero;
    if (currentTreeRb != null && currentTreeRb.bodyType != RigidbodyType2D.Static)
    {
      treeDelta = currentTreeRb.position - lastPlatformPos;
      lastPlatformPos = currentTreeRb.position;
    }

    // 5. 应用最终位置
    rb.MovePosition(rb.position + climbMovement + treeDelta);
  }

  // 附着到树上
  void AttachToTree(Rigidbody2D treeRb)
  {
    isAttachedToTree = true;
    currentTreeRb = treeRb;
    rb.gravityScale = 0f; // 停止重力
    rb.linearVelocity = Vector2.zero; // 停止现有运动
    lastPlatformPos = currentTreeRb.position; // 记录初始位置以便同步
                                              // 可以设置为树的子对象，这样树移动时玩家也会移动
                                              // transform.SetParent(currentTreeRb.transform); 
  }

  // 脱离树
  void DetachFromTree()
  {
    isAttachedToTree = false;
    currentTreeRb = null;
    rb.gravityScale = originalGravityScale; // 恢复重力
                                            // 施加一个向上的力，模拟跳离
    rb.linearVelocity = new Vector2(moveInput * moveSpeed, jumpForce * 0.7f); // 脱离时给个小跳力
                                                                              // transform.SetParent(null); // 取消父子关系
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

    if (collision.gameObject.CompareTag("Tree") && !isAttachedToTree)
    {
      if (rb.linearVelocity.y < 0.1f || collision.contacts[0].normal.y < 0.5f)
      {
        AttachToTree(collision.rigidbody);
        return;
      }
    }

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