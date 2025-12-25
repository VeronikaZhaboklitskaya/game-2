using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
  public GameObject itemPrefab;
  public float spawnInterval = 12f;
  public float spawnRangeX = 8f;
  public float spawnRangeY = 4f;

  public LayerMask groundLayer;
  void Start()
  {
    InvokeRepeating("SpawnItem", 1f, spawnInterval);
  }


  void SpawnItem()
  {
    float randomX = Random.Range(-spawnRangeX, spawnRangeX);
    Vector2 rayOrigin = new Vector2(randomX, 10f);

    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 20f, groundLayer);

    if (hit.collider != null)
    {
      Vector2 spawnPos = hit.point + new Vector2(0, 0.5f);
      Instantiate(itemPrefab, spawnPos, Quaternion.identity);
    }
  }
}
