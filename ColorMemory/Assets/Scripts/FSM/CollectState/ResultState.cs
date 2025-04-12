using NetworkService.DTO;
using NetworkService.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.U2D;

namespace Collect
{
    public class ResultState : BaseState<CollectMode.State>
    {
        CollectStageUIPresenter _collectStageUIPresenter;
        CollectMode.Data _modeData;

        public ResultState(
            FSM<CollectMode.State> fsm,
            CollectStageUIPresenter collectStageUIPresenter,
            CollectMode.Data modeData) : base(fsm)
        {
            _collectStageUIPresenter = collectStageUIPresenter;
            _modeData = modeData; 
        }

        public override void OnClickNextBtn()
        {
            ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.HomeScene);
        }

        async Task<Rank?> UpdateArtworkToServer(PlayerArtworkDTO dTO)
        {
            ArtworkManager artworkManager = new ArtworkManager();
            Rank? rank;

            try
            {
                rank = await artworkManager.UpdatePlayerArtworkAsync(dTO);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                Debug.Log("서버로 데이터를 전송하지 못함");
                return null;
            }

            return rank;
        }

        async Task<Tuple<List<PlayerArtworkDTO>, int, int>> GetArtDataFromServer()
        {
            ArtworkManager artworkManager = new ArtworkManager();
            List<PlayerArtworkDTO> ownedArtworkDTOs, unownedArtworkDTOs;

            try
            {
                string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;
                ownedArtworkDTOs = await artworkManager.GetPlayerArtworksAsync(userId, true);
                unownedArtworkDTOs = await artworkManager.GetPlayerArtworksAsync(userId, false);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                Debug.Log("서버로부터 데이터를 받아오지 못함");
                return null;
            }

            int ownCount = ownedArtworkDTOs.Count;
            int unownedCount = unownedArtworkDTOs.Count;

            ownedArtworkDTOs.AddRange(unownedArtworkDTOs); // list1에 list2 요소 추가
            return new Tuple<List<PlayerArtworkDTO>, int, int>(ownedArtworkDTOs, ownCount, unownedCount);
        }

        public override async void OnStateEnter()
        {
            Tuple<List<PlayerArtworkDTO>, int, int> artDatas = await GetArtDataFromServer();
            if (artDatas == null) return;

            SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();
            PlayerArtworkDTO artworkDTO = artDatas.Item1.Find(x => x.ArtworkId == saveData.SelectedArtworkKey);

            bool clearThisStage = true;

            // 힌트, 틀린 개수 업데이트
            int totalGoBackCount = artworkDTO.TotalHints;
            int totalWrongCount = artworkDTO.TotalMistakes;
            int clearStageCount = 0;
            int totalStageCount = 0;

            foreach (var item in artworkDTO.Stages)
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

            AddressableHandler addressableHandler = UnityEngine.Object.FindObjectOfType<AddressableHandler>();
            if (addressableHandler == null) return;

            _collectStageUIPresenter.ActivatePlayPanel(false);
            _collectStageUIPresenter.ActivateGameResultPanel(true);

            artworkSprite = addressableHandler.ArtSpriteAsserts[saveData.SelectedArtworkKey];

            Rank? getRank;
            int haveArtworkCount = artDatas.Item2;
            int totalArtworkCount = artDatas.Item3;

            // 모든 스테이지를 클리어 했는지 확인 필요
            if (clearThisStage == true)
            {
                _collectStageUIPresenter.ChangeGameResultTitle(true);
                haveArtworkCount += 1; // 하나 더 획득했으므로 +1 추가
                artworkDTO.HasIt = true;  // HasIt 업데이트

                getRank = await UpdateArtworkToServer(artworkDTO);
                if (getRank == null) return;
            }
            else
            {
                _collectStageUIPresenter.ChangeGameResultTitle(false);
                getRank = Rank.NONE;
            }

            rankFrameSprite = addressableHandler.ArtworkFrameAsserts[getRank.Value];
            rankDecorationIconSprite = addressableHandler.RankDecorationIconAssets[getRank.Value];
            rankBadgeIconSprite = addressableHandler.RankBadgeIconAssets[getRank.Value];

            _collectStageUIPresenter.ChangeRank(rankBadgeIconSprite, _rankInfo[getRank.Value].Item1, _rankInfo[getRank.Value].Item2, _rankColor[getRank.Value]);
            _collectStageUIPresenter.ChangeArtwork(artworkSprite, rankFrameSprite, rankDecorationIconSprite, clearThisStage);
            _collectStageUIPresenter.ChangeGetRank(totalGoBackCount, totalWrongCount);

            float currentRatio = (float)clearStageCount / totalStageCount;
            float totalRatio = (float)haveArtworkCount / (haveArtworkCount + totalArtworkCount);
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