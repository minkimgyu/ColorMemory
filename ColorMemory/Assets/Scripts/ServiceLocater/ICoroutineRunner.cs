using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICoroutineRunner
{
    void Initialize();
    Coroutine Run(IEnumerator routine);
    void Stop(Coroutine routine);
}
