using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPagePresenter
{
    CollectPageViewer _collectPageViewer;
    CollectPageModel _collectPageModel;

    public int ArtworkIndex { get { return _collectPageModel.ArtworkIndex; } }

    public CollectPagePresenter(CollectPageModel collectPageModel)
    {
        _collectPageModel = collectPageModel;
    }

    public void InjectViewer(CollectPageViewer collectPageViewer)
    {
        _collectPageViewer = collectPageViewer;
    }

    public void ActiveContent(bool active)
    {
        _collectPageModel.ActiveContent = active;
        _collectPageViewer.ActiveContent(_collectPageModel.ActiveContent);
    }



    public void OnArtworkScrollDragEnd(int index)
    {
        _collectPageModel.ArtworkIndex = index;
        // 설명 텍스트 바꿔주기
        // 
    }
}
