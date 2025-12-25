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
    float timeOffset = Mathf.PingPong(Time.fixedTime * speed, moveDistance);
    Vector2 targetPos = startPos + Vector2.up * timeOffset;

    rb.MovePosition(targetPos);
  }
}