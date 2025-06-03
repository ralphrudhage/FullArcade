using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.1f;
    [SerializeField] private Vector3 offset;

    private Coroutine moveDownCoroutine;
    private float yTarget = -5.7f;

    private void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z) + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }

    public void MoveDown()
    {
        if (moveDownCoroutine != null)
            StopCoroutine(moveDownCoroutine);

        moveDownCoroutine = StartCoroutine(MoveYToTarget(yTarget, 1.5f));
    }
    
    private IEnumerator MoveYToTarget(float targetY, float duration)
    {
        float time = 0f;
        float startY = transform.position.y;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float newY = Mathf.Lerp(startY, targetY, t);

            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            yield return null;
        }

        // Final snap just in case
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }

}