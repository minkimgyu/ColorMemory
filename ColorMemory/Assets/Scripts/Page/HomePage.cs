using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NetworkService.Manager;
using NetworkService.DTO;
using System.Threading.Tasks;

public class HomePage : MonoBehaviour
{
    public enum InnerPageState
    {
        Main, // -> Collection, Ranking, Shop
        Collection, // -> Main, Ranking
        Ranking, // -> Main, Shop
        Shop, // -> Main, Ranking
        Setting
    }

    EffectFactory _effectFactory;
    DotFactory _dotFactory;

    const int _dotSize = 6;
    const float _contentSize = 0.8f;

    [Header("Setting")]
    [SerializeField] SideSheetUI _sideSheetUI;

    [Header("Top")]
    [SerializeField] Button _homeBtn;
    [SerializeField] Button _shopBtn;
    [SerializeField] TMP_Text _goldTxt;
    [SerializeField] Button _rankingBtn;
    [SerializeField] Button _settingBtn;

    [Header("Main")]
    [SerializeField] GameObject _mainContent;
    [SerializeField] Image _modeTitleImg;
    [SerializeField] ToggleBtn _toggleBtn;
    [SerializeField] Transform _dotParent;
    [SerializeField] Button _playBtn;
    [SerializeField] TMP_Text _playBtnTxt;

    [Header("Collect")]
    [SerializeField] GameObject _collectionContent;

    [SerializeField] GameObject _selectStageContent;
    [SerializeField] Transform _stageUIContent;

    [SerializeField] TMP_Text _titleTxt;
    [SerializeField] TMP_Text _descriptionTxt;

    [SerializeField] Image _completeSlider;
    [SerializeField] TMP_Text _leftCompleteText;
    [SerializeField] TMP_Text _totalCompleteText;
    [SerializeField] Button _playCollectModeBtn;
    [SerializeField] Button _exitBtn;

    [SerializeField] TMP_Text _stageUsedHintUseCount;
    [SerializeField] TMP_Text _stageWrongCount;

    [SerializeField] ArtworkScrollUI _artworkScrollUI;

    [Header("Ranking")]
    [SerializeField] GameObject _rankingContent;
    [SerializeField] Transform _rankingScrollContent;
    [SerializeField] Transform _myRankingContent;

    [Header("Shop")]
    [SerializeField] GameObject _shopContent;
    [SerializeField] Transform _shopScrollContent;

    [Header("Setting")]
    [SerializeField] SettingPage _settingPage;

    FSM<InnerPageState> _pageFsm;

    TopElementPresenter _topElementPresenter;

    async Task<Dictionary<int, ArtData>> GetArtDataFromServer()
    {
        ArtworkManager artworkManager = new ArtworkManager();
        List<PlayerArtworkDTO> ownedArtworkDTOs, unownedArtworkDTOs;

        try
        {
            ownedArtworkDTOs = await artworkManager.GetOwnedArtworksAsync("testId1");
            unownedArtworkDTOs = await artworkManager.GetUnownedArtworksAsync("testId1");
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버로부터 데이터를 받아오지 못함");
            return null;
        }

        Dictionary<int, ArtData> artDatas = new Dictionary<int, ArtData>();

        for (int i = 0; i < ownedArtworkDTOs.Count; i++)
        {
            Dictionary<int, StageData> stageDatas = new Dictionary<int, StageData>();

            foreach (var dto in ownedArtworkDTOs[i].Stages)
            {
                StageData stageData = new StageData(dto.Value.Rank, dto.Value.HintUsage, dto.Value.IncorrectCnt);
                stageDatas.Add(dto.Key, stageData);
            }

            ArtData artData = new ArtData(
                ownedArtworkDTOs[i].Rank,
                ownedArtworkDTOs[i].HasIt,
                stageDatas,
                ownedArtworkDTOs[i].TotalMistakesAndHints,
                ownedArtworkDTOs[i].ObtainedDate);

            artDatas.Add(ownedArtworkDTOs[i].ArtworkId, artData);
        }

        for (int i = 0; i < unownedArtworkDTOs.Count; i++)
        {
            Dictionary<int, StageData> stageDatas = new Dictionary<int, StageData>();

            foreach (var dto in unownedArtworkDTOs[i].Stages)
            {
                StageData stageData = new StageData(dto.Value.Rank, dto.Value.HintUsage, dto.Value.IncorrectCnt);
                stageDatas.Add(dto.Key, stageData);
            }

            ArtData artData = new ArtData(
                unownedArtworkDTOs[i].Rank,
                unownedArtworkDTOs[i].HasIt,
                stageDatas,
                unownedArtworkDTOs[i].TotalMistakesAndHints,
                unownedArtworkDTOs[i].ObtainedDate);

            artDatas.Add(unownedArtworkDTOs[i].ArtworkId, artData);
        }

        return artDatas;
    }

    private async void Start()
    {
        Dictionary<int, ArtData> artDatas = await GetArtDataFromServer();
        if (artDatas == null) return;

        AddressableHandler addressableHandler = FindObjectOfType<AddressableHandler>();
        if (addressableHandler == null) return;

        _effectFactory = new EffectFactory(addressableHandler.EffectAssets);
        _dotFactory = new DotFactory(addressableHandler.DotAssets);

        StageUIFactory stageUIFactory = new StageUIFactory(
           addressableHandler.SpawnableUIAssets[SpawnableUI.Name.StageSelectBtnUI]
       );

        ArtworkUIFactory artWorkUIFactory = new ArtworkUIFactory(
            addressableHandler.SpawnableUIAssets[SpawnableUI.Name.ArtworkUI],
            addressableHandler.ArtSpriteAsserts,
            addressableHandler.ArtworkFrameAsserts
        );

        RankingUIFactory rankingUIFactory = new RankingUIFactory(
            addressableHandler.SpawnableUIAssets[SpawnableUI.Name.RankingUI],
            addressableHandler.ProfileIconAssets
        );

        ShopBundleUIFactory shopBundleUIFactory = new ShopBundleUIFactory(
            addressableHandler.SpawnableUIAssets[SpawnableUI.Name.ShopBundleUI]
        );

        _shopBtn.onClick.AddListener(() => { _pageFsm.OnClickShopBtn(); });
        _homeBtn.onClick.AddListener(() => { _pageFsm.OnClickHomeBtn(); });
        _rankingBtn.onClick.AddListener(() => { _pageFsm.OnClickRankingBtn(); });
        _settingBtn.onClick.AddListener(() => { _settingPage.TogglePanel(); });


        TopElementModel topElementModel = new TopElementModel();
        TopElementViewer topElementViewer = new TopElementViewer(_goldTxt);
        _topElementPresenter = new TopElementPresenter(topElementViewer, topElementModel);


        MoneyManager moneyManager = new MoneyManager();
        int money = await moneyManager.GetMoneyAsync("testId1");
        _topElementPresenter.ChangeGoldCount(money);

        _settingPage.Initialize(addressableHandler.ProfileIconAssets);

        SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();

        _pageFsm = new FSM<InnerPageState>();
        _pageFsm.Initialize(new Dictionary<InnerPageState, BaseState<InnerPageState>>
        {
            { 
                InnerPageState.Main, new MainPageState(
                data.SelectedType,
                _effectFactory,
                _dotFactory,
                _modeTitleImg,
                _toggleBtn,
                _dotParent,
                _playBtn,
                _playBtnTxt,
                _mainContent,
                addressableHandler.ModeTitleIconAssets,
                _pageFsm)
            },
            { 
                InnerPageState.Collection, new CollectionPageState(
                _homeBtn,
                _collectionContent,
                _titleTxt,
                _descriptionTxt,
                _completeSlider,
                _leftCompleteText,
                _totalCompleteText,
                _selectStageContent,
                _stageUIContent,
                _exitBtn,
                _playCollectModeBtn,
                _stageUsedHintUseCount,
                _stageWrongCount,
                _artworkScrollUI,
                artWorkUIFactory,
                stageUIFactory,
                artDatas,
                addressableHandler.ArtworkJsonAsset.Data,
                addressableHandler.CollectiveArtJsonAsserts,
                _pageFsm)
            },
            { 
                InnerPageState.Ranking, new RankingPageState(
                _rankingContent,
                _rankingScrollContent,
                _myRankingContent,
                rankingUIFactory,
                _pageFsm)
            },
            {
                InnerPageState.Shop, new ShopPageState(
                _shopContent,
                _shopScrollContent,
                shopBundleUIFactory,
                _topElementPresenter,
                _pageFsm)
            },
        }, InnerPageState.Main);
    }
}
