using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
{
    public void Initialize()
    {
        DontDestroyOnLoad(gameObject);
    }

    public Coroutine Run(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }

    public void Stop(Coroutine routine)
    {
        if (routine != null) StopCoroutine(routine);
    }
}
