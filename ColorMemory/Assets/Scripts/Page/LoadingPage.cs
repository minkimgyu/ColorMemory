using Google.Play.AppUpdate;
using Google.Play.Common;
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
    [Header("Patch")]
    [SerializeField] GameObject _patchPanel;
    [SerializeField] TMP_Text _patchTxt;
    [SerializeField] Button _patchBtn;

    [Header("Loading")]
    [SerializeField] GameObject _loadingObj;
    [SerializeField] Image _loadingPregressBar;
    [SerializeField] TMP_Text _loadingPregressTxt;

    [Header("Video")]
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

        _videoPlayer.isLooping = true;
        _loadingObj.SetActive(true);
        SetUserData();

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
        Debug.Log("Standalone ���� ���� ��");

        _userId = "testId1";
        _userName = "testMeal";
        SetUp();
#elif UNITY_EDITOR
        Debug.Log("Editor ���� ���� ��");

        _userId = "testId1";
        _userName = "testMeal";
        SetUp();
#elif UNITY_ANDROID
        Debug.Log("Android ���� ���� ��");
        Application.targetFrameRate = 60;

        UpdateAppVersion(() =>
        {
            LoginToGoogle((id, name) =>
            {
                _userId = id;
                _userName = name;
                SetUp();
            });
        });
#elif UNITY_IOS
    Debug.Log("iOS ���� ���� ��");
#else
    Debug.Log("��Ÿ �÷���");
#endif
    }

    void LoginToGoogle(Action<string, string> OnCompleted)
    {
        GPGSManager gPGSManager = new GPGSManager();
        ServiceLocater.Provide(gPGSManager);

        gPGSManager.Login((isLogin, id, name) =>
        {
            if (isLogin == false)
            {
                // ���� gpgs �α��� �� �� ��� ����
                Debug.Log("GPGS �α��� ����");
                return;
            }

            Debug.Log("GPGS �α��� ����");
            OnCompleted?.Invoke(id, name);
        });
    }

    void UpdateAppVersion(Action OnComplete)
    {
        GameObject inAppUpdateManagerObject = new GameObject("InAppUpdateManager");
        InAppUpdateManager inAppUpdateManager = inAppUpdateManagerObject.AddComponent<InAppUpdateManager>();

        // �ʱ�ȭ �Ϸ� �� ����
        inAppUpdateManager.Initialize((value) =>
        {
            Debug.Log(value);
            OnComplete?.Invoke();
        });
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
            Debug.Log("������ �����͸� ���� �� ����");
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

        AddressableUpdater addressableUpdater = CreateAddressableUpdater();
        addressableUpdater.AddEvents(
            (patchSize, labels) =>
            {
                _patchPanel.SetActive(true);
                _patchTxt.text = $"���� ������ {patchSize}";
                _patchBtn.onClick.AddListener(() => { addressableUpdater.PatchFiles(labels); });
            },
            (value) => 
            {
                _loadingPregressBar.fillAmount = value;
                _loadingPregressTxt.text = $"{(value * 100f).ToString("F2")} %";
            }
        );

        // ������Ʈ ���� Ȯ���ϰ� ���� ����
        addressableUpdater.UpdateAddressable(() =>
        {
            AddressableLoader addressableLoader = CreateAddressableLoader();
            addressableLoader.AddProgressEvent((value) => {
                _loadingPregressBar.fillAmount = value;
                _loadingPregressTxt.text = $"{(value * 100f).ToString("F2")} %";
            });

            addressableLoader.Load(() => { Initialize(addressableLoader); });
        });
    }

    void Initialize(AddressableLoader addressableHandler)
    {
        TimeController timeController = new TimeController();
        SceneController sceneController = new SceneController();
        SaveManager saveManager = new SaveManager(new SaveData(_userId, _userName));

        ServiceLocater.Provide(timeController);
        ServiceLocater.Provide(sceneController);
        ServiceLocater.Provide(saveManager);

        ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.HomeScene);
    }

    AddressableLoader CreateAddressableLoader()
    {
        GameObject addressableObject = new GameObject("AddressableLoader");
        AddressableLoader addressableHandler = addressableObject.AddComponent<AddressableLoader>();
        addressableHandler.Initialize();

        return addressableHandler;
    }

    AddressableUpdater CreateAddressableUpdater()
    {
        GameObject addressableObject = new GameObject("AddressableUpdater");
        AddressableUpdater addressableUpdater = addressableObject.AddComponent<AddressableUpdater>();
        addressableUpdater.Initialize();

        return addressableUpdater;
    }
}
