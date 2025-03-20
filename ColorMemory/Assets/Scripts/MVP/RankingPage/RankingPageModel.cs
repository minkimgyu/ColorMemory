using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingPageModel
{
    bool _activeContent;

    public RankingPageModel()
    {
        _activeContent = false;
    }

    public bool ActiveContent { get => _activeContent; set => _activeContent = value; }
}
