using UnityEngine;
using System.Collections;
public class Enemy : MonoBehaviour
{
  public float moveSpeed = 2f;
  public Transform groundCheckPoint;
  public Transform wallCheckPoint;
  public float detectionDistance = 0.6f;

  float lastFlipTime;
  float flipCooldown = 0.2f; // 翻转冷却时间
  public LayerMask groundLayer;

  int direction = 1;
  Rigidbody2D rb;
  bool canDetect;

  IEnumerator Start()
  {
    rb = GetComponent<Rigidbody2D>();
    rb.constraints = RigidbodyConstraints2D.FreezeRotation;

    yield return null; // 等一帧
    canDetect = true;
  }

  void FixedUpdate()
  {
    rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

    if (!canDetect) return;

    bool hasGround = Physics2D.Raycast(
        groundCheckPoint.position,
        Vector2.down,
        detectionDistance,
        groundLayer
    );

    bool hitWall = Physics2D.Raycast(
        wallCheckPoint.position,
        Vector2.right * direction,
        0.2f,
        groundLayer
    );

    if (!hasGround || hitWall)
    {
      Flip();
    }
  }

  void Flip()
  {
    if (Time.time < lastFlipTime + flipCooldown) return; // 冷却中不执行翻转

    direction *= -1;
    Vector3 s = transform.localScale;
    s.x *= -1;
    transform.localScale = s;

    lastFlipTime = Time.time;
  }
}
