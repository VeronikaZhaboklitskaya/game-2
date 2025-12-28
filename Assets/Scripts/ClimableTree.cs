// ClimbableTree.cs (挂在你的树物体上)
using UnityEngine;

public class ClimbableTree : MonoBehaviour
{
  public Rigidbody2D treeRb { get; private set; }

  void Awake()
  {
    treeRb = GetComponent<Rigidbody2D>();
  }
}