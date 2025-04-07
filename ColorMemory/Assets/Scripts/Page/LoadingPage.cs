using NetworkService.DTO;
using NetworkService.Manager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LoadingPage : MonoBehaviour
{
    [SerializeField] GameObject _loadingObj;
    [SerializeField] Image _loadingPregressBar;
    [SerializeField] TMP_Text _loadingPregressTxt;

    [SerializeField] UnityEngine.Video.VideoPlayer _videoPlayer;
    [SerializeField] RenderTexture _renderTexture;

    string _userId;
    string _userName;

    void ClearRenderTextureToWhite(RenderTexture rt)
    {
        RenderTexture activeRT = RenderTexture.active;
        RenderTexture.active = rt;

        GL.Clear(true, true, Color.white);

        RenderTexture.active = activeRT;
    }

    private void Awake()
    {
        ClearRenderTextureToWhite(_renderTexture);
        _loadingObj.SetActive(false);

        _videoPlayer.loopPointReached += (source) => { _loadingObj.SetActive(true); SetUserData(); };

        _videoPlayer.Prepare();
        _videoPlayer.prepareCompleted += (vp) => { vp.Play(); };
    }

    // Start is called before the first frame update
    void Start()
    {
        _videoPlayer.Play();
    }

    void SetUserData()
    {
#if UNITY_STANDALONE
        Debug.Log("Standalone 버전 실행 중");

        _userId = "testId1";
        _userName = "meal";
        SetUp();
#elif UNITY_EDITOR
        Debug.Log("Editor 버전 실행 중");

        _userId = "testId1";
        _userName = "meal";
        SetUp();
#elif UNITY_ANDROID
        Debug.Log("Android 버전 실행 중");
        Application.targetFrameRate = 60;

        GPGSManager gPGSManager = new GPGSManager();
        ServiceLocater.Provide(gPGSManager);

        gPGSManager.Login((isLogin, id, name) =>
        {
            if (isLogin == false)
            {
                // 만약 gpgs 로그인 안 될 경우 리턴
                Debug.Log("GPGS 로그인 실패");
                return;
            }

            Debug.Log("GPGS 로그인 성공");

            _userId = id;
            _userName = name;
            SetUp();
        });
#elif UNITY_IOS
    Debug.Log("iOS 버전 실행 중");
#else
    Debug.Log("기타 플랫폼");
#endif
    }

    async Task<bool> SendDataToServer()
    {
        PlayerManager playerManager = new PlayerManager();
        bool canLogin = false;

        try
        {
            Debug.Log("_userId          " + _userId);
            Debug.Log("_userName            " + _userName);
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

    async void SetUp()
    {
        bool canLogin = await SendDataToServer();
        if (canLogin == false) return;

        _loadingPregressBar.fillAmount = 0;
        _loadingPregressTxt.text = $"{0} %";

        AddressableHandler addressableHandler = CreateAddressableHandler();
        addressableHandler.AddProgressEvent((value) => {
            _loadingPregressBar.fillAmount = value;
            _loadingPregressTxt.text = $"{(value * 100f).ToString("F2")} %";
        });

        addressableHandler.Load(() => { Initialize(addressableHandler); });
    }

    void Initialize(AddressableHandler addressableHandler)
    {
        TimeController timeController = new TimeController();
        SceneController sceneController = new SceneController();
        SaveManager saveManager = new SaveManager(new SaveData(_userId, _userName));

        ServiceLocater.Provide(timeController);
        ServiceLocater.Provide(sceneController);
        ServiceLocater.Provide(saveManager);

        ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.HomeScene);
    }

    AddressableHandler CreateAddressableHandler()
    {
        GameObject addressableObject = new GameObject("AddressableHandler");
        AddressableHandler addressableHandler = addressableObject.AddComponent<AddressableHandler>();
        addressableHandler.Initialize();

        return addressableHandler;
    }
}
