using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
  public GameObject enemyPrefab;
  public float spawnInterval = 2f;
  public float spawnRangeX = 8f;
  public float spawnRangeY = 4f;

  public LayerMask groundLayer;
  void Start()
  {
    InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
  }

  void SpawnEnemy()
  {
    float randomX = Random.Range(-spawnRangeX, spawnRangeX);
    Vector2 rayOrigin = new Vector2(randomX, 10f);

    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 20f, groundLayer);

    if (hit.collider != null)
    {
      Vector2 spawnPos = hit.point + new Vector2(0, 0.5f);
      Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
  }
}
