using NetworkService.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

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

        public override void OnStateEnter()
        {
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


            _collectStageUIPresenter.ChangeArtwork(artworkSprite, rankSprite);
            _collectStageUIPresenter.ChangeGetRank(_modeData.TotalGoBackCount, _modeData.TotalWrongCount);
            //_collectStageUIPresenter.ChangeRank();
            //_collectStageUIPresenter.ChangeCollectionRatio();
        }

        public override void OnStateExit()
        {
            _collectStageUIPresenter.ActivateGameResultPanel(false);
        }
    }
}