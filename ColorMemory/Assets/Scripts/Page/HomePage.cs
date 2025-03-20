using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomePage : MonoBehaviour
{
    public enum InnerPageState
    {
        Main, // -> Collection, Ranking
        Collection, // -> Main, Ranking
        Ranking, // -> Main
    }


    GameMode.Type _type = GameMode.Type.Collect;
    EffectFactory _effectFactory;
    DotFactory _dotFactory;

    const int _dotSize = 6;
    const float _contentSize = 0.8f;

    [SerializeField] GameObject _mainContent;
    [SerializeField] Image _modeTitleImg;
    [SerializeField] ToggleBtn _toggleBtn;
    [SerializeField] Transform _dotParent;

    [SerializeField] Button _playBtn;

    [SerializeField] Button _homeBtn;

    [SerializeField] GameObject _collectionContent;
    [SerializeField] TMP_Text _titleTxt;
    [SerializeField] TMP_Text _descriptionTxt;

    [SerializeField] ScrollUI _artworkScroll;
    [SerializeField] Button _selectBtn;

    [SerializeField] Button _rankingBtn;

    [SerializeField] GameObject _rankingContent;
    [SerializeField] Transform _rankingScrollContent;


    FSM<InnerPageState> _pageFsm;

    private void Start()
    {
        AddressableHandler addressableHandler = FindObjectOfType<AddressableHandler>();
        if (addressableHandler == null) return;

        _effectFactory = new EffectFactory(addressableHandler.EffectAssets);
        _dotFactory = new DotFactory(addressableHandler.DotAssets);
        ArtworkFactory artworkFactory = new ArtworkFactory(
            addressableHandler.ArtworkAsset,
            addressableHandler.ArtSpriteAsserts,
            addressableHandler.ArtworkFrameAsserts);

        RankingFactory rankingFactory = new RankingFactory(addressableHandler.RankingAsset, addressableHandler.RankingIconAssets);

        _homeBtn.onClick.AddListener(() => { _pageFsm.OnClickHomeBtn(); });
        _rankingBtn.onClick.AddListener(() => { _pageFsm.OnClickRankingBtn(); });

        _pageFsm = new FSM<InnerPageState>();
        _pageFsm.Initialize(new Dictionary<InnerPageState, BaseState<InnerPageState>>
        {
            { 
                InnerPageState.Main, new MainPageState(
                _type,
                _effectFactory,
                _dotFactory,
                _modeTitleImg,
                _toggleBtn,
                _dotParent,
                _playBtn,
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
                _artworkScroll,
                _selectBtn,
                artworkFactory,
                addressableHandler.ArtworkJsonAsset.Data,
                addressableHandler.CollectiveArtJsonAsserts,
                _pageFsm)
            },
            { 
                InnerPageState.Ranking, new RankingPageState(
                _rankingContent,
                _rankingScrollContent,
                rankingFactory,
                _pageFsm)
            },
        }, InnerPageState.Main);
    }
}
