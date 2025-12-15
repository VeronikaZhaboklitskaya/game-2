using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
  public GameObject enemyPrefab;
  public float spawnInterval = 2f;
  public float spawnRangeX = 8f;
  public float spawnRangeY = 4f;
  void Start()
  {
    InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
  }

  void SpawnEnemy()
  {
    Vector2 spawnPos = new Vector2(
      Random.Range(-spawnRangeX, spawnRangeX),
      Random.Range(-spawnRangeY, spawnRangeY)
    );

    Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
  }
}
