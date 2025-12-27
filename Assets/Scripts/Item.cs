using UnityEngine;

public class Item : MonoBehaviour
{
  // public int scoreValue = 1;
  public int healValue = 1;

  Vector3 startPos;

  void Start()
  {
    startPos = transform.localPosition;
  }

  void Update()
  {
    transform.localPosition = startPos +
        new Vector3(0, Mathf.Sin(Time.time * 5f) * 0.2f, 0);
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      // ScoreManager.instance.AddScore(scoreValue);
      GameManager.instance.Heal(healValue);

      Destroy(gameObject);
    }
  }
}