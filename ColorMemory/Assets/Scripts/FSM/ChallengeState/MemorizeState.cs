using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Challenge
{
    public class MemorizeState : BaseState<ChallengeMode.State>
    {
        Dot[,] _dots;
        Vector2Int _levelSize;
        MapData _mapData;

        Color[] _pickColors;

        readonly Color _fadeColor = new Color(236f / 255f, 232f / 255f, 232f / 255f);

        Timer _timer;
        ChallengeMode.ModeData _modeData;

        Func<Tuple<Dot[,], Dot[], MapData>> GetStage;

        ChallengeStageUIPresenter _challengeStageUIPresenter;

        public MemorizeState(
            FSM<ChallengeMode.State> fsm,
            Color[] pickColors,
            ChallengeMode.ModeData modeData,

            ChallengeStageUIPresenter challengeStageUIPresenter,
            Func<Tuple<Dot[,], Dot[], MapData>> GetStage
        ) : base(fsm)
        {
            _pickColors = pickColors;
            _modeData = modeData;
            _timer = new Timer();

            _challengeStageUIPresenter = challengeStageUIPresenter;
            this.GetStage = GetStage;
        }

        Color GetDotColor(int row, int col)
        {
            return _pickColors[_mapData.DotColor[row, col]];
        }

        public override void OnStateEnter()
        {
            // �ʱ�ȭ ����
            Tuple<Dot[,], Dot[], MapData> levelData = GetStage();
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
                    // ���� ���� ������ �������ֱ�
                    _dots[i, j].ChangeColor(GetDotColor(i, j));

                    // �����ϰ� Ű���
                    _dots[i, j].Maximize(1f);
                }
            }

            _challengeStageUIPresenter.ActivateRememberPanel(true);
            _challengeStageUIPresenter.ChangeTotalTime(_modeData.MemorizeDuration);
            _timer.Start(_modeData.MemorizeDuration);
        }

        public override void OnStateUpdate()
        {
            _challengeStageUIPresenter.ChangeLeftTime(_timer.LeftTime, 1 - _timer.Ratio);

            if (_timer.CurrentState == Timer.State.Finish)
            {
                _challengeStageUIPresenter.ActivateRememberPanel(false);

                // dot ������ �ڵ� �߰�
                for (int i = 0; i < _levelSize.x; i++)
                {
                    for (int j = 0; j < _levelSize.y; j++)
                    {
                        Image.FillMethod fillMethod = (Image.FillMethod)Random.Range(0, 5);
                        float duration = Random.Range(0, 1.5f);

                        _dots[i, j].Fade(_fadeColor, fillMethod, duration);
                    }
                }


                _timer.Reset(); // Ÿ�̸� ����

                // ���� �ð� ������ ���� State�� �̵�
                DOVirtual.DelayedCall(1.5f, () =>
                {
                    _fsm.SetState(ChallengeMode.State.Paint);
                });
                return;
            }
        }
    }
}