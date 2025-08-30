using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullCoroutineRunner : ICoroutineRunner
{
    public void Initialize()
    {
    }

    public Coroutine Run(IEnumerator routine)
    {
        return null;
    }

    public void Stop(Coroutine routine)
    {
    }
}
