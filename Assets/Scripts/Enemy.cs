using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
  public float moveSpeed = 2f;
  public Transform groundCheckPoint;
  public Transform wallCheckPoint;
  public float detectionDistance = 0.6f;
  public LayerMask groundLayer;

  private SpriteRenderer sr;
  private Rigidbody2D rb;
  private int direction = 1;
  private float lastFlipTime;
  private float flipCooldown = 0.3f;
  private bool canDetect;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    sr = GetComponent<SpriteRenderer>();
    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    canDetect = true;
  }

  void FixedUpdate()
  {
    if (!canDetect) return;

    rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

    Vector2 checkPos = groundCheckPoint.position;

    bool hasGround = Physics2D.Raycast(checkPos, Vector2.down, detectionDistance, groundLayer);

    bool hitWall = Physics2D.Raycast(wallCheckPoint.position, Vector2.right * direction, 0.1f, groundLayer);

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
  }
}