using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.U2D.Aseprite;
using DG.Tweening;

public class PaintState : BaseState<ChallengeStageController.State>
{
    MapData _mapData;

    Dot[,] _dots;
    Dot[] _penDots;
    Color[] _pickColors;
    Vector2Int[] _closePoints;
    Vector2Int _levelSize;

    Timer _timer;
    float _paintDuration;

    Func<Tuple<Dot[,], Dot[], MapData>> GetLevelData;

    ChallengeStageUIController _challengeStageUIController;

    public PaintState(
        FSM<ChallengeStageController.State> fsm,
        Color[] pickColors,
        float paintDuration,
        ChallengeStageUIController challengeStageUIController,
        Func<Tuple<Dot[,], Dot[], MapData>> GetLevelData
    ) : base(fsm)
    {
        _pickColors = pickColors;
        _closePoints = new Vector2Int[8]
        {
            new Vector2Int(-1, 0), // ↑
            new Vector2Int(0, 1), // →
            new Vector2Int(1, 0), // ↓
            new Vector2Int(0, -1), // ←
            new Vector2Int(-1, 1), // ↗
            new Vector2Int(1, 1), // ↘
            new Vector2Int(1, -1), // ↙
            new Vector2Int(-1, -1), // ↖
        };

        _paintDuration = paintDuration;
        _timer = new Timer();

        _challengeStageUIController = challengeStageUIController;
        this.GetLevelData = GetLevelData;
    }

    int[,] _visit;

    bool CanClearStage()
    {
        bool canClear = true;

        for (int x = 0; x < _levelSize.x; x++)
        {
            for (int y = 0; y < _levelSize.y; y++)
            {
                if (_visit[x, y] == -1)
                {
                    canClear = false;
                    break;
                }
            }
        }

        return canClear;
    }

    public override void OnStateUpdate()
    {
        _challengeStageUIController.ChangeTime(_timer.LeftTime, 1 - _timer.Ratio);

        if (_timer.CurrentState == Timer.State.Finish)
        {
            _timer.Reset(); // 타이머 리셋
            _fsm.SetState(ChallengeStageController.State.Clear);
            return;
        }
    }

    public override void OnStateEnter()
    {
        // 초기화 진행
        Tuple<Dot[,], Dot[], MapData> levelData = GetLevelData();
        _dots = levelData.Item1;
        _penDots = levelData.Item2;
        _mapData = levelData.Item3;
        _levelSize = new Vector2Int(_dots.GetLength(0), _dots.GetLength(1));

        _selectedColorIndex = _mapData.PickColors[0];

        for (int i = 0; i < _penDots.Length; i++)
        {
            _penDots[i].Maximize(0.5f);
        }

        _visit = new int[_levelSize.x, _levelSize.y];

        for (int x = 0; x < _levelSize.x; x++)
        {
            for (int y = 0; y < _levelSize.y; y++)
            {
                _visit[x, y] = -1;
            }
        }

        _timer.Start(_paintDuration);
    }

    bool OutOfRange(Vector2Int point)
    {
        return point.x < 0 || point.y < 0 || point.x >= _levelSize.x || point.y >= _levelSize.y;
    }

    List<Vector2Int> GetNearPoints(Vector2Int point)
    {
        List<Vector2Int> nearPoints = new List<Vector2Int>();

        for (int i = 0; i < _closePoints.Length; i++)
        {
            Vector2Int nearPoint = point + _closePoints[i];

            if (OutOfRange(nearPoint)) continue;
            nearPoints.Add(nearPoint);
        }

        return nearPoints;
    }

    Color GetDotColor(Vector2Int index)
    {
        return _pickColors[_mapData.DotColor[index.x, index.y]];
    }

    public override void OnClickDot(Vector2Int index)
    {
        Debug.Log(index);
        SpreadColor(index);
    }

    public override void OnClickDot(int index)
    {
        Debug.Log(index);
        ChangeSelectedColor(index);
    }

    int _selectedColorIndex = 0;
    void ChangeSelectedColor(int index) => _selectedColorIndex = index;

    // 색 같은 거끼리 bfs 돌려서 확인해줌
    void SpreadColor(Vector2Int index)
    {
        if (_selectedColorIndex != _mapData.DotColor[index.x, index.y]) return;

        Queue<Tuple<Vector2Int, int>> queue = new Queue<Tuple<Vector2Int, int>>();

        Color dotColor = GetDotColor(index);
        _dots[index.x, index.y].Pop(dotColor); // 색 바꿔주는 코드 추가

        _visit[index.x, index.y] = _mapData.DotColor[index.x, index.y];
        Tuple<Vector2Int, int> startTuple = new Tuple<Vector2Int, int>(index, _mapData.DotColor[index.x, index.y]);

        queue.Enqueue(startTuple);

        while (queue.Count > 0) // bfs를 통한 배열 채우기
        {
            Tuple<Vector2Int, int> front = queue.Dequeue();

            List<Vector2Int> nearPoints = GetNearPoints(front.Item1);
            for (int i = 0; i < nearPoints.Count; i++)
            {
                if (_visit[nearPoints[i].x, nearPoints[i].y] >= 0) continue; // 이미 방문했다면 continue
                if (_mapData.DotColor[nearPoints[i].x, nearPoints[i].y] != front.Item2) continue; // 색이 다르다면 continue

                _visit[nearPoints[i].x, nearPoints[i].y] = front.Item2;

                Color nearDotColor = GetDotColor(nearPoints[i]);
                _dots[nearPoints[i].x, nearPoints[i].y].Pop(nearDotColor); // 색 바꿔주는 코드 추가

                Tuple<Vector2Int, int> nearTuple = new Tuple<Vector2Int, int>(nearPoints[i], front.Item2);
                queue.Enqueue(nearTuple);
            }
        }

        bool canClear = CanClearStage();
        if (canClear == false) return;

        _timer.Reset(); // 타이머 리셋
        _fsm.SetState(ChallengeStageController.State.Clear);
    }
}
