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

        Dictionary<int, Sprite> _artSpriteAsserts;
        Dictionary<NetworkService.DTO.Rank, Sprite> _artworkFrameAssets;
        Dictionary<NetworkService.DTO.Rank, Sprite> _rankDecorationIconAssets;
        Dictionary<NetworkService.DTO.Rank, Sprite> _rankBadgeIconAssets;
        ArtworkDateWrapper _artworkJsonDataAssets;

        public ResultState(
            FSM<CollectMode.State> fsm,
            IArtDataService artDataLoaderService,

            Dictionary<int, Sprite> artSpriteAsserts,
            Dictionary<NetworkService.DTO.Rank, Sprite> artworkFrameAssets,
            Dictionary<NetworkService.DTO.Rank, Sprite> rankDecorationIconAssets,
            Dictionary<NetworkService.DTO.Rank, Sprite> rankBadgeIconAssets,
            ArtworkDateWrapper artworkJsonDataAssets,

            CollectStageUIPresenter collectStageUIPresenter,
            CollectMode.Data modeData) : base(fsm)
        {
            _artDataLoaderService = artDataLoaderService;

            _artSpriteAsserts = artSpriteAsserts;
            _artworkFrameAssets = artworkFrameAssets;
            _rankDecorationIconAssets = rankDecorationIconAssets;
            _rankBadgeIconAssets = rankBadgeIconAssets;
            _artworkJsonDataAssets = artworkJsonDataAssets;

            _collectStageUIPresenter = collectStageUIPresenter;
            _collectStageUIPresenter.GoToShareState += () => { _fsm.SetState(CollectMode.State.Share); };
            _collectStageUIPresenter.OnClickNextBtn += () => 
            {
                ServiceLocater.ReturnSaveManager().ChangeGoToCollectPage(true);
                ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.HomeScene);
            };

            _modeData = modeData;
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

            // 힌트, 틀린 개수 업데이트
            int totalGoBackCount = artData.Item1.TotalHints;
            int totalWrongCount = artData.Item1.TotalMistakes;
            int clearStageCount = 0;
            int totalStageCount = 0;

            foreach (var item in artData.Item1.Stages)
            {
                // 하나의 스테이지라도 클리어 하지 못한 경우
                if(item.Value.Status == StageStauts.Clear) clearStageCount++;
                totalStageCount++;
            }

            Sprite artworkSprite;
            Sprite rankFrameSprite;
            Sprite rankDecorationIconSprite;
            Sprite rankBadgeIconSprite;

            _collectStageUIPresenter.ActivatePlayPanel(false);
            _collectStageUIPresenter.ActivateGameResultPanel(true);

            ServiceLocater.ReturnSoundPlayer().PlayBGM(ISoundPlayable.SoundName.CollectResultBGM);

            artworkSprite = _artSpriteAsserts[saveData.SelectedArtworkKey];

            string artworkTitle = _artworkJsonDataAssets.Data[saveData.SelectedArtworkKey].Title;
            ApplyLocalization(artworkTitle);

            //Rank? getRank;
            int ownCount = artData.Item2;
            int unownedCount = artData.Item3;

            // 모든 스테이지를 클리어 했는지 확인 필요
            if (artData.Item1.Rank != Rank.NONE)
            {
                AdManager.Instance.ShowRewardedAd();

                string gameResultTitle = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.CompleteArtworkResultTitle);

                _collectStageUIPresenter.ChangeGameResultTitle(gameResultTitle);
                _collectStageUIPresenter.ActivateOpenShareBtnInteraction(true);
                ownCount += 1; // 하나 더 획득했으므로 +1 추가

                //getRank = await _artDataUpdaterService.UpdateArtData(artData.Item1);
                //if (getRank == null) return;
            }
            else
            {
                string gameResultTitle = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.ProgressArtworkResultTitle);

                _collectStageUIPresenter.ChangeGameResultTitle(gameResultTitle);
                _collectStageUIPresenter.ActivateOpenShareBtnInteraction(false);
                //getRank = Rank.NONE;
            }

            rankFrameSprite = _artworkFrameAssets[artData.Item1.Rank];
            rankDecorationIconSprite = _rankDecorationIconAssets[artData.Item1.Rank];
            rankBadgeIconSprite = _rankBadgeIconAssets[artData.Item1.Rank];

            _collectStageUIPresenter.ChangeRank(rankBadgeIconSprite, _rankInfo[artData.Item1.Rank].Item1, _rankInfo[artData.Item1.Rank].Item2, _rankColor[artData.Item1.Rank]);
            _collectStageUIPresenter.ChangeArtwork(artworkSprite, rankFrameSprite, rankDecorationIconSprite, artData.Item1.Rank != Rank.NONE);


            string usageFormat;
            if (totalGoBackCount > 1) usageFormat = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.Counts);
            else usageFormat = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.Count);

            string wrongFormat;
            if (totalWrongCount > 1) wrongFormat = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.Counts);
            else wrongFormat = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.Count);

            _collectStageUIPresenter.ChangeGetRank(totalGoBackCount, totalWrongCount, usageFormat, wrongFormat);

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
    }
}