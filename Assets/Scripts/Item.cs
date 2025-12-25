using UnityEngine;

public class Item : MonoBehaviour
{
  public int scoreValue = 1;


  void Update()
  {
    transform.localPosition += new Vector3(0, Mathf.Sin(Time.time * 5f) * 0.005f, 0);
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      ScoreManager.instance.AddScore(scoreValue);

      Destroy(gameObject);
    }
  }
}