using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NetworkService.Manager;
using NetworkService.DTO;
using System.Threading.Tasks;
using DG.Tweening;

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

    [SerializeField] GameObject _artworkInfoContent;
    [SerializeField] GameObject _artworkCompleteRatioContent;

    [SerializeField] CustomProgressUI _currentComplete;
    [SerializeField] TMP_Text _currentCompleteRatio;

    [SerializeField] CustomProgressUI _totalComplete;
    [SerializeField] TMP_Text _totalCompleteRatio;

    [SerializeField] TMP_Text _titleTxt;
    [SerializeField] TMP_Text _descriptionTxt;

    [SerializeField] Image _completeSlider;
    [SerializeField] TMP_Text _leftCompleteText;
    [SerializeField] TMP_Text _totalCompleteText;

    [SerializeField] TMP_Text _stageNameTxt;
    [SerializeField] TMP_Text _selectStageTitle;

    [SerializeField] TMP_Text _stageUsedHintUseTitle;
    [SerializeField] TMP_Text _stageWrongCountTitle;

    [SerializeField] Button _playCollectModeBtn;
    [SerializeField] Button _exitBtn;

    [SerializeField] GameObject _stageDetailContent;
    [SerializeField] TMP_Text _stageUsedHintUseCount;
    [SerializeField] TMP_Text _stageWrongCount;

    [SerializeField] ArtworkScrollUI _artworkScrollUI;

    [SerializeField] FilterUI _filterScrollUI;
    [SerializeField] Button _filterOpenBtn;
    [SerializeField] Button _filterExitBtn;
    [SerializeField] GameObject _filterContent;
    [SerializeField] TMP_Text _collectionRatioText;
    [SerializeField] TMP_Text _collectionCheerText;

    [SerializeField] TMP_Text _ownFilterTitleText;
    [SerializeField] TMP_Text _rankFilterTitleText;
    [SerializeField] TMP_Text _dateFilterTitleText;

    [SerializeField] Toggle[] _ownToggles;
    [SerializeField] Toggle[] _rankToggles;
    [SerializeField] Toggle[] _dateToggles;

    [Header("Ranking")]
    [SerializeField] GameObject _rankingContent;
    [SerializeField] TMP_Text _rankingTitleText;
    [SerializeField] Transform _rankingScrollContent;
    [SerializeField] Transform _myRankingContent;

    [Header("Shop")]
    [SerializeField] GameObject _shopContent;
    [SerializeField] Button _shopAdBtn;
    [SerializeField] Transform _shopScrollContent;

    [Header("Setting")]
    [SerializeField] SettingPage _settingPage;

    FSM<InnerPageState> _pageFsm;

    TopElementPresenter _topElementPresenter;
    public TopElementPresenter TopElementPresenter {  get { return _topElementPresenter; } }

    IAssetService _currencyService;
    IArtDataService _artDataLoaderService;
  
    private async void Start()
    {
        ServiceLocater.ReturnSoundPlayer().PlayBGM(ISoundPlayable.SoundName.LobbyBGM);

        string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;
        _currencyService = new CurrencyService();
        _artDataLoaderService = new ArtDataLoaderService();

        Dictionary<int, ArtData> artDatas = await _artDataLoaderService.GetArtData(userId);
        if (artDatas == null) return;

        int money = await _currencyService.GetCurrency(userId);
        if (money == -1) return;

        AddressableLoader addressableHandler = FindObjectOfType<AddressableLoader>();
        if (addressableHandler == null) return;

        _effectFactory = new EffectFactory(addressableHandler.EffectAssets);
        _dotFactory = new DotFactory(addressableHandler.DotAssets);

        StageUIFactory stageUIFactory = new StageUIFactory(
           addressableHandler.SpawnableUIAssets[SpawnableUI.Name.StageSelectBtnUI],
            addressableHandler.StageRankIconAssets
       );

        ArtworkUIFactory artWorkUIFactory = new ArtworkUIFactory(
            addressableHandler.SpawnableUIAssets[SpawnableUI.Name.ArtworkUI],
            addressableHandler.ArtSpriteAssets,
            addressableHandler.ArtworkFrameAssets,
            addressableHandler.RankDecorationIconAssets
        );

        RankingUIFactory rankingUIFactory = new RankingUIFactory(
            addressableHandler.SpawnableUIAssets[SpawnableUI.Name.RankingUI],
            addressableHandler.RectProfileIconAssets
        );

        ShopBundleUIFactory shopBundleUIFactory = new ShopBundleUIFactory(
            addressableHandler.SpawnableUIAssets[SpawnableUI.Name.ShopBundleUI]
        );

        FilteredArtworkFactory filteredArtworkFactory = new FilteredArtworkFactory(
            addressableHandler.SpawnableUIAssets[SpawnableUI.Name.FilteredArtworkUI],
            addressableHandler.ArtSpriteAssets
        );

        FilterItemFactory filterItemFactory = new FilterItemFactory(
            addressableHandler.SpawnableUIAssets[SpawnableUI.Name.FilterItemUI],
            addressableHandler.RankBadgeIconAssets
        );

        TopElementModel topElementModel = new TopElementModel();
        _topElementPresenter = new TopElementPresenter(topElementModel);
        TopElementViewer topElementViewer = new TopElementViewer(
            _goldTxt,
            _homeBtn,
            _shopBtn,
            _rankingBtn,
            _settingBtn,
            _topElementPresenter);
        
        _topElementPresenter.InjectViewer(topElementViewer);
        _topElementPresenter.ChangeGoldCount(money);

        _topElementPresenter.OnClickShopBtn += () => 
        {
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick);
            _pageFsm.SetState(InnerPageState.Shop); 
        };

        _topElementPresenter.OnClickHomeBtn += () => 
        {
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick);
            _pageFsm.SetState(InnerPageState.Main); 
        };

        _topElementPresenter.OnClickRankingBtn += () => 
        {
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick);
            _pageFsm.SetState(InnerPageState.Ranking); 
        };

        _topElementPresenter.OnClickSettingBtn += () =>
        {
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick);
            _settingPage.TogglePanel();
        };

        SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();
        _settingPage.Initialize(
            data.UserName, 
            addressableHandler.CircleProfileIconAssets, 
            () => { _pageFsm.SetState(InnerPageState.Main); },
            () => { _pageFsm.ChangeLanguage(); }
        );

        _filterScrollUI.Initialize();
        _filterScrollUI.Activate(false);


        InnerPageState startState = InnerPageState.Main;
        if (data.GoToCollectPage == true)
        {
            startState = InnerPageState.Collection;
            ServiceLocater.ReturnSaveManager().ChangeGoToCollectPage(false);
        }
       

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
                _artworkInfoContent,
                _artworkCompleteRatioContent,

                _currentComplete,
                _currentCompleteRatio,

                _totalComplete,
                _totalCompleteRatio,

                _titleTxt,
                _descriptionTxt,
                _completeSlider,
                _leftCompleteText,
                _totalCompleteText,

                _stageNameTxt,
                _selectStageTitle,

                _selectStageContent,
                _stageUIContent,

                _stageUsedHintUseTitle,
                _stageWrongCountTitle,

                _exitBtn,
                _playCollectModeBtn,
                _stageDetailContent,
                _stageUsedHintUseCount,
                _stageWrongCount,


                _filterScrollUI,
                _filterOpenBtn,
                _filterExitBtn,
                _filterContent,
                _collectionRatioText,
                _collectionCheerText,

                _ownFilterTitleText,
                _rankFilterTitleText,
                _dateFilterTitleText,

                _ownToggles,
                _rankToggles,
                _dateToggles,

                _artworkScrollUI,

                artWorkUIFactory,
                stageUIFactory,
                filteredArtworkFactory,
                filterItemFactory,

                artDatas,
                addressableHandler.ArtworkJsonDataAssets, // 이거 매번 불러오기
                addressableHandler.CollectiveArtJsonAssets,
                _pageFsm)
            },
            { 
                InnerPageState.Ranking, new RankingPageState(
                _rankingContent,
                _rankingTitleText,
                _rankingScrollContent,
                _myRankingContent,
                rankingUIFactory,
                new Top10RankingService(),
                _pageFsm)
            },
            {
                InnerPageState.Shop, new ShopPageState(
                _shopContent,
                _shopAdBtn,
                _shopScrollContent,
                shopBundleUIFactory,
                _topElementPresenter,
                _currencyService,
                _pageFsm)
            },
        }, startState);
    }
}
