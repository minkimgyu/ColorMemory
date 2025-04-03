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

        async Task<Tuple<List<PlayerArtworkDTO>, int, int>> GetArtDataFromServer()
        {
            ArtworkManager artworkManager = new ArtworkManager();
            List<PlayerArtworkDTO> ownedArtworkDTOs, unownedArtworkDTOs;

            try
            {
                ownedArtworkDTOs = await artworkManager.GetOwnedArtworksAsync("testId1");
                unownedArtworkDTOs = await artworkManager.GetUnownedArtworksAsync("testId1");
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                Debug.Log("서버로부터 데이터를 받아오지 못함");
                return null;
            }

            int ownCount = ownedArtworkDTOs.Count;
            int unownedCount = ownedArtworkDTOs.Count;

            ownedArtworkDTOs.AddRange(unownedArtworkDTOs); // list1에 list2 요소 추가
            return new Tuple<List<PlayerArtworkDTO>, int, int>(ownedArtworkDTOs, ownCount, unownedCount);
        }

        public override async void OnStateEnter()
        {
            Tuple<List<PlayerArtworkDTO>, int, int> artDatas = await GetArtDataFromServer();
            if (artDatas == null) return;

            _collectStageUIPresenter.ActivatePlayPanel(false);
            _collectStageUIPresenter.ActivateGameResultPanel(true);

            AddressableHandler addressableHandler = UnityEngine.Object.FindObjectOfType<AddressableHandler>();
            if (addressableHandler == null) return;

            SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();

            Sprite artworkSprite = addressableHandler.ArtSpriteAsserts[saveData.SelectedArtworkKey];
            Sprite rankSprite = addressableHandler.ArtworkFrameAsserts[NetworkService.DTO.Rank.COPPER];

            // 저장하고 랭크 받기
            // public async Task<Rank> UpdatePlayerArtworkAsync(PlayerArtworkDTO playerArtwork)

            // public async Task<List<PlayerArtworkDTO>> GetOwnedArtworksAsync(string playerId)
            // public async Task<List<PlayerArtworkDTO>> GetUnownedArtworksAsync(string playerId)
            // 호출해서 모은 비율 가져오기

            PlayerArtworkDTO artworkDTO = artDatas.Item1.Find(x => x.ArtworkId == saveData.SelectedArtworkKey);

            int totalGoBackCount = 0;
            int totalWrongCount = 0;

            foreach (var item in artworkDTO.Stages)
            {
                totalGoBackCount += item.Value.HintUsage;
                totalWrongCount += item.Value.IncorrectCnt;
            }

            float ratio = (float)artDatas.Item2 / (float)(artDatas.Item2 + artDatas.Item3);

            _collectStageUIPresenter.ChangeRank(_rankColor[artworkDTO.Rank], addressableHandler.RankIconAssets[artworkDTO.Rank], _rankString[artworkDTO.Rank]);
            _collectStageUIPresenter.ChangeArtwork(artworkSprite, rankSprite);
            _collectStageUIPresenter.ChangeGetRank(totalGoBackCount, totalWrongCount);
            _collectStageUIPresenter.ChangeCollectionRatio(ratio);
        }

        readonly Dictionary<Rank, Color> _rankColor = new Dictionary<Rank, Color>
        {
            { Rank.COPPER, new Color(255f / 255f, 248f / 255f, 199f / 255f) },
            { Rank.SILVER, new Color(175f / 255f, 175f / 255f, 175f / 255f) },
            { Rank.GOLD, new Color(236f / 255f, 176f / 255f, 87f / 255f) },
        };

        readonly Dictionary<Rank, string> _rankString = new Dictionary<Rank, string>
        {
            { Rank.COPPER, "Copper" },
            { Rank.SILVER, "Silver" },
            { Rank.GOLD, "Gold" },
        };

        public override void OnStateExit()
        {
            _collectStageUIPresenter.ActivateGameResultPanel(false);
        }
    }
}