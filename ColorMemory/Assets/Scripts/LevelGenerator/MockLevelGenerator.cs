using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockLevelGenerator : ILevelGenerator
{
    ILevelGenerator _randomLevelGenerator;
    ILevelGenerator _customLevelGenerator;

    public MockLevelGenerator(ILevelGenerator randomLevelGenerator, ILevelGenerator customLevelGenerator)
    {
        _randomLevelGenerator = randomLevelGenerator;
        _customLevelGenerator = customLevelGenerator;
    }

    public bool CanGenerateLevelData()
    {
        return _randomLevelGenerator.CanGenerateLevelData() && _randomLevelGenerator.CanGenerateLevelData();
    }

    public MapData GenerateLevelData(int currentStageIndex) 
    {
        return _randomLevelGenerator.GenerateLevelData(currentStageIndex);
    }

    public MapData GenerateLevelData() 
    { 
        return _customLevelGenerator.GenerateLevelData(); 
    }
}
