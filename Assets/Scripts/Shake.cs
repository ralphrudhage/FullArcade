using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shake : MonoBehaviour
{
    [SerializeField] GameObject earth;
    private const float shakeDuration = 0.3f;
    private const float maxShakeAngle = 4f;
    private const float decreaseFactor = 1.0f;

    private Quaternion originalRotation;
    private Quaternion originalEarthRotation;
    
    private bool isShaking;

    private void Start()
    {
        originalRotation = transform.rotation;
        originalEarthRotation = earth.transform.rotation;
    }

    public void TriggerShake()
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
            var shakeRotation = Quaternion.Euler(0, 0, rotationAmount);
            
            transform.localRotation = originalRotation * shakeRotation;
            earth.transform.rotation = originalEarthRotation * shakeRotation;
            
            currentShakeDuration -= Time.deltaTime * decreaseFactor;
            yield return null;
        }

        transform.localRotation = originalRotation;
        earth.transform.rotation = originalEarthRotation;
        isShaking = false;
    }
}
