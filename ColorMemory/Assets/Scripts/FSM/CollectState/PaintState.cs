using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Collect
{
    public class PaintState : BaseState<CollectMode.State>
    {
        MapData _mapData;

        Dot[,] _dots;
        Dot[] _penDots;
        CollectMode.Data _modeData;
        Vector2Int[] _closePoints;
        Vector2Int _levelSize;

        //Timer _timer;
        //float _paintDuration;

        Func<Tuple<Dot[,], Dot[], MapData>> GetLevelData;
        CollectStageUIPresenter _collectStageUIPresenter;

        public PaintState(
            FSM<CollectMode.State> fsm,
            CollectMode.Data modeData,
            CollectStageUIPresenter collectStageUIPresenter,
            Func<Tuple<Dot[,], Dot[], MapData>> GetLevelData
        ) : base(fsm)
        {
            _modeData = modeData;
            _closePoints = new Vector2Int[4]
            {
            new Vector2Int(-1, 0), // ↑
            new Vector2Int(0, 1), // →
            new Vector2Int(1, 0), // ↓
            new Vector2Int(0, -1), // ←
            //new Vector2Int(-1, 1), // ↗
            //new Vector2Int(1, 1), // ↘
            //new Vector2Int(1, -1), // ↙
            //new Vector2Int(-1, -1), // ↖
            };

            //_paintDuration = paintDuration;
            //_timer = new Timer();

            _collectStageUIPresenter = collectStageUIPresenter;

            _collectStageUIPresenter.OnClickGoBackHint += OnClickGoBackHint;

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

        void ChangePenDotColorCount()
        {
            int row = _mapData.DotColor.GetLength(0);
            int col = _mapData.DotColor.GetLength(1);

            List<int> pickColor = _mapData.PickColors;
            int[] colorArr = new int[_modeData.PickColors.Length];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    int colorIndex = _mapData.DotColor[i, j];
                    colorArr[colorIndex]++;
                }
            }

            // 방문한 건 따로 체크해주기
            for (int x = 0; x < _levelSize.x; x++)
            {
                for (int y = 0; y < _levelSize.y; y++)
                {
                    if (_visit[x, y] != -1)
                    {
                        colorArr[_visit[x, y]]--;
                    }
                }
            }

            for (int i = 0; i < _penDots.Length; i++)
            {
                _penDots[i].ChangeColorCount(colorArr[pickColor[i]]);
            }
        }

        void OnClickGoBackHint()
        {
            _goBackActivated = true;
            _goBackCount++;

            for (int i = 0; i < _penDots.Length; i++)
            {
                _penDots[i].Minimize();
            }

            _collectStageUIPresenter.ActivateDetailContent(false);
            _collectStageUIPresenter.ChangeHintInfoText($"힌트를 사용할수록 높은 랭크를 받을 확률이 떨어져요!");
            SaveData save = ServiceLocater.ReturnSaveManager().GetSaveData();

            _modeData.GoBackCount += 1;
            _fsm.SetState(CollectMode.State.Memorize);
        }

        bool _goBackActivated = false;
        int _goBackCount = 0;

        readonly Color _fadeColor = new Color(236f / 255f, 232f / 255f, 232f / 255f);

        public override void OnStateEnter()
        {
            // 초기화 진행
            Tuple<Dot[,], Dot[], MapData> levelData = GetLevelData();
            _dots = levelData.Item1;
            _penDots = levelData.Item2;
            _mapData = levelData.Item3;
            _levelSize = new Vector2Int(_dots.GetLength(0), _dots.GetLength(1));

            _collectStageUIPresenter.ActivateDetailContent(true);
            _collectStageUIPresenter.ChangeCurrentHintUsage(_goBackCount);
            _collectStageUIPresenter.ChangeCurrentWrongCount(_modeData.WrongCount);

            _collectStageUIPresenter.ChangeHintInfoText($"힌트를 {_goBackCount}번 사용했어요");

            if (_goBackActivated == true)
            {
                _goBackActivated = false;

                for (int i = 0; i < _levelSize.x; i++)
                {
                    for (int j = 0; j < _levelSize.y; j++)
                    {
                        if (_visit[i, j] == -1) continue;

                        int colorIndex = _visit[i, j];
                        _dots[i, j].Pop(_modeData.PickColors[colorIndex]);
                    }
                }
            }
            else
            {
                _selectedColorIndex = _mapData.PickColors[0];
                _penDots[0].SeletDotToggle();

                _visit = new int[_levelSize.x, _levelSize.y];

                for (int x = 0; x < _levelSize.x; x++)
                {
                    for (int y = 0; y < _levelSize.y; y++)
                    {
                        _visit[x, y] = -1;
                    }
                }

                ChangePenDotColorCount();
            }

            for (int i = 0; i < _penDots.Length; i++)
            {
                _penDots[i].Maximize(0.5f);
            }
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
            return _modeData.PickColors[_mapData.DotColor[index.x, index.y]];
        }

        public override void OnClickDot(Vector2Int index)
        {
            Debug.Log(index);

            if (_selectedColorIndex != _mapData.DotColor[index.x, index.y])
            {
                _dots[index.x, index.y].XSlide(Color.red);
                _modeData.WrongCount += 1;
                _collectStageUIPresenter.ChangeCurrentWrongCount(_modeData.WrongCount);
                return;
            }
            SpreadColor(index);
        }

        int _selectedColorIndex = 0;

        // color pen dot의 경우
        public override void OnClickDot(int index)
        {
            _selectedColorIndex = index;
        }

        // 색 같은 거끼리 bfs 돌려서 확인해줌
        void SpreadColor(Vector2Int index)
        {
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

            ChangePenDotColorCount();

            bool canClear = CanClearStage();
            if (canClear == false) return;

            //float leftRatio = _timer.Ratio;
            //_timer.Reset(); // 타이머 리셋

            

            _goBackCount = 0;
            _fsm.SetState(CollectMode.State.Clear);
        }
    }
}