using UnityEngine;

abstract public class BaseFactory
{
    public virtual BaseEffect Create(BaseEffect.Name name) { return null; }
    public virtual Dot Create(Dot.Name name) { return null; }
}