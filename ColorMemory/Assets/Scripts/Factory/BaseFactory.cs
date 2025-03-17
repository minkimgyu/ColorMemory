using UnityEngine;

abstract public class BaseFactory
{
    public virtual Effect Create(Effect.Name name) { return null; }
    public virtual Dot Create(Dot.Name name) { return null; }
}