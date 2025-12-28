using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
  public GameObject itemPrefab;
  public float spawnInterval = 12f;
  public LayerMask groundLayer;

  public LayerMask itemLayer;
  public float minItemDistance = 1.5f;

  private List<Collider2D> platformCache = new List<Collider2D>();

  void Start()
  {
    UpdatePlatformCache();
    InvokeRepeating("SpawnItem", 1f, spawnInterval);
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

  void SpawnItem()
  {
    if (platformCache.Count == 0) return;

    Collider2D target = platformCache[Random.Range(0, platformCache.Count)];

    float margin = 0.5f;
    float randomX = Random.Range(target.bounds.min.x + margin, target.bounds.max.x - margin);

    Vector2 rayOrigin = new Vector2(randomX, target.bounds.max.y + 0.5f);
    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 1f, groundLayer);

    if (hit.collider != null)
    {
      Vector3 spawnPos = new Vector3(hit.point.x, hit.point.y + 0.5f, 0f);

      if (Physics2D.OverlapCircle(spawnPos, minItemDistance, itemLayer)) return;

      Instantiate(itemPrefab, spawnPos, Quaternion.identity);
    }
  }
}