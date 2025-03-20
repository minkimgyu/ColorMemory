using UnityEngine;

abstract public class BaseFactory
{
    public virtual Effect Create(Effect.Name name) { return null; }
    public virtual Dot Create(Dot.Name name) { return null; }
    public virtual ArtworkUI Create(ArtName name, ArtworkUI.Type frameType) { return null; }
    public virtual RankingUI Create(RankingData data) { return null; }
}