using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPageModel
{
    bool _activeContent;

    public ShopPageModel()
    {
        _activeContent = false;
    }

    public bool ActiveContent { get => _activeContent; set => _activeContent = value; }
}
