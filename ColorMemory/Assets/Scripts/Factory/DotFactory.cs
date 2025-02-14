using System.Collections.Generic;
using UnityEngine;

public class DotCreater
{
    Dot _dotPrefab;

    public DotCreater(Dot dotPrefab)
    {
        _dotPrefab = dotPrefab;
    }

    public Dot Create()
    {
        Dot dot = Object.Instantiate(_dotPrefab);
        dot.Initialize();

        return dot;
    }
}

public class DotFactory : BaseFactory
{
    Dictionary<Dot.Name, DotCreater> _dotCreater;

    public DotFactory(Dictionary<Dot.Name, Dot> dotPrefab)
    {
        _dotCreater = new Dictionary<Dot.Name, DotCreater>();
        _dotCreater.Add(Dot.Name.Basic, new DotCreater(dotPrefab[Dot.Name.Basic]));
        _dotCreater.Add(Dot.Name.ColorPen, new DotCreater(dotPrefab[Dot.Name.ColorPen]));
    }

    public override Dot Create(Dot.Name name)
    {
        return _dotCreater[name].Create();
    }
}
