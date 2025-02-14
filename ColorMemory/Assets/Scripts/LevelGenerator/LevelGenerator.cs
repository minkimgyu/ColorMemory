using UnityEngine;

abstract public class LevelGenerator
{
    public abstract bool CanGenerateLevelData();
    public abstract MapData GenerateLevelData();
}