using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NetworkService.Manager;

public class OnboardingPage : MonoBehaviour
{
    [SerializeField] Slider _loadingPregressBar;
    [SerializeField] TMP_Text _loadingPregressTxt;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    void SetUp()
    {
        _loadingPregressBar.value = 0;
        _loadingPregressTxt.text = $"{0} %";

        AddressableHandler addressableHandler = CreateAddressableHandler();
        addressableHandler.AddProgressEvent((value) => { _loadingPregressBar.value = value; _loadingPregressTxt.text = $"{value * 100} %"; });
        addressableHandler.Load(() => { Initialize(addressableHandler); });

        PlayerManager playerManager = new PlayerManager();
        playerManager.AddPlayerAsync("testId", "testName");
    }

    void Initialize(AddressableHandler addressableHandler)
    {
        SceneController sceneController = new SceneController();
        SaveManager saveManager = new SaveManager(new SaveData("meal"));

        //SoundPlayer soundPlayer = FindObjectOfType<SoundPlayer>();
        //soundPlayer.Initialize(addressableHandler.SoundAsset);

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
