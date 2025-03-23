using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPatternCreater
{
    ClearPatternUI _clearPatternPrefab;

    public ClearPatternCreater(ClearPatternUI clearPatternPrefab)
    {
        _clearPatternPrefab = clearPatternPrefab;
    }

    public ClearPatternUI Create(MapData data, Color[] pickColors)
    {
        ClearPatternUI clearPatternUI = Object.Instantiate(_clearPatternPrefab);
        clearPatternUI.Initialize(data, pickColors);

        return clearPatternUI;
    }
}

public class ClearPatternFactory : BaseFactory
{
    ClearPatternCreater _clearPatternCreater;

    public ClearPatternFactory(ClearPatternUI clearPatternPrefab)
    {
        _clearPatternCreater = new ClearPatternCreater(clearPatternPrefab);
    }

    public override ClearPatternUI Create(MapData data, Color[] pickColors)
    {
        return _clearPatternCreater.Create(data, pickColors);
    }
}
