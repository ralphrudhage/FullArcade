using System;
using UnityEngine;

public class SledPull : MonoBehaviour
{
    private Grizzly grizzly;

    private void OnEnable()
    {
        grizzly = FindAnyObjectByType<Grizzly>();
    }

    public void Move()
    {
        grizzly.MoveSled();
    }
}
