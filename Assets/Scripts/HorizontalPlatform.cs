using UnityEngine;

public class HorizontalPlatform : MonoBehaviour
{
  public float moveDistance = 5f;
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
    float xOffset = Mathf.Sin(Time.fixedTime * speed) * (moveDistance / 2f);

    Vector2 targetPos = startPos + Vector2.right * xOffset;

    rb.MovePosition(targetPos);
  }
}