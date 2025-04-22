using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingPageModel
{
    bool _activeContent;
    string _rankingTitle;

    public RankingPageModel()
    {
        _activeContent = false;
        _rankingTitle = "";
    }

    public bool ActiveContent { get => _activeContent; set => _activeContent = value; }
    public string RankingTitle { get => _rankingTitle; set => _rankingTitle = value; }
}
