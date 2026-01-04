using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float FollowSpeed = 1.5f;
    public float yOffset = 1f;
    public Transform target;

    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);    
    }
}