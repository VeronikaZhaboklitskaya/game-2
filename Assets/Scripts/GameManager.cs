using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;

  [Header("UI References")]
  public GameObject gameOverPanel;
  public int maxHealth = 3;
  public List<Image> hearts;
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
    if (gameOverPanel != null) gameOverPanel.SetActive(false);
    UpdateHealthUI();
  }

  void UpdateHealthUI()
  {
    for (int i = 0; i < hearts.Count; i++)
    {
      if (i < currentHealth)
      {
        hearts[i].enabled = true;
      }
      else
      {
        hearts[i].enabled = false;
      }
    }
  }

  public void TakeDamage(int amount)
  {
    if (isGameOver) return;
    currentHealth -= amount;
    if (currentHealth <= 0)
    {
      GameOver();
    }
    Debug.Log("Hit Enemy! Health: " + currentHealth);
    UpdateHealthUI();
  }

  public void Heal(int amount)
  {
    if (isGameOver) return;
    currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    UpdateHealthUI();
    Debug.Log("Heal! Health: " + currentHealth);
  }

  public void FallDie()
  {
    if (isGameOver) return;
    currentHealth = 0;
    UpdateHealthUI();
    GameOver();
  }

  void GameOver()
  {
    isGameOver = true;
    Time.timeScale = 0f;

    if (gameOverPanel != null)
    {
      gameOverPanel.SetActive(true);
    }
  }

  public void RestartGame()
  {
    Time.timeScale = 1f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}