using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Challenge
{
    public class ResultState : BaseState<ChallengeMode.State>
    {
        ChallengeStageUIPresenter _challengeStageUIPresenter;
        RankingUIFactory _rankingUIFactory;

        ChallengeMode.ModeData _modeData;

        public ResultState(
            FSM<ChallengeMode.State> fsm,
            RankingUIFactory rankingUIFactory,
            ChallengeStageUIPresenter challengeStageUIPresenter,
            ChallengeMode.ModeData modeData) : base(fsm)
        {
            _rankingUIFactory = rankingUIFactory;
            _challengeStageUIPresenter = challengeStageUIPresenter;
            _modeData = modeData;
        }

        RankingData GetRankingData()
        {
            List<PersonalRankingData> topRankingDatas = new List<PersonalRankingData>();
            string[] names = { "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Hank", "Ivy", "Jack" };

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
                RankingIconName iconName = (RankingIconName)UnityEngine.Random.Range(0, Enum.GetValues(typeof(RankingIconName)).Length);
                string name = names[i];
                int score = UnityEngine.Random.Range(0, 100000000);
                int rank = i + 1; // 1���� �����ϴ� ����


                await scoreManager.UpdatePlayerWeeklyScoreAsync(userId, _modeData.MyScore);

                int currentMoneyInServer = await moneyManager.GetMoneyAsync(userId);
                int useMoney = currentMoneyInServer - _modeData.GoldCount;

                int money = GetMoney();
                await moneyManager.PayPlayerMoneyAsync(userId, useMoney);
                await moneyManager.EarnPlayerMoneyAsync(userId, money);
            }

            // ���������ֱ�
            PersonalRankingData myRankingData = new PersonalRankingData((RankingIconName)1, "Meal", 10000000, 15);

            RankingData rankingData = new RankingData(topRankingDatas, myRankingData);
            return rankingData;
        }

        public override void OnClickRetryBtn()
        {
            ScoreManager scoreManager = new ScoreManager();
            List<PlayerRankingDTO> playerScoreDTOs = new List<PlayerRankingDTO>();

            try
            {
                string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;
                playerScoreDTOs = await scoreManager.GetSurroundingWeeklyRankingByIdAsync(userId, 2);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                Debug.Log("������ �����͸� �������� �� ��");
                return null;
            }

            return playerScoreDTOs;
        }

        public override void OnClickExitBtn()
        {
            ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.HomeScene);
        }

            List<PlayerRankingDTO> playerScoreDTOs = await GetRankingDataFromServer();
            if (playerScoreDTOs == null) return;

            int money = GetMoney();
            _challengeStageUIPresenter.ActivateGameResultPanel(true);
            _challengeStageUIPresenter.ChangeResultGoldCount(money);

            // ���������ֱ�
            RankingData rankingData = GetRankingData();

            for (int i = 0; i < rankingData.OtherRankingDatas.Count; i++)
            {
                SpawnableUI rankingUI = _rankingUIFactory.Create(rankingData.OtherRankingDatas[i]);
                _challengeStageUIPresenter.AddRanking(rankingUI);
            }

            SpawnableUI myRankingUI = _rankingUIFactory.Create(rankingData.MyRankingData);
            _challengeStageUIPresenter.AddRanking(myRankingUI, true);

            int totalCount = rankingData.OtherRankingDatas.Count + 1;
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