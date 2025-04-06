using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using GooglePlayGames.BasicApi;

public interface IGPGS
{
    void Login(Action<bool, string, string> OnLoginComplete);
}

public class NullGPGSManager : IGPGS
{
    public void Login(Action<bool, string, string> OnLoginComplete) { }
}

public class GPGSManager : IGPGS
{
    Action<bool, string, string> OnLoginComplete;

    #region 로그인

    public void Login(Action<bool, string, string> OnLoginComplete)
    {
        this.OnLoginComplete = OnLoginComplete;
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            string displayName = PlayGamesPlatform.Instance.GetUserDisplayName();
            string userID = PlayGamesPlatform.Instance.GetUserId();

            Debug.Log(displayName);
            Debug.Log(userID);
            Debug.Log("로그인 성공");
            OnLoginComplete?.Invoke(true, userID, displayName);
        }
        else
        {
            Debug.Log("로그인 실패");
            OnLoginComplete?.Invoke(false, "", "");
        }

        OnLoginComplete = null;
    }

    #endregion
}