using UnityEngine;

abstract public class BaseFactory
{
    public virtual Effect Create(Effect.Name name) { return null; }
    public virtual Dot Create(Dot.Name name) { return null; }
    public virtual ArtworkUI Create(ArtName name, ArtworkUI.Type frameType) { return null; }
    public virtual RankingUI Create(PersonalRankingData data) { return null; }
    public virtual ClearPatternUI Create(MapData data, Color[] pickColors) { return null; }
}