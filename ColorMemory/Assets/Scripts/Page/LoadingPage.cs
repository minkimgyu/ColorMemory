using NetworkService.DTO;
using NetworkService.Manager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPage : MonoBehaviour
{
    [SerializeField] Image _loadingPregressBar;
    [SerializeField] TMP_Text _loadingPregressTxt;

    string _userId;
    string _userName;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    async Task<bool> SendDataToServer()
    {
        PlayerManager playerManager = new PlayerManager();
        bool canLogin = false;

        try
        {
            canLogin = await playerManager.AddPlayerAsync(_userId, _userName);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버에 데이터를 보낼 수 없음");
            return false;
        }

        return canLogin;
    }

    void SetUserData()
    {
#if UNITY_STANDALONE
        _userId = "testId1";
        _userName = "meal";
        Debug.Log("Standalone 버전 실행 중");
#elif UNITY_EDITOR
        _userId = "testId1";
        _userName = "meal";
        Debug.Log("Editor 버전 실행 중");
#elif UNITY_ANDROID
    Debug.Log("Android 버전 실행 중");
#elif UNITY_IOS
    Debug.Log("iOS 버전 실행 중");
#else
    Debug.Log("기타 플랫폼");
#endif
    }

    async void SetUp()
    {
        SetUserData();

        bool canLogin = await SendDataToServer();
        if (canLogin == false) return;

        _loadingPregressBar.fillAmount = 0;
        _loadingPregressTxt.text = $"{0} %";

        AddressableHandler addressableHandler = CreateAddressableHandler();
        addressableHandler.AddProgressEvent((value) => { _loadingPregressBar.fillAmount = value; _loadingPregressTxt.text = $"{value * 100} %"; });
        addressableHandler.Load(() => { Initialize(addressableHandler); });
    }

    void InitializeOnAndroid()
    {
        Application.targetFrameRate = 60;

        TimeController timeController = new TimeController();
        SceneController sceneController = new SceneController();
        GPGSManager gPGSManager = new GPGSManager();

        ServiceLocater.Provide(timeController);
        ServiceLocater.Provide(sceneController);
        ServiceLocater.Provide(gPGSManager);

        gPGSManager.Login((isLogin, id, name) =>
        {
            if (isLogin == false)
            {
                // 만약 gpgs 로그인 안 될 경우 리턴
                Debug.Log("GPGS 로그인 실패");
                return;
            }

            SaveManager saveManager = new SaveManager(new SaveData(_userId, _userName));
            ServiceLocater.Provide(saveManager);
            ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.HomeScene);
        });
    }

    void InitializeOnEditor()
    {
        TimeController timeController = new TimeController();
        SceneController sceneController = new SceneController();

        SaveManager saveManager = new SaveManager(new SaveData(_userId, _userName));

        ServiceLocater.Provide(timeController);
        ServiceLocater.Provide(sceneController);
        ServiceLocater.Provide(saveManager);

        ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.HomeScene);
    }

    void Initialize(AddressableHandler addressableHandler)
    {
#if UNITY_STANDALONE
       InitializeOnEditor();
#elif UNITY_EDITOR
        InitializeOnEditor();
#elif UNITY_ANDROID
       InitializeOnAndroid();
#elif UNITY_IOS
#else
#endif
    }

    AddressableHandler CreateAddressableHandler()
    {
        GameObject addressableObject = new GameObject("AddressableHandler");
        AddressableHandler addressableHandler = addressableObject.AddComponent<AddressableHandler>();
        addressableHandler.Initialize();

        return addressableHandler;
    }
}
