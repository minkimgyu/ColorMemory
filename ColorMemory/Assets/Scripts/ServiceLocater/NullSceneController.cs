using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ISceneControllable;

public class NullSceneController : ISceneControllable
{
    public void ChangeScene(SceneName sceneName) { }
    public SceneName GetCurrentSceneName() { return default; }
}
