using UnityEngine;

public class Item : MonoBehaviour
{
  // public int scoreValue = 1;
  public int healValue = 1;
  AudioManager audioManager;

    Vector3 startPos;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

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
      if(audioManager != null)
                audioManager.PlaySFX(audioManager.cheesecollect);
      Destroy(gameObject);
    }
  }
}