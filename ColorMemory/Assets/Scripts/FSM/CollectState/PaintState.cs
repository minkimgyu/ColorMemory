using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

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
            _closePoints = new Vector2Int[8]
            {
            new Vector2Int(-1, 0), // ��
            new Vector2Int(0, 1), // ��
            new Vector2Int(1, 0), // ��
            new Vector2Int(0, -1), // ��
            new Vector2Int(-1, 1), // ��
            new Vector2Int(1, 1), // ��
            new Vector2Int(1, -1), // ��
            new Vector2Int(-1, -1), // ��
            };

            //_paintDuration = paintDuration;
            //_timer = new Timer();

            _collectStageUIPresenter = collectStageUIPresenter;
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

            // �湮�� �� ���� üũ���ֱ�
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

        public override void OnClickGoBackHint()
        {
            SaveData save = ServiceLocater.ReturnSaveManager().GetSaveData();

            _modeData.IsPlayed[save.SelectedArtworkSectionIndex.x, save.SelectedArtworkSectionIndex.y] = true;
            _modeData.GoBackCount[save.SelectedArtworkSectionIndex.x, save.SelectedArtworkSectionIndex.y] += 1;
            _fsm.SetState(CollectMode.State.Memorize);
        }

        public override void OnStateEnter()
        {
            // �ʱ�ȭ ����
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

            ChangePenDotColorCount();

            //_challengeStageUIPresenter.ChangeTotalTime(_paintDuration);

            DOVirtual.DelayedCall(0.5f, () =>
            {
                //_timer.Start(_paintDuration);
            });
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

            if (_selectedColorIndex != _mapData.DotColor[index.x, index.y]) return;
            SpreadColor(index);
        }

        int _selectedColorIndex = 0;
        void ChangeSelectedColor(int index) => _selectedColorIndex = index;

        // color pen dot�� ���
        public override void OnClickDot(int index)
        {
            Debug.Log(index);

            switch (_state)
            {
                case RevealSameColorHintState.Idle:
                    ChangeSelectedColor(index);
                    break;
                case RevealSameColorHintState.SelectColor:

                    for (int x = 0; x < _levelSize.x; x++)
                    {
                        for (int y = 0; y < _levelSize.y; y++)
                        {
                            int colorIndex = _mapData.DotColor[x, y];
                            if (colorIndex == index && _visit[x, y] < 0) // ���� �����̰� �湮���� ���� ���
                            {
                                Color nearDotColor = GetDotColor(new Vector2Int(x, y));
                                _dots[x, y].Pop(nearDotColor); // �� �ٲ��ִ� �ڵ� �߰�
                                _visit[x, y] = colorIndex;
                            }
                        }
                    }

                    ChangePenDotColorCount();

                    Time.timeScale = 1;
                    _state = RevealSameColorHintState.Idle;
                    _collectStageUIPresenter.ActivateHintPanel(false);

                    bool canClear = CanClearStage();
                    if (canClear == false) return;

                    //_timer.Reset(); // Ÿ�̸� ����
                    _fsm.SetState(CollectMode.State.Clear);

                    break;
                default:
                    break;
            }
        }

        public enum RevealSameColorHintState
        {
            Idle,
            SelectColor,
        }

        RevealSameColorHintState _state = RevealSameColorHintState.Idle;

        public override void OnClickRevealSameColorHint()
        {
            Time.timeScale = 0;
            _state = RevealSameColorHintState.SelectColor;
            _collectStageUIPresenter.ActivateHintPanel(true); // active panel �������ֱ�
        }

        public override void OnClickRandomFillHint()
        {
            RandomFindHint();
        }

        void RandomFindHint()
        {
            while (true)
            {
                int randomRow = Random.Range(0, _levelSize.x);
                int randomCol = Random.Range(0, _levelSize.y);

                if (_visit[randomRow, randomCol] >= 0) continue; // �̹� �湮�� �����̶�� �ٽ� ����

                SpreadColor(new Vector2Int(randomRow, randomCol));
                break;
            }
        }

        // �� ���� �ų��� bfs ������ Ȯ������
        void SpreadColor(Vector2Int index)
        {
            Queue<Tuple<Vector2Int, int>> queue = new Queue<Tuple<Vector2Int, int>>();

            Color dotColor = GetDotColor(index);
            _dots[index.x, index.y].Pop(dotColor); // �� �ٲ��ִ� �ڵ� �߰�

            _visit[index.x, index.y] = _mapData.DotColor[index.x, index.y];
            Tuple<Vector2Int, int> startTuple = new Tuple<Vector2Int, int>(index, _mapData.DotColor[index.x, index.y]);

            queue.Enqueue(startTuple);

            while (queue.Count > 0) // bfs�� ���� �迭 ä���
            {
                Tuple<Vector2Int, int> front = queue.Dequeue();

                List<Vector2Int> nearPoints = GetNearPoints(front.Item1);
                for (int i = 0; i < nearPoints.Count; i++)
                {
                    if (_visit[nearPoints[i].x, nearPoints[i].y] >= 0) continue; // �̹� �湮�ߴٸ� continue
                    if (_mapData.DotColor[nearPoints[i].x, nearPoints[i].y] != front.Item2) continue; // ���� �ٸ��ٸ� continue

                    _visit[nearPoints[i].x, nearPoints[i].y] = front.Item2;

                    Color nearDotColor = GetDotColor(nearPoints[i]);
                    _dots[nearPoints[i].x, nearPoints[i].y].Pop(nearDotColor); // �� �ٲ��ִ� �ڵ� �߰�

                    Tuple<Vector2Int, int> nearTuple = new Tuple<Vector2Int, int>(nearPoints[i], front.Item2);
                    queue.Enqueue(nearTuple);
                }
            }

            ChangePenDotColorCount();

            bool canClear = CanClearStage();
            if (canClear == false) return;

            //float leftRatio = _timer.Ratio;
            //_timer.Reset(); // Ÿ�̸� ����

            _fsm.SetState(CollectMode.State.Clear);
        }
    }
}