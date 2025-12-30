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
    platformCache.Clear();

    Collider2D[] platforms = FindObjectsByType<Collider2D>(FindObjectsSortMode.None);
    foreach (var col in platforms)
    {
      if (((1 << col.gameObject.layer) & groundLayer) != 0)
      {
        platformCache.Add(col);
      }
    }
  }

  void SpawnEnemy()
  {
    if (platformCache.Count == 0) return;

    Collider2D target = platformCache[Random.Range(0, platformCache.Count)];

    float margin = 1.0f;

    float randomX = Random.Range(target.bounds.min.x + margin, target.bounds.max.x - margin);

    Vector2 rayOrigin = new Vector2(randomX, target.bounds.max.y + 0.5f);
    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 1f, groundLayer);

    if (hit.collider != null)
    {
      Vector3 spawnPos = new Vector3(hit.point.x, hit.point.y + 0.5f, 0f);
      Collider2D wallCheck = Physics2D.OverlapCircle(spawnPos, 0.4f, groundLayer);
      if (wallCheck != null) return;
      if (Physics2D.OverlapCircle(spawnPos, minEnemyDistance, enemyLayer)) return;

      Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
  }

}