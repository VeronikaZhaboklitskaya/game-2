using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public float moveSpeed = 5f;
  public float jumpForce = 8f;

  private Rigidbody2D rb;
  private bool isGrounded;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
  }

  void Update()
  {
    Move();
    Jump();
  }

  void Move()
  {
    float moveInput = Input.GetAxis("Horizontal");
    rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
  }

  void Jump()
  {
    if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
    {
      rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("Ground"))
    {
      isGrounded = true;
    }
    if (collision.gameObject.CompareTag("Enemy"))
    {
      Debug.Log("Game Over");
      Time.timeScale = 0f;
    }
  }

  private void OnCollisionExit2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("Ground"))
    {
      isGrounded = false;
    }
  }
}
