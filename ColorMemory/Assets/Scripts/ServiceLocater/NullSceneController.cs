using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullSceneController : ISceneControllable
{
    public void ChangeScene(ISceneControllable.SceneName sceneName) { }
    public ISceneControllable.SceneName GetCurrentSceneName() { return default; }
}
