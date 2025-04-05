using System.Collections.Generic;
using UnityEngine;

abstract public class BaseFactory
{
    public virtual Effect Create(Effect.Name name) { return null; }
    public virtual Dot Create(Dot.Name name) { return null; }


    public virtual SpawnableUI Create(NetworkService.DTO.Rank rank, string description) { return default; }
    public virtual SpawnableUI Create(string description) { return default; }

    public virtual SpawnableUI Create(string name, string description, int reward, int price) { return null; }
    public virtual SpawnableUI Create(int artworkIndex, NetworkService.DTO.Rank frameType, bool hasIt) { return null; }
    public virtual SpawnableUI Create(int artworkIndex, string title, bool hasIt) { return null; }
    public virtual SpawnableUI Create(PersonalRankingData data) { return null; }
    public virtual SpawnableUI Create(int currentStageCount, int totalStageCount, MapData data, Color[] pickColors) { return null; }
    public virtual SpawnableUI Create() { return default; }
}