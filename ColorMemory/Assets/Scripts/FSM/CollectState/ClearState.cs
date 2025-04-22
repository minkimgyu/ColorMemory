using UnityEngine;
using System;
using DG.Tweening;
using NetworkService.DTO;
using NetworkService.Manager;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;

namespace Collect
{
    public class ClearState : BaseState<CollectMode.State>
    {
        Action DestroyDots;
        CollectMode.Data _modeData;
        CollectArtData _artData;

        Func<Tuple<Dot[,], Dot[], MapData>> GetLevelData;
        CollectStageUIPresenter _collectStageUIPresenter;

        IArtDataService _artDataLoaderService;
        IArtDataService _artDataUpdaterService;

        public ClearState(
            FSM<CollectMode.State> fsm,
            IArtDataService artDataLoaderService,
            IArtDataService artDataUpdaterService,
            CollectMode.Data modeData,
            CollectArtData artData,
            CollectStageUIPresenter collectStageUIPresenter,

            Func<Tuple<Dot[,], Dot[], MapData>> GetLevelData,
            Action DestroyDots) : base(fsm)
        {
            _artDataLoaderService = artDataLoaderService;
            _artDataUpdaterService = artDataUpdaterService;

            _collectStageUIPresenter = collectStageUIPresenter;
            this.GetLevelData = GetLevelData;
            this.DestroyDots = DestroyDots;
            _modeData = modeData;
            _artData = artData;
        }

        public override void OnClickExitBtn()
        {
            _fsm.SetState(CollectMode.State.Result);
            //ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.HomeScene);
        }

        async Task<List<PlayerArtworkDTO>> GetArtDataFromServer()
        {
            ArtworkManager artworkManager = new ArtworkManager();
            List<PlayerArtworkDTO> artworkDTOs;

            try
            {
                string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;
                artworkDTOs = await artworkManager.GetWholePlayerArtworksAsync(userId);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                Debug.Log("서버로부터 데이터를 받아오지 못함");
                return null;
            }

            return artworkDTOs;
        }

        async Task<Rank?> UpdateArtDataToServer(PlayerArtworkDTO artworkDTO)
        {
            ArtworkManager artworkManager = new ArtworkManager();
            Rank? rank = null;

            try
            {
                rank = await artworkManager.UpdatePlayerArtworkAsync(artworkDTO);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                Debug.Log("서버로 데이터를 업데이트하지 못 함");
                return null;
            }

            return rank;
        }

        async Task<Rank?> UpdateArtDataToServer()
        {
            string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;
            SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();

            Tuple<PlayerArtworkDTO, int, int> artData = await _artDataLoaderService.GetArtData(userId, data.SelectedArtworkKey);
            if (artData == null) return null;

            _artworkDTO = artData.Item1;
            // 데이터 업데이트

            // 다음 스테이지 해금해주는 코드
            _artworkDTO.Stages[data.SelectedArtworkSectionIntIndex].Status = StageStauts.Clear; // 현재 스테이지 클리어 적용

            int lastIndex = _artworkDTO.Stages.Count - 1; // 스테이지 개수 - 1 -> 0부터 시작함
            if(lastIndex >= data.SelectedArtworkSectionIntIndex + 1) // 다음 스테이지 인덱스가 lastIndex 보다 작거나 같은 경우만 진행
            {
                if(_artworkDTO.Stages[data.SelectedArtworkSectionIntIndex + 1].Status == StageStauts.Lock)
                {
                    _artworkDTO.Stages[data.SelectedArtworkSectionIntIndex + 1].Status = StageStauts.Open;
                }
            }

            _artworkDTO.Stages[data.SelectedArtworkSectionIntIndex].IncorrectCnt = _modeData.WrongCount; // index + 1 해서 찾기 -> 1-indexed임
            _artworkDTO.Stages[data.SelectedArtworkSectionIntIndex].HintUsage = _modeData.GoBackCount;

            Rank? rank = await _artDataUpdaterService.UpdateArtData(_artworkDTO);
            if (rank == null) return null;

            return rank;
        }

        PlayerArtworkDTO _artworkDTO;

        public override void OnClickNextStageBtn()
        {
            SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();

            int row = _artData.Sections.Count;
            int col = _artData.Sections[0].Count;

            if (data.SelectedArtworkSectionIndex.x == row - 1
            && data.SelectedArtworkSectionIndex.y == col - 1
            && _artworkDTO.HasIt == false)
            {
                _fsm.SetState(CollectMode.State.Result); 
                // 현재 스테이지가 마지막인 경우 그리고 보유 중이지 않은 경우
                // 이후 플레이 시 랭크가 바뀔 경우 테투리가 변하게끔 적용하자
                return;
            }

            // 0, 0

            Vector2Int changedIndex;
            int nextIndex = data.SelectedArtworkSectionIndex.y + 1;
            if (nextIndex == col) changedIndex = new Vector2Int(data.SelectedArtworkSectionIndex.x + 1, 0);
            else changedIndex = new Vector2Int(data.SelectedArtworkSectionIndex.x, data.SelectedArtworkSectionIndex.y + 1);

            ServiceLocater.ReturnSaveManager().SelectArtworkSection(changedIndex);

            SaveData newData = ServiceLocater.ReturnSaveManager().GetSaveData();
            _fsm.SetState(CollectMode.State.Initialize);
        }

        public override async void OnStateEnter()
        {
            await UpdateArtDataToServer();

            DOVirtual.DelayedCall(0.5f, () =>
            {
                Tuple<Dot[,], Dot[], MapData> levelData = GetLevelData();
                Dot[,] dots = levelData.Item1;
                Vector2Int levelSize = new Vector2Int(dots.GetLength(0), dots.GetLength(1));

                for (int i = 0; i < levelSize.x; i++)
                {
                    for (int j = 0; j < levelSize.y; j++)
                    {
                        // 랜덤하게 줄이기
                        dots[i, j].Minimize(1f);
                    }
                }

                DOVirtual.DelayedCall(1.5f, () =>
                {
                    DestroyDots?.Invoke(); // 모든 닷 제거

                    // 다음 스테이지로 갈 것인지 판단하는 UI 띄우기
                    _collectStageUIPresenter.ActivateGameClearPanel(true);
                    _collectStageUIPresenter.ActivateNextStageBtn(true);
                    _collectStageUIPresenter.ActivateClearExitBtn(true);

                    SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();
                    int row = _artData.Sections.Count;
                    int col = _artData.Sections[0].Count;

                    string completeTitle;
                    string completeContent;

                    if(data.SelectedArtworkSectionIndex.x == row - 1
                    && data.SelectedArtworkSectionIndex.y == col - 1) // 마지막 스테이지의 경우
                    {
                        completeTitle = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.CollectionCompleteTitle);
                        completeContent = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.CollectionCompleteContent);

                        if (_artworkDTO.HasIt == true) // 이미 아트워크를 보유한 경우
                        {
                            // next 버튼 없애주기
                            _collectStageUIPresenter.ActivateNextStageBtn(false);
                            // exit만 가능하게 만들어준다.
                        }
                        else // 보유하지 않은 경우
                        {
                            _collectStageUIPresenter.ActivateClearExitBtn(false);
                            // next만 가능하게 만들어준다.
                        }
                    }
                    else
                    {
                        completeTitle = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.CollectionClearTitle);
                        completeContent = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.CollectionClearContent);
                    }

                    _collectStageUIPresenter.ChangeClearTitleInfo(completeTitle);
                    _collectStageUIPresenter.ChangeClearContentInfo(completeContent);
                });
            });
        }

        const int clearPoint = 100;

        public override void OnStateExit()
        {
            _artworkDTO = null;
            _modeData.MyScore += clearPoint;
            _collectStageUIPresenter.ActivateDetailContent(false);
            _collectStageUIPresenter.ActivateGameClearPanel(false);

            //_collectStageUIPresenter.ChangeNowScore(data.MyScore);
            //_challengeStageUIPresenter.ChangeBestScore(data.MyScore);
        }
    }
}