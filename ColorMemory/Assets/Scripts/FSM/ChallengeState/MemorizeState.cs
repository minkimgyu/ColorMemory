using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

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
        List<LevelData> _stageDatas;

        Func<Tuple<Dot[,], Dot[], MapData>> GetStage;

        ChallengeStageUIPresenter _challengeStageUIPresenter;

        public MemorizeState(
            FSM<ChallengeMode.State> fsm,
            Color[] pickColors,
            ChallengeMode.ModeData modeData,
            List<LevelData> stageDatas,

            ChallengeStageUIPresenter challengeStageUIPresenter,
            Func<Tuple<Dot[,], Dot[], MapData>> GetStage
        ) : base(fsm)
        {
            _pickColors = pickColors;
            _modeData = modeData;
            _stageDatas = stageDatas;
            _timer = new Timer();

            _challengeStageUIPresenter = challengeStageUIPresenter;
            _challengeStageUIPresenter.OnClickSkipBtn += GoToPaintState;
            this.GetStage = GetStage;
        }

        Color GetDotColor(int row, int col)
        {
            return _pickColors[_mapData.DotColor[row, col]];
        }

        public override void OnStateEnter()
        {
            // 초기화 진행
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
                    // 원래 레벨 색으로 변경해주기
                    _dots[i, j].ChangeColor(GetDotColor(i, j));

                    // 랜덤하게 키우기
                    _dots[i, j].Maximize(1f);
                }
            }

            int index = Mathf.Clamp(_modeData.StageCount - 1, 0, _stageDatas.Count - 1);
            float memorizeDuration = _stageDatas[index].MemorizeDuration;

            _challengeStageUIPresenter.ActivateBottomContent(false);
            _challengeStageUIPresenter.ActivateSkipBtn(true);

            string rememberTxt = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.RememberTitle);
            _challengeStageUIPresenter.ActivateRememberPanel(true, rememberTxt);
            _challengeStageUIPresenter.ChangeTotalTime(memorizeDuration);
            _timer.Start(memorizeDuration);
        }

        void GoToPaintState()
        {
            _challengeStageUIPresenter.ActivateBottomContent(true);
            _challengeStageUIPresenter.ActivateSkipBtn(false);

            string rememberTxt = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.RememberTitle);
            _challengeStageUIPresenter.ActivateRememberPanel(false, rememberTxt);

            // dot 뒤집는 코드 추가
            for (int i = 0; i < _levelSize.x; i++)
            {
                for (int j = 0; j < _levelSize.y; j++)
                {
                    _dots[i, j].Expand(_fadeColor, 1.5f);
                }
            }

            _timer.Reset(); // 타이머 리셋

            // 일정 시간 지나면 다음 State로 이동
            DOVirtual.DelayedCall(1.5f, () =>
            {
                // 만약 현재 상태가 다른 상태라면 실행되지 못하게 막아야함
                if (_fsm.CurrentState != ChallengeMode.State.Memorize) return;
                _fsm.SetState(ChallengeMode.State.Paint);
            });
        }

        public override void OnStateUpdate()
        {
            _challengeStageUIPresenter.ChangeLeftTime(_timer.LeftTime, 1 - _timer.Ratio);

            if (_timer.CurrentState == Timer.State.Finish)
            {
                GoToPaintState();
                return;
            }
        }
    }
}