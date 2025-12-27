using UnityEngine;

public class Enemy : MonoBehaviour
{
  public float moveSpeed = 2f;
  public float detectionDistance = 0.6f;
  public LayerMask groundLayer;

  public Vector2 checkOffset = new Vector2(0.5f, -0.5f);

  private SpriteRenderer sr;
  private Rigidbody2D rb;
  private int direction = 1;
  private float lastFlipTime;
  private float flipCooldown = 0.3f;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    sr = GetComponent<SpriteRenderer>();
    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

    rb.sleepMode = RigidbodySleepMode2D.StartAwake;
  }

  void FixedUpdate()
  {
    rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

    Vector2 currentCheckPos = (Vector2)transform.position + new Vector2(checkOffset.x * direction, checkOffset.y);

    bool hasGround = Physics2D.Raycast(currentCheckPos, Vector2.down, detectionDistance, groundLayer);
    bool hitWall = Physics2D.Raycast(currentCheckPos, Vector2.right * direction, 0.1f, groundLayer);

    Debug.DrawRay(currentCheckPos, Vector2.down * detectionDistance, hasGround ? Color.green : Color.red);

    if (!hasGround || hitWall)
    {
      Flip();
    }
  }

  void Flip()
  {
    if (Time.time < lastFlipTime + flipCooldown) return;

    direction *= -1;
    sr.flipX = (direction == -1);
    lastFlipTime = Time.time;

    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
  }
}