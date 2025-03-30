using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomePage : MonoBehaviour
{
    public enum InnerPageState
    {
        Main, // -> Collection, Ranking, Shop
        Collection, // -> Main, Ranking
        Ranking, // -> Main, Shop
        Shop // -> Main, Ranking
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

    [SerializeField] ArtworkScrollUI _artworkScrollUI;

    [Header("Ranking")]
    [SerializeField] GameObject _rankingContent;
    [SerializeField] Transform _rankingScrollContent;
    [SerializeField] Transform _myRankingContent;

    [Header("Shop")]
    [SerializeField] GameObject _shopContent;
    [SerializeField] Transform _shopScrollContent;

    FSM<InnerPageState> _pageFsm;

    private void Start()
    {
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
            addressableHandler.RankingIconAssets
        );

        _shopBtn.onClick.AddListener(() => { _pageFsm.OnClickShopBtn(); });
        _homeBtn.onClick.AddListener(() => { _pageFsm.OnClickHomeBtn(); });
        _rankingBtn.onClick.AddListener(() => { _pageFsm.OnClickRankingBtn(); });

        _settingBtn.onClick.AddListener(() => { _sideSheetUI.TogglePanel(); });

        SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();
        _goldTxt.text = data.Money.ToString();

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
                _artworkScrollUI,
                artWorkUIFactory,
                stageUIFactory,
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
                _pageFsm)
            },
        }, InnerPageState.Main);
    }
}
