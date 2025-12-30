using UnityEngine;

public class ClimbableTree : MonoBehaviour
{
  public Rigidbody2D treeRb { get; private set; }

  void Awake()
  {
    treeRb = GetComponent<Rigidbody2D>();
  }
}