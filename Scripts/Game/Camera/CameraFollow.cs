using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // Player to follow
    public float smoothSpeed = 5f;  // Camera smoothness
    public Vector3 offset;          // Distance from player

    void LateUpdate()
    {
        if (target == null)
            return;

        // Desired position
        Vector3 desiredPosition = target.position + offset;

        // Smooth movement
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Apply position
        transform.position = smoothedPosition;
    }
}
