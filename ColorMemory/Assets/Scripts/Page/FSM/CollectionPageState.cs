using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionPageState : BaseState<HomePage.InnerPageState>
{
    [SerializeField] GameObject _collectionContent;
    [SerializeField] Transform _artworkParent;
    [SerializeField] Button _selectBtn;

    ArtworkFactory _artworkFactory;
    Dictionary<ArtData.Name, ArtData> _artDatas;

    public CollectionPageState(
        GameObject collectionContent,
        Transform artworkParent,
        Button selectBtn,


        ArtworkFactory artworkFactory,
        Dictionary<ArtData.Name, ArtData> artDatas,
        FSM<HomePage.InnerPageState> fsm) : base(fsm)
    {
        _collectionContent = collectionContent;
        _artworkParent = artworkParent;
        _selectBtn = selectBtn;

        _artworkFactory = artworkFactory;
        _artDatas = artDatas;

        foreach (ArtData.Name name in System.Enum.GetValues(typeof(ArtData.Name)))
        {
            Artwork artwork = _artworkFactory.Create(name, Artwork.Type.Bronze);
            artwork.transform.SetParent(_artworkParent);
        }
    }
}
