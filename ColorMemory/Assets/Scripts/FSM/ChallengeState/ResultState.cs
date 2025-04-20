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
            _modeData = modeData;
        }

        public override void OnClickRetryBtn()
        {
            ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.ChallengeScene);
        }

        public override void OnClickExitBtn()
        {
            ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.HomeScene);
        }

        const int moneyDivideValue = 10;

        int GetMoney()
        {
            return _modeData.MyScore / moneyDivideValue;
        }

        async Task<bool> SendDataToServer()
        {
            ScoreManager scoreManager = new ScoreManager();
            MoneyManager moneyManager = new MoneyManager();

            try
            {
                string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;


                await scoreManager.UpdatePlayerWeeklyScoreAsync(userId, _modeData.MyScore);

                int currentMoneyInServer = await moneyManager.GetMoneyAsync(userId);
                int useMoney = currentMoneyInServer - _modeData.GoldCount;

                int money = GetMoney();
                await moneyManager.PayPlayerMoneyAsync(userId, useMoney);
                await moneyManager.EarnPlayerMoneyAsync(userId, money);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                Debug.Log("서버로 데이터를 전송하지 못 함");
                return false;
            }

            return true;
        }

        const int _nearRange = 2;

        public override async void OnStateEnter()
        {
            string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;

            bool canUpdate = await _transactionService.ProcessTransaction(userId, _modeData.GoldCount, GetMoney());
            if (canUpdate == false) return;

            Tuple<List<PersonalRankingData>, int> rankingData = await _nearRankingService.GetNearRankingData(_nearRange, userId);
            if (rankingData == null) return;

            int money = GetMoney();
            _challengeStageUIPresenter.ActivateGameResultPanel(true);
            _challengeStageUIPresenter.ChangeResultGoldCount(money);


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

            int totalCount = rankingData.Item1.Count; // 5개
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