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
    Collider2D[] platforms = Physics2D.OverlapAreaAll(new Vector2(-200, -200), new Vector2(200, 200), groundLayer);
    platformCache.Clear();
    platformCache.AddRange(platforms);
  }

  void SpawnItem()
  {
    if (platformCache.Count == 0) return;

    // 1. 随机挑选一个地面物体
    Collider2D target = platformCache[Random.Range(0, platformCache.Count)];

    // 2. 计算该地面的水平边界，预留一小段边距防止生成在死角
    float margin = 0.5f;
    float randomX = Random.Range(target.bounds.min.x + margin, target.bounds.max.x - margin);

    // 3. 从平台顶端稍微靠上的位置发射射线
    Vector2 rayOrigin = new Vector2(randomX, target.bounds.max.y + 0.5f);
    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 1f, groundLayer);

    if (hit.collider != null)
    {
      // 4. 计算生成位置，强制 Z 为 0，并稍微抬高一点防止嵌入地面
      Vector3 spawnPos = new Vector3(hit.point.x, hit.point.y + 0.5f, 0f);

      // 5. [可选] 检查该位置是否已经有其他物品了，防止重叠
      if (Physics2D.OverlapCircle(spawnPos, minItemDistance, itemLayer)) return;

      Instantiate(itemPrefab, spawnPos, Quaternion.identity);
    }
  }
}