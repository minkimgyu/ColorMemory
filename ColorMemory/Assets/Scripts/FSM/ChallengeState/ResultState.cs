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

        async Task<bool> SendDataToServer()
        {
            ScoreManager scoreManager = new ScoreManager();
            MoneyManager moneyManager = new MoneyManager();

            try
            {
                await scoreManager.UpdatePlayerWeeklyScoreAsync("testId1", _modeData.MyScore);

                int currentMoneyInServer = await moneyManager.GetMoneyAsync("testId1");
                int useMoney = currentMoneyInServer - _modeData.GoldCount;

                await moneyManager.PayPlayerMoneyAsync("testId1", useMoney);
                await moneyManager.EarnPlayerMoneyAsync("testId1", _modeData.MyScore);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                Debug.Log("������ �����͸� �������� �� ��");
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
                playerScoreDTOs = await scoreManager.GetSurroundingWeeklyRankingAsync("testId1", 2);

            }
            catch (Exception e)
            {
                Debug.Log(e);
                Debug.Log("������ �����͸� �������� �� ��");
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

            _challengeStageUIPresenter.ActivateGameResultPanel(true);
            _challengeStageUIPresenter.ChangeResultGoldCount(_modeData.MyScore);

            int myRankingIndex = -1;
            List<PersonalRankingData> rankingDatas = new List<PersonalRankingData>();
            for (int i = 0; i < playerScoreDTOs.Count; i++)
            {
                if(playerScoreDTOs[i].PlayerId == "testId1") myRankingIndex = i;
                rankingDatas.Add(new PersonalRankingData(1, playerScoreDTOs[i].Name, playerScoreDTOs[i].Score, i + 1));
            }

            for (int i = 0; i < rankingDatas.Count; i++)
            {
                SpawnableUI rankingUI = _rankingUIFactory.Create(rankingDatas[i]);
                rankingUI.ChangeSelect(false);
                rankingUI.ChangeScale(Vector3.one * 0.8f);

                if(i == myRankingIndex)
                {
                    rankingUI.ChangeSelect(true);
                    rankingUI.ChangeScale(Vector3.one);
                }

                _challengeStageUIPresenter.AddRanking(rankingUI);
            }

            int totalCount = rankingDatas.Count; // 5��
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