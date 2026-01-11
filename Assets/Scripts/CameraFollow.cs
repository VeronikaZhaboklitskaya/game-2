using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float FollowSpeed = 1.5f;
    public float yOffset = 1f;
    public float xOffset = 3.5f;
    public Transform target;

    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x + xOffset, target.position.y + yOffset, -50f);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);    
    }
}