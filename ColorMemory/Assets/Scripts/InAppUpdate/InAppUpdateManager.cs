using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Play.AppUpdate;
using Google.Play.Common;
using System;

// gameci test runner용
#if UNITY_STANDALONE_LINUX
public class InAppUpdateManager : MonoBehaviour
{
    public void Initialize(Action<string> OnCompleted)
    {
    }
}
#else
public class InAppUpdateManager : MonoBehaviour
{
    AppUpdateManager _appUpdateManager;

    public void Initialize(Action<string> OnCompleted)
    {
        _appUpdateManager = new AppUpdateManager();
        StartCoroutine(CheckForUpdate(OnCompleted));
    }

    IEnumerator CheckForUpdate(Action<string> OnCompleted)
    {
        yield return new WaitForSeconds(1);

        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation = _appUpdateManager.GetAppUpdateInfo();

        // Wait until the asynchronous operation completes.
        yield return appUpdateInfoOperation;

        if (appUpdateInfoOperation.IsSuccessful)
        {
            var appUpdateInfoResult = appUpdateInfoOperation.GetResult();

            if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();
                var startUpdateRequest = _appUpdateManager.StartUpdate(appUpdateInfoResult, appUpdateOptions);

                while (startUpdateRequest.IsDone == false)
                {
                    if (startUpdateRequest.Status == AppUpdateStatus.Downloading)
                    {
                        //Debug.Log("업데이트 다운로드 진행 중");
                    }
                    else if (startUpdateRequest.Status == AppUpdateStatus.Downloaded)
                    {
                        //Debug.Log("다운로드 완료");
                    }

                    yield return null;
                }

                var result = _appUpdateManager.CompleteUpdate();
                while (result.IsDone == false)
                {
                    yield return new WaitForEndOfFrame();
                }

                OnCompleted?.Invoke("업데이트 완료");
                yield return (int)startUpdateRequest.Status;
            }
            else if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateNotAvailable)
            {
                OnCompleted?.Invoke("업데이트가 없음");
                yield return (int)UpdateAvailability.UpdateNotAvailable;
            }
            else
            {
                OnCompleted?.Invoke("업데이트 가능 여부를 알 수 없음");
                yield return (int)UpdateAvailability.Unknown;
            }
        }
        else
        {
            OnCompleted?.Invoke("업데이트 오류");
        }
    }
}
#endif