using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
  public GameObject enemyPrefab;
  public float spawnInterval = 5f;
  public LayerMask groundLayer;
  public LayerMask enemyLayer;
  public float minEnemyDistance = 2f;

  private List<Collider2D> platformCache = new List<Collider2D>();

  void Start()
  {
    UpdatePlatformCache();
    InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
  }

  void UpdatePlatformCache()
  {
    Collider2D[] platforms = Physics2D.OverlapAreaAll(new Vector2(-200, -200), new Vector2(200, 200), groundLayer);
    platformCache.AddRange(platforms);
  }

  void SpawnEnemy()
  {
    if (platformCache.Count == 0) return;

    Collider2D target = platformCache[Random.Range(0, platformCache.Count)];

    float margin = 0.5f;
    float randomX = Random.Range(target.bounds.min.x + margin, target.bounds.max.x - margin);

    Vector2 rayOrigin = new Vector2(randomX, target.bounds.max.y + 0.5f);
    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 1f, groundLayer);

    if (hit.collider != null)
    {
      Vector3 spawnPos = new Vector3(hit.point.x, hit.point.y + 0.2f, 0f);

      if (Physics2D.OverlapCircle(spawnPos, minEnemyDistance, enemyLayer)) return;

      Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
  }
}