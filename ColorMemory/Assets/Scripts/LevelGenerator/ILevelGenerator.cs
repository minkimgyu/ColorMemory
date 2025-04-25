using UnityEngine;

public interface ILevelGenerator
{
    bool CanGenerateLevelData();
    MapData GenerateLevelData(int currentStageIndex) { return default; }
    MapData GenerateLevelData() { return default; }
}