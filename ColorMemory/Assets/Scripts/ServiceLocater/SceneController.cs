using DG.Tweening;
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
        // 씬 전환 전에 실행 중인 모든 트윈 제거
        DOTween.KillAll();
        ServiceLocater.ReturnSoundPlayer().StopBGM(); // 브금 멈춰준다.
        SceneManager.LoadScene(sceneName.ToString());
    }

    public SceneName GetCurrentSceneName()
    {
        return (SceneName)Enum.Parse(typeof(SceneName), SceneManager.GetActiveScene().name);
    }
}
