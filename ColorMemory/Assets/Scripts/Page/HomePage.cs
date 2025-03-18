using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    [SerializeField] GameObject _collectionContent;
    [SerializeField] Transform _artworkParent;
    [SerializeField] Button _selectBtn;




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
                _collectionContent,
                _artworkParent,
                _selectBtn,
                artworkFactory,
                null,
                _pageFsm)
            },
            { 
                InnerPageState.Ranking, new RankingPageState(
                _pageFsm)
            },
        }, InnerPageState.Main);
    }
}
