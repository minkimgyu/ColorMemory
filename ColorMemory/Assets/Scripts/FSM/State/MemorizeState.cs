using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MemorizeState : BaseState<ChallengeStageController.State>
{
    Dot[,] _dots;
    Vector2Int _levelSize;
    MapData _mapData;

    Color[] _pickColors;

    Timer _timer;
    float _memorizeDuration;

    Func<Tuple<Dot[,], Dot[], MapData>> GetLevelData;

    ChallengeStageUIController _challengeStageUIController;

    public MemorizeState(
        FSM<ChallengeStageController.State> fsm,
        Color[] pickColors,
        float memorizeDuration,

        ChallengeStageUIController challengeStageUIController,
        Func<Tuple<Dot[,], Dot[], MapData>> GetLevelData
    ) : base(fsm)
    {
        _pickColors = pickColors;
        _memorizeDuration = memorizeDuration;
        _timer = new Timer();

        _challengeStageUIController = challengeStageUIController;
        this.GetLevelData = GetLevelData;
    }

    Color GetDotColor(int row, int col)
    {
        return _pickColors[_mapData.DotColor[row, col]];
    }

    public override void OnStateEnter()
    {
        // 초기화 진행
        Tuple<Dot[,], Dot[], MapData> levelData = GetLevelData();
        _dots = levelData.Item1;
        _mapData = levelData.Item3;
        _levelSize = new Vector2Int(_dots.GetLength(0), _dots.GetLength(1));

        for (int i = 0; i < _levelSize.x; i++)
        {
            for (int j = 0; j < _levelSize.y; j++)
            {
                _dots[i, j].Minimize();
            }
        }

        for (int i = 0; i < _levelSize.x; i++)
        {
            for (int j = 0; j < _levelSize.y; j++)
            {
                // 원래 레벨 색으로 변경해주기
                _dots[i, j].ChangeColor(GetDotColor(i, j));

                // 랜덤하게 키우기
                _dots[i, j].Maximize(1f);
            }
        }

        _timer.Start(_memorizeDuration);
    }

    public override void OnStateUpdate()
    {
        _challengeStageUIController.ChangeTime(_timer.LeftTime, 1 - _timer.Ratio);

        if (_timer.CurrentState == Timer.State.Finish)
        {
            // dot 뒤집는 코드 추가
            for (int i = 0; i < _levelSize.x; i++)
            {
                for (int j = 0; j < _levelSize.y; j++)
                {
                    Image.FillMethod fillMethod = (Image.FillMethod)Random.Range(0, 5);
                    float duration = Random.Range(0, 1.5f);

                    _dots[i, j].Fade(Color.white, fillMethod, duration);
                }
            }

            _timer.Reset(); // 타이머 리셋

            // 일정 시간 지나면 다음 State로 이동
            DOVirtual.DelayedCall(1.5f, () =>
            {
                _fsm.SetState(ChallengeStageController.State.Paint);
            });
            return;
        }
    }
}
