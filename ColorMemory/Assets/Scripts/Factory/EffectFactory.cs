using UnityEngine;
using System.Collections.Generic;

abstract public class EffectCreater
{
    public virtual Effect Create() { return null; }
}

public class DotEffectCreater : EffectCreater
{
    Effect _effectPrefab;

    public DotEffectCreater(Effect effectPrefab)
    {
        _effectPrefab = effectPrefab;
    }

    public override Effect Create()
    {
        Effect effect = Object.Instantiate(_effectPrefab);
        effect.Initialize();

        return effect;
    }
}

public class EffectFactory : BaseFactory
{
    [SerializeField] Effect _circleEffectPrefab;
    [SerializeField] Effect _rectEffectPrefab;

    Dictionary<Effect.Name, EffectCreater> _effectCreater;

    public EffectFactory(Dictionary<Effect.Name, Effect> effectPrefab)
    {
        _effectCreater = new Dictionary<Effect.Name, EffectCreater>();
        _effectCreater.Add(Effect.Name.CircleEffect, new DotEffectCreater(effectPrefab[Effect.Name.CircleEffect]));
        _effectCreater.Add(Effect.Name.RectEffect, new DotEffectCreater(effectPrefab[Effect.Name.RectEffect]));
        _effectCreater.Add(Effect.Name.XEffect, new DotEffectCreater(effectPrefab[Effect.Name.XEffect]));
    }

    public override Effect Create(Effect.Name name)
    {
        return _effectCreater[name].Create();
    }
}
