using System.Collections.Generic;
using UnityEngine;

abstract public class BaseFactory
{
    public virtual Effect Create(Effect.Name name) { return null; }
    public virtual Dot Create(Dot.Name name) { return null; }
    public virtual SpawnableUI Create(ArtName name, Rank frameType) { return null; }
    public virtual SpawnableUI Create(PersonalRankingData data) { return null; }
    public virtual SpawnableUI Create(int currentStageCount, int totalStageCount, MapData data, Color[] pickColors) { return null; }

    public virtual SpawnableUI Create(
        List<List<CollectiveArtData.Block>> blocks,
        List<List<CollectiveArtData.Color>> usedColors)
    { return default; }
}