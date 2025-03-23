using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionPageState : BaseState<HomePage.InnerPageState>
{
    ArtworkFactory _artworkFactory;

    CollectPagePresenter _collectPagePresenter;

    public CollectionPageState(
        Button homeBtn,
        GameObject collectionContent,
        TMP_Text titleTxt,
        TMP_Text descriptionTxt,
        ArtworkScrollUI artworkScroll,
        Button selectBtn,

        ArtworkFactory artworkFactory,

        Dictionary<ArtName, ArtData> artworkDatas,
        Dictionary<ArtName, CollectiveArtData> artDatas,
        FSM<HomePage.InnerPageState> fsm) : base(fsm)
    {
        _artworkFactory = artworkFactory;

        int artDataCount = Enum.GetValues(typeof(ArtName)).Length;
        artworkScroll.SetUp(artDataCount);

        CollectPageModel collectPageModel = new CollectPageModel(artworkDatas, artDatas);
        _collectPagePresenter = new CollectPagePresenter(collectPageModel);
        CollectPageViewer collectPageViewer = new CollectPageViewer(
            collectionContent,
            titleTxt,
            descriptionTxt,
            selectBtn,
            artworkScroll,
            _collectPagePresenter);

        _collectPagePresenter.InjectViewer(collectPageViewer);

        _collectPagePresenter.ActiveContent(false);
    }

    public override void OnClickRankingBtn()
    {
        _fsm.SetState(HomePage.InnerPageState.Ranking);
    }

    public override void OnClickHomeBtn()
    {
        _fsm.SetState(HomePage.InnerPageState.Main);
    }

    List<ArtworkUI> artworkUIs = new List<ArtworkUI>();

    public override void OnStateEnter()
    {
        foreach (ArtName name in System.Enum.GetValues(typeof(ArtName)))
        {
            ArtworkUI artwork = _artworkFactory.Create(name, ArtworkUI.Type.Bronze);
            artworkUIs.Add(artwork);

            _collectPagePresenter.AddArtwork(artwork);
        }

        _collectPagePresenter.ActiveContent(true); // home ╢щ╬фаж╠Б
    }

    public override void OnStateExit()
    {
        for (int i = 0; i < artworkUIs.Count; i++)
        {
            artworkUIs[i].DestroyObject();
            artworkUIs.RemoveAt(i);
            i--;
        }
        _collectPagePresenter.ActiveContent(false); // home ╢щ╬фаж╠Б
    }
}
