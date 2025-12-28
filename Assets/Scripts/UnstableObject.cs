using UnityEngine;

public class UnstableObject : MonoBehaviour
{
  private Rigidbody2D rb;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    rb.centerOfMass = new Vector2(0, 0.5f);
  }
}