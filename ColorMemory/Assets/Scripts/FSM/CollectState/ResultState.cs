using NetworkService.DTO;
using NetworkService.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;

namespace Collect
{
    public class ResultState : BaseState<CollectMode.State>
    {
        CollectStageUIPresenter _collectStageUIPresenter;
        CollectMode.Data _modeData;

        IArtDataService _artDataLoaderService;
        IArtDataService _artDataUpdaterService;

        public ResultState(
            FSM<CollectMode.State> fsm,
            IArtDataService artDataLoaderService,
            IArtDataService artDataUpdaterService,
            CollectStageUIPresenter collectStageUIPresenter,
            CollectMode.Data modeData) : base(fsm)
        {
            _artDataLoaderService = artDataLoaderService;
            _artDataUpdaterService = artDataUpdaterService;

            _collectStageUIPresenter = collectStageUIPresenter;
            _modeData = modeData;
        }

        public override void OnClickNextBtn()
        {
            ServiceLocater.ReturnSaveManager().ChangeGoToCollectPage(true);
            ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.HomeScene);
        }

        void ApplyLocalization(string artworkTitle)
        {
            _collectStageUIPresenter.ChangeArtworkTitle(artworkTitle);

            string getRankTitle = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.GetRankTitle);
            string hintUsageTitle = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.HintUsage);
            string wrongCountTitle = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.WrongCount);

            _collectStageUIPresenter.ChangeGetRankTitle(getRankTitle, hintUsageTitle, wrongCountTitle);

            string myCollectionTitle = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.MyCollectionTitle);
            string currentCollectionComplete = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.CurrentCollectionComplete);
            string totalCollectionComplete = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.TotalCollectionComplete);

            _collectStageUIPresenter.ChangeMyCollectionTitle(myCollectionTitle, currentCollectionComplete, totalCollectionComplete);
        }

        public override async void OnStateEnter()
        {
            string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;
            SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();

            Tuple<PlayerArtworkDTO, int, int> artData = await _artDataLoaderService.GetArtData(userId, saveData.SelectedArtworkKey);
            if (artData == null) return;

            bool clearThisStage = true;

            // 힌트, 틀린 개수 업데이트
            int totalGoBackCount = artData.Item1.TotalHints;
            int totalWrongCount = artData.Item1.TotalMistakes;
            int clearStageCount = 0;
            int totalStageCount = 0;

            foreach (var item in artData.Item1.Stages)
            {
                // 하나의 스테이지라도 클리어 하지 못한 경우
                if(item.Value.Status != StageStauts.Clear) clearThisStage = false;
                else clearStageCount++;

                totalStageCount++;
            }

            Sprite artworkSprite;
            Sprite rankFrameSprite;
            Sprite rankDecorationIconSprite;
            Sprite rankBadgeIconSprite;

            AddressableLoader addressableHandler = UnityEngine.Object.FindObjectOfType<AddressableLoader>();
            if (addressableHandler == null) return;

            _collectStageUIPresenter.ActivatePlayPanel(false);
            _collectStageUIPresenter.ActivateGameResultPanel(true);

            artworkSprite = addressableHandler.ArtSpriteAsserts[saveData.SelectedArtworkKey];

            string artworkTitle = addressableHandler.ArtworkJsonDataAssets[saveData.Language].Data[saveData.SelectedArtworkKey].Title;
            ApplyLocalization(artworkTitle);

            Rank? getRank;
            int ownCount = artData.Item2;
            int unownedCount = artData.Item3;

            // 모든 스테이지를 클리어 했는지 확인 필요
            if (clearThisStage == true)
            {
                _collectStageUIPresenter.ChangeGameResultTitle(true);
                ownCount += 1; // 하나 더 획득했으므로 +1 추가
                artData.Item1.HasIt = true;  // HasIt 업데이트

                getRank = await _artDataUpdaterService.UpdateArtData(artData.Item1);
                if (getRank == null) return;
            }
            else
            {
                _collectStageUIPresenter.ChangeGameResultTitle(false);
                getRank = Rank.NONE;
            }

            rankFrameSprite = addressableHandler.ArtworkFrameAssets[getRank.Value];
            rankDecorationIconSprite = addressableHandler.RankDecorationIconAssets[getRank.Value];
            rankBadgeIconSprite = addressableHandler.RankBadgeIconAssets[getRank.Value];

            _collectStageUIPresenter.ChangeRank(rankBadgeIconSprite, _rankInfo[getRank.Value].Item1, _rankInfo[getRank.Value].Item2, _rankColor[getRank.Value]);
            _collectStageUIPresenter.ChangeArtwork(artworkSprite, rankFrameSprite, rankDecorationIconSprite, clearThisStage);
            _collectStageUIPresenter.ChangeGetRank(totalGoBackCount, totalWrongCount);

            float currentRatio = (float)clearStageCount / totalStageCount;
            float totalRatio = (float)ownCount / (ownCount + unownedCount);
            _collectStageUIPresenter.ChangeCollectionRatio(currentRatio, totalRatio);
        }

        readonly Dictionary<Rank, Tuple<bool, string>> _rankInfo = new Dictionary<Rank, Tuple<bool, string>>
        {
            { Rank.COPPER, new Tuple<bool, string>(true, "Bronze") },
            { Rank.SILVER, new Tuple<bool, string>(true, "Silver") },
            { Rank.GOLD, new Tuple<bool, string>(true, "Gold") },
            { Rank.NONE, new Tuple<bool, string>(false, "No Clear") },
        };

        readonly Dictionary<Rank, Color> _rankColor = new Dictionary<Rank, Color>
        {
            { Rank.COPPER, new Color(250f/255f, 214f/255f, 190f/255f) },
            { Rank.SILVER, new Color(207f/255f, 207f/255f, 207f/255f) },
            { Rank.GOLD, new Color(255f/255f, 245f/255f, 173f/255f) },
            { Rank.NONE, new Color(236f/255f, 232f/255f, 232f/255f) },
        };

        public override void OnStateExit()
        {
            _collectStageUIPresenter.ActivateGameResultPanel(false);
        }
    }
}