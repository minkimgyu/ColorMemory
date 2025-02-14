using UnityEngine;
using System.Collections.Generic;

abstract public class EffectCreater
{
    public virtual BaseEffect Create() { return null; }
}

public class DotEffectCreater : EffectCreater
{
    BaseEffect _effectPrefab;

    public DotEffectCreater(BaseEffect effectPrefab)
    {
        _effectPrefab = effectPrefab;
    }

    public override BaseEffect Create()
    {
        BaseEffect effect = Object.Instantiate(_effectPrefab);
        effect.Initialize();

        return effect;
    }
}

public class EffectFactory : BaseFactory
{
    [SerializeField] BaseEffect _circleEffectPrefab;
    [SerializeField] BaseEffect _rectEffectPrefab;

    Dictionary<BaseEffect.Name, EffectCreater> _effectCreater;

    public EffectFactory(Dictionary<BaseEffect.Name, BaseEffect> effectPrefab)
    {
        _effectCreater = new Dictionary<BaseEffect.Name, EffectCreater>();
        _effectCreater.Add(BaseEffect.Name.CircleEffect, new DotEffectCreater(effectPrefab[BaseEffect.Name.CircleEffect]));
        _effectCreater.Add(BaseEffect.Name.RectEffect, new DotEffectCreater(effectPrefab[BaseEffect.Name.RectEffect]));
    }

    public override BaseEffect Create(BaseEffect.Name name)
    {
        return _effectCreater[name].Create();
    }
}
