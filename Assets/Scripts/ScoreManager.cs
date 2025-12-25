using UnityEngine;

public class ScoreManager : MonoBehaviour
{
  public static ScoreManager instance;
  private int score = 0;

  void Awake()
  {
    if (instance == null) instance = this;
  }

  public void AddScore(int amount)
  {
    score += amount;
    Debug.Log("Cheese got! Current scores: " + score);
  }
}