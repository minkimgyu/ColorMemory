using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPageModel
{
    bool _activeContent;
    string _shopAdBtnTxt;

    public ShopPageModel()
    {
        _activeContent = false;
    }

    public bool ActiveContent { get => _activeContent; set => _activeContent = value; }
    public string ShopAdBtnTxt { get => _shopAdBtnTxt; set => _shopAdBtnTxt = value; }
}
