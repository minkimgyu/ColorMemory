using System.Collections.Generic;
using UnityEngine;

public class ChallengeMode : GameMode
{
    

    ChallengeStageController _stageController;

    public override void OnGameClear()
    {

    }

    public override void OnGameFail()
    {

    }

    private void Start()
    {
        _stageController = GetComponent<ChallengeStageController>();
        _stageController.Initialize();
    }
}
