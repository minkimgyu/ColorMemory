using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopElementModel
{
    int _goldCount;

    public TopElementModel()
    {
        _goldCount = 0;
    }

    public int GoldCount { get => _goldCount; set => _goldCount = value; }
}
