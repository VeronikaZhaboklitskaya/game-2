using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
  public float moveDistance = 3f;
  public float speed = 2f;

  private Rigidbody2D rb;
  private Vector2 startPos;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    startPos = rb.position;

    rb.bodyType = RigidbodyType2D.Kinematic;
    rb.interpolation = RigidbodyInterpolation2D.Interpolate;
  }

  void FixedUpdate()
  {
    // 使用 Sin 函数替代 PingPong，产生平滑的上下往复运动
    // Mathf.Sin 返回 -1 到 1，我们需要将其映射到 0 到 1 
    float sinValue = (Mathf.Sin(Time.fixedTime * speed) + 1f) / 2f;

    float timeOffset = sinValue * moveDistance;
    Vector2 targetPos = startPos + Vector2.up * timeOffset;

    rb.MovePosition(targetPos);
  }
}