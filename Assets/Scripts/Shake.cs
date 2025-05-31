using System.Collections;
using UnityEngine;

public class Shake : MonoBehaviour
{
    
    private const float shakeDuration = 0.3f;
    private const float maxShakeAngle = 4.5f;
    private const float decreaseFactor = 1.0f;

    private Quaternion leftOriginalRotation;
    private Quaternion rightOriginalRotation;
    private bool isShaking;
    
    private void TriggerShake()
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeControl());
        }
    }

    private IEnumerator ShakeControl()
    {
        isShaking = true;
        var currentShakeDuration = shakeDuration;

        while (currentShakeDuration > 0)
        {
            var rotationAmount = Random.Range(-maxShakeAngle, maxShakeAngle);
            var newRotation = leftOriginalRotation * Quaternion.Euler(0, 0, rotationAmount);
            transform.localRotation = newRotation;
            
            currentShakeDuration -= Time.deltaTime * decreaseFactor;
            yield return null;
        }

        transform.localRotation = leftOriginalRotation;
        isShaking = false;
    }
}
