using System.Collections.Generic;
using UnityEngine;

abstract public class BaseFactory
{
    public virtual Effect Create(Effect.Name name) { return null; }
    public virtual Dot Create(Dot.Name name) { return null; }
    public virtual SpawnableUI Create(int artworkIndex, NetworkService.DTO.Rank frameType) { return null; }
    public virtual SpawnableUI Create(PersonalRankingData data) { return null; }
    public virtual SpawnableUI Create(int currentStageCount, int totalStageCount, MapData data, Color[] pickColors) { return null; }
    public virtual SpawnableUI Create(Vector2Int index) { return default; }
}