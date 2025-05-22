using NetworkService.DTO;
using NetworkService.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Challenge
{
    public class ResultState : BaseState<ChallengeMode.State>
    {
        ChallengeStageUIPresenter _challengeStageUIPresenter;
        RankingUIFactory _rankingUIFactory;

        ChallengeMode.ModeData _modeData;
        IRankingService _nearRankingService;
        IRankingService _weeklyScoreUpdateService;
        IAssetService _transactionService;

        public ResultState(
            FSM<ChallengeMode.State> fsm,
            IRankingService nearRankingService,
            IRankingService weeklyScoreUpdateService,
            IAssetService transactionService,
            RankingUIFactory rankingUIFactory,
            ChallengeStageUIPresenter challengeStageUIPresenter,
            ChallengeMode.ModeData modeData) : base(fsm)
        {
            _nearRankingService = nearRankingService;
            _weeklyScoreUpdateService = weeklyScoreUpdateService;
            _transactionService = transactionService;

            _rankingUIFactory = rankingUIFactory;
            _challengeStageUIPresenter = challengeStageUIPresenter;

            _challengeStageUIPresenter.OnClickRetryBtn += OnClickRetryBtn;
            _challengeStageUIPresenter.OnClickExitBtn += OnClickExitBtn;

            _modeData = modeData;
        }

        void OnClickRetryBtn()
        {
            ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.ChallengeScene);
        }

        void OnClickExitBtn()
        {
            ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.HomeScene);
        }

        const int moneyDivideValue = 10;

        int GetMoney()
        {
            return _modeData.MyScore / moneyDivideValue;
        }

        const int _nearRange = 2;

        public override async void OnStateEnter()
        {
            string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;

            bool canUpdate = await _transactionService.ProcessTransaction(userId, _modeData.GoldCount, GetMoney());
            if (canUpdate == false) return;

            bool updateResult = await _weeklyScoreUpdateService.UpdatePlayerWeeklyScore(_modeData.MyScore, userId);
            if (updateResult == false) return;

            Tuple<List<PersonalRankingData>, int> rankingData = await _nearRankingService.GetNearRankingData(_nearRange, userId);
            if (rankingData == null) return;

            int money = GetMoney();
            _challengeStageUIPresenter.ActivateGameResultPanel(true);

            string format = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.GetCoin);
            _challengeStageUIPresenter.ChangeResultGoldCount(money, format);


            for (int i = 0; i < rankingData.Item1.Count; i++)
            {
                SpawnableUI rankingUI = _rankingUIFactory.Create(rankingData.Item1[i]);
                rankingUI.ChangeSelect(false);
                Vector3 size;

                if(i == rankingData.Item2)
                {
                    rankingUI.ChangeSelect(true);
                    size = Vector3.one;
                }
                else
                {
                    size = Vector3.one * 0.8f;
                }

                _challengeStageUIPresenter.AddRanking(rankingUI, size);
            }

            int totalCount = rankingData.Item1.Count; // 5°³
            int middleIndex = totalCount / 2;
            _challengeStageUIPresenter.SetUpRankingScroll(totalCount, middleIndex);
        }

        public override void OnStateExit()
        {
            _challengeStageUIPresenter.RemoveAllRanking();
            _challengeStageUIPresenter.ActivateGameResultPanel(false);
        }
    }
}