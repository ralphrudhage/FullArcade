using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.1f;
    [SerializeField] private Vector3 offset;

    private void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z) + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}