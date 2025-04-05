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
                ownedArtworkDTOs = await artworkManager.GetOwnedArtworksAsync(userId);
                unownedArtworkDTOs = await artworkManager.GetUnownedArtworksAsync(userId);
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

            // 힌트, 틀린 개수 업데이트
            int totalGoBackCount = 0;
            int totalWrongCount = 0;

            foreach (var item in artworkDTO.Stages)
            {
                totalGoBackCount += item.Value.HintUsage;
                totalWrongCount += item.Value.IncorrectCnt;
            }

            artworkDTO.HasIt = true;  // HasIt 업데이트
            // artworkDTO.ObtainedDate = DateTime.Now; // 획득 날짜 업데이트
            Rank? getRank = await UpdateArtworkToServer(artworkDTO);
            if (getRank == null) return;

            AddressableHandler addressableHandler = UnityEngine.Object.FindObjectOfType<AddressableHandler>();
            if (addressableHandler == null) return;

            _collectStageUIPresenter.ActivatePlayPanel(false);
            _collectStageUIPresenter.ActivateGameResultPanel(true);

            Sprite artworkSprite = addressableHandler.ArtSpriteAsserts[saveData.SelectedArtworkKey];
            Sprite rankFrameSprite = addressableHandler.ArtworkFrameAsserts[getRank.Value];
            Sprite rankIconSprite = addressableHandler.RankIconAssets[getRank.Value];

            float ratio = (float)artDatas.Item2 / (artDatas.Item2 + artDatas.Item3);

            _collectStageUIPresenter.ChangeRank(rankIconSprite, _rankString[getRank.Value]);
            _collectStageUIPresenter.ChangeArtwork(artworkSprite, rankFrameSprite);
            _collectStageUIPresenter.ChangeGetRank(totalGoBackCount, totalWrongCount);
            _collectStageUIPresenter.ChangeCollectionRatio(ratio);
        }

        readonly Dictionary<Rank, string> _rankString = new Dictionary<Rank, string>
        {
            { Rank.COPPER, "Bronze" },
            { Rank.SILVER, "Silver" },
            { Rank.GOLD, "Gold" },
        };

        public override void OnStateExit()
        {
            _collectStageUIPresenter.ActivateGameResultPanel(false);
        }
    }
}