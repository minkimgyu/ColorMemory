using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Challenge
{
    public class ResultState : BaseState<ChallengeMode.State>
    {
        ChallengeStageUIPresenter _challengeStageUIPresenter;
        RankingFactory _factory;
        System.Func<ChallengeMode.Data> GetChallengeModeData;

        public ResultState(
            FSM<ChallengeMode.State> fsm,
            RankingFactory factory,
            ChallengeStageUIPresenter challengeStageUIPresenter,
            System.Func<ChallengeMode.Data> GetChallengeModeData) : base(fsm)
        {
            _factory = factory;
            _challengeStageUIPresenter = challengeStageUIPresenter;
            this.GetChallengeModeData = GetChallengeModeData;
        }

        RankingData GetRankingData()
        {
            List<PersonalRankingData> topRankingDatas = new List<PersonalRankingData>();
            string[] names = { "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Hank", "Ivy", "Jack" };

            int count = 10; // 생성할 데이터 개수
            for (int i = 0; i < count; i++)
            {
                RankingIconName iconName = (RankingIconName)UnityEngine.Random.Range(0, Enum.GetValues(typeof(RankingIconName)).Length);
                string name = names[i];
                int score = UnityEngine.Random.Range(0, 100000000);
                int rank = i + 1; // 1부터 시작하는 순위

                topRankingDatas.Add(new PersonalRankingData(iconName, name, score, rank));
            }

            // 생성시켜주기
            PersonalRankingData myRankingData = new PersonalRankingData((RankingIconName)1, "Meal", 10000000, 15);

            RankingData rankingData = new RankingData(topRankingDatas, myRankingData);
            return rankingData;
        }

        public override void OnStateEnter()
        {
            _challengeStageUIPresenter.ActivateGameResultPanel(true);

            ChallengeMode.Data modeData = GetChallengeModeData();
            _challengeStageUIPresenter.ChangeResultScore(modeData.MyScore);
            _challengeStageUIPresenter.ChangeGoldCount(modeData.MyScore);

            // 생성시켜주기
            RankingData rankingData = GetRankingData();

            for (int i = 0; i < rankingData.OtherRankingDatas.Count; i++)
            {
                RankingUI rankingUI = _factory.Create(rankingData.OtherRankingDatas[i]);
                _challengeStageUIPresenter.AddRanking(rankingUI);
            }

            RankingUI myRankingUI = _factory.Create(rankingData.MyRankingData);
            _challengeStageUIPresenter.AddRanking(myRankingUI, true);

            int totalCount = rankingData.OtherRankingDatas.Count + 1;
            int middleIndex = totalCount / 2;
            _challengeStageUIPresenter.SetUpRankingScroll(totalCount, middleIndex);
        }

        public override void OnStateExit()
        {
            _challengeStageUIPresenter.ActivateGameResultPanel(false);
        }
    }
}