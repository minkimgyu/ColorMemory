using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NetworkService.Manager;
using NetworkService.DTO;
using System.Threading.Tasks;

public class LoadingPage : MonoBehaviour
{
    [SerializeField] Image _loadingPregressBar;
    [SerializeField] TMP_Text _loadingPregressTxt;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    async void SetUp()
    {
        PlayerManager playerManager = new PlayerManager();
        bool canLogin = await playerManager.AddPlayerAsync("testId1", "meal");
        if (canLogin == false) return;

        _loadingPregressBar.fillAmount = 0;
        _loadingPregressTxt.text = $"{0} %";

        AddressableHandler addressableHandler = CreateAddressableHandler();
        addressableHandler.AddProgressEvent((value) => { _loadingPregressBar.fillAmount = value; _loadingPregressTxt.text = $"{value * 100} %"; });
        addressableHandler.Load(() => { Initialize(addressableHandler); });
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
