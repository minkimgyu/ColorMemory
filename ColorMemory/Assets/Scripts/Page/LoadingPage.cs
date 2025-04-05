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
            Debug.Log("������ �����͸� ���� �� ����");
            return false;
        }

        return canLogin;
    }

    void SetUserData()
    {
#if UNITY_STANDALONE
        _userId = "testId1";
        _userName = "meal";
#elif UNITY_ANDROID
    // gpgs ID �޾ƿ���
    Debug.Log("Android ���� ���� ��");
#elif UNITY_IOS
    Debug.Log("iOS ���� ���� ��");
#else
    Debug.Log("��Ÿ �÷���");
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

    void Initialize(AddressableHandler addressableHandler)
    {
        TimeController timeController = new TimeController();
        SceneController sceneController = new SceneController();
        SaveManager saveManager = new SaveManager(new SaveData(_userId, _userName));

        //SoundPlayer soundPlayer = FindObjectOfType<SoundPlayer>();
        //soundPlayer.Initialize(addressableHandler.SoundAsset);
        ServiceLocater.Provide(timeController);
        ServiceLocater.Provide(sceneController);
        ServiceLocater.Provide(saveManager);
        //ServiceLocater.Provide(soundPlayer);

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
