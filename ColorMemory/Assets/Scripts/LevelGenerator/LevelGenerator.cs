using UnityEngine;

public interface LevelGenerator
{
    bool CanGenerateLevelData();
    MapData GenerateLevelData(int currentStageIndex) { return default; }
    MapData GenerateLevelData() { return default; }
}