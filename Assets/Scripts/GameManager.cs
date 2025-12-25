using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;

  public int maxHealth = 3;
  public int currentHealth;

  public bool isGameOver = false;

  void Awake()
  {
    if (instance == null)
      instance = this;
    else
      Destroy(gameObject);
  }

  void Start()
  {
    currentHealth = maxHealth;
    Debug.Log("Game Start! Health: " + currentHealth);
  }

  public void TakeDamage(int amount)
  {
    if (isGameOver) return;

    currentHealth -= amount;
    Debug.Log("Hit Enemy! Health: " + currentHealth);

    if (currentHealth <= 0)
    {
      GameOver();
    }
  }

  public void Heal(int amount)
  {
    if (isGameOver) return;

    currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    Debug.Log("Heal! Health: " + currentHealth);
  }

  void GameOver()
  {
    isGameOver = true;
    Debug.Log("GAME OVER");
    Time.timeScale = 0f;
  }
}
