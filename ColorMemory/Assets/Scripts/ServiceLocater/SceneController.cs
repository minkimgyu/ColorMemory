using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ISceneControllable;

public class SceneController : ISceneControllable
{
    public void ChangeScene(SceneName sceneName)
    {
        ServiceLocater.ReturnSoundPlayer().StopBGM(); // ∫Í±› ∏ÿ√Á¡ÿ¥Ÿ.
        SceneManager.LoadScene(sceneName.ToString());
    }

    public SceneName GetCurrentSceneName()
    {
        return (SceneName)Enum.Parse(typeof(SceneName), SceneManager.GetActiveScene().name);
    }
}
