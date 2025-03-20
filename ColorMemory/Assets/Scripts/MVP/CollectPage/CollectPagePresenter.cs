using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPagePresenter
{
    CollectPageViewer _collectPageViewer;
    CollectPageModel _collectPageModel;


    public CollectPagePresenter(CollectPageModel collectPageModel)
    {
        _collectPageModel = collectPageModel;
    }

    public void InjectViewer(CollectPageViewer collectPageViewer)
    {
        _collectPageViewer = collectPageViewer;
        ChangeArtworkDescription(_collectPageModel.ArtworkIndex);
    }

    public void ActiveContent(bool active)
    {
        _collectPageModel.ActiveContent = active;
        _collectPageViewer.ActiveContent(_collectPageModel.ActiveContent);
    }

    public void AddArtwork(ArtworkUI artwork)
    {
        _collectPageViewer.AddArtwork(artwork);
    }

    public void ChangeArtworkDescription(int index)
    {
        _collectPageModel.ArtworkIndex = index;
        ArtData artData = _collectPageModel.ArtworkDatas[(ArtName)_collectPageModel.ArtworkIndex];
        _collectPageViewer.ChangeArtDescription(artData.Title, artData.Description);
    }
}
