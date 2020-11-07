using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 5f;
    public float cameraDistance = -10f;
    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + new Vector3(0, 0, cameraDistance);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.fixedDeltaTime);
        transform.position = smoothedPosition;
    }
}
