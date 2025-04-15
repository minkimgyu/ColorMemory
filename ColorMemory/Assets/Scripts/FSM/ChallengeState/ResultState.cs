using NetworkService.DTO;
using NetworkService.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Challenge.ChallengeMode;

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

        async Task<List<PlayerRankingDTO>> GetRankingDataFromServer()
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
                Debug.Log("서버로 데이터를 전송하지 못 함");
                return null;
            }

            return playerScoreDTOs;
        }

        public override async void OnStateEnter()
        {
            bool isSuccess = await SendDataToServer();
            if (isSuccess == false) return;

            List<PlayerRankingDTO> playerScoreDTOs = await GetRankingDataFromServer();
            if (playerScoreDTOs == null) return;

            int money = GetMoney();
            _challengeStageUIPresenter.ActivateGameResultPanel(true);
            _challengeStageUIPresenter.ChangeResultGoldCount(money);

            string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;

            int myRankingIndex = -1;
            List<PersonalRankingData> rankingDatas = new List<PersonalRankingData>();
            for (int i = 0; i < playerScoreDTOs.Count; i++)
            {
                if(playerScoreDTOs[i].PlayerId == userId) myRankingIndex = i;
                rankingDatas.Add(new PersonalRankingData(playerScoreDTOs[i].IconId, playerScoreDTOs[i].Name, playerScoreDTOs[i].Score, i + 1));
            }

            for (int i = 0; i < rankingDatas.Count; i++)
            {
                SpawnableUI rankingUI = _rankingUIFactory.Create(rankingDatas[i]);
                rankingUI.ChangeSelect(false);
                Vector3 size;

                if(i == myRankingIndex)
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

            int totalCount = rankingDatas.Count; // 5개
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