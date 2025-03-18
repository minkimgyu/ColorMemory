using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPageModel
{
    bool _activeContent;
    string _title;
    string _description;

    int _artworkIndex;

    public CollectPageModel()
    {
        _activeContent = false;
        _title = "";
        _description = "";
        _artworkIndex = 0;
    }

    public bool ActiveContent { get => _activeContent; set => _activeContent = value; }
    public string Title { get => _title; set => _title = value; }
    public string Description { get => _description; set => _description = value; }
    public int ArtworkIndex { get => _artworkIndex; set => _artworkIndex = value; }
}
