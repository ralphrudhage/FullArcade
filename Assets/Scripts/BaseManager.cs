using System;
using System.Collections;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    protected IEnumerator DelayedAction(float? delay, Action method)
    {
        if (delay is > 0f)
        {
            yield return new WaitForSeconds(delay.Value);
        }
        else
        {
            yield return null;
        }
    
        method?.Invoke();
    }

}