using UnityEngine;

public class Enemy : MonoBehaviour
{
  public float moveSpeed = 2f;
  private Vector2 direction;

  void Start()
  {
    direction = Random.insideUnitCircle.normalized;
  }

  void Update()
  {
    transform.Translate(direction * moveSpeed * Time.deltaTime);
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("Player"))
    {
      Debug.Log("Game Over");
      Time.timeScale = 0f;
    }

    if (collision.gameObject.CompareTag("Ground"))
    {
      direction = -direction;
    }
  }
}
