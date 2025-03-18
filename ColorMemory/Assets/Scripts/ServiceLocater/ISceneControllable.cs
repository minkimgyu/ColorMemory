using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneControllable
{
    public enum SceneName
    {
        OnboardingScene,
        HomeScene,
        ChallengeScene,
        CollectScene,
    }

    void ChangeScene(SceneName sceneName);
    SceneName GetCurrentSceneName();
}
