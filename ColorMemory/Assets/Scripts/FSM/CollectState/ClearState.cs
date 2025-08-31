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
        Animator _completeAnimator;

        IArtDataService _artDataLoaderService;
        IArtDataService _artDataUpdaterService;

        Dictionary<int, Sprite> _artSpriteAsserts;
        Dictionary<NetworkService.DTO.Rank, Sprite> _artworkFrameAssets;
        Dictionary<NetworkService.DTO.Rank, Sprite> _rankDecorationIconAssets;

        public ClearState(
            FSM<CollectMode.State> fsm,
            IArtDataService artDataLoaderService,
            IArtDataService artDataUpdaterService,
            CollectMode.Data modeData,
            CollectArtData artData,
            CollectStageUIPresenter collectStageUIPresenter,
            Animator completeAnimator,

            Dictionary<int, Sprite> artSpriteAsserts,
            Dictionary<NetworkService.DTO.Rank, Sprite> artworkFrameAssets,
            Dictionary<NetworkService.DTO.Rank, Sprite> rankDecorationIconAssets,

            Func<Tuple<Dot[,], Dot[], MapData>> GetLevelData,
            Action DestroyDots) : base(fsm)
        {
            _stateTimer = new Timer();
            _artDataLoaderService = artDataLoaderService;
            _artDataUpdaterService = artDataUpdaterService;

            _collectStageUIPresenter = collectStageUIPresenter;

            _collectStageUIPresenter.OnClickPauseGameExitBtn += () => { _fsm.SetState(CollectMode.State.Result); };
            _collectStageUIPresenter.OnClickClearExitBtn += () => { _fsm.SetState(CollectMode.State.Result); };
            _collectStageUIPresenter.OnClickNextStageBtn += OnClickNextStageBtn;

            _completeAnimator = completeAnimator;

            _artSpriteAsserts = artSpriteAsserts;
            _artworkFrameAssets = artworkFrameAssets;
            _rankDecorationIconAssets = rankDecorationIconAssets;

            this.GetLevelData = GetLevelData;
            this.DestroyDots = DestroyDots;
            _modeData = modeData;
            _artData = artData;
        }

        async Task<Rank?> UpdateArtDataToServer()
        {
            string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;
            SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();

            Tuple<PlayerArtworkDTO, int, int> artData = await _artDataLoaderService.GetArtData(userId, data.SelectedArtworkKey);
            if (artData == null) return null;

            // 다음 스테이지 해금해주는 코드
            artData.Item1.Stages[data.SelectedArtworkSectionIntIndex].Status = StageStauts.Clear; // 현재 스테이지 클리어 적용

            int lastIndex = artData.Item1.Stages.Count - 1; // 스테이지 개수 - 1 -> 0부터 시작함
            if(lastIndex >= data.SelectedArtworkSectionIntIndex + 1) // 다음 스테이지 인덱스가 lastIndex 보다 작거나 같은 경우만 진행
            {
                if(artData.Item1.Stages[data.SelectedArtworkSectionIntIndex + 1].Status == StageStauts.Lock)
                {
                    artData.Item1.Stages[data.SelectedArtworkSectionIntIndex + 1].Status = StageStauts.Open;
                }
            }

            artData.Item1.Stages[data.SelectedArtworkSectionIntIndex].IncorrectCnt = _modeData.WrongCount; // index + 1 해서 찾기 -> 1-indexed임
            artData.Item1.Stages[data.SelectedArtworkSectionIntIndex].HintUsage = _modeData.GoBackCount;

            bool clearAllStage = true;
            foreach (var item in artData.Item1.Stages)
            {
                // 하나의 스테이지라도 클리어 하지 못한 경우
                if (item.Value.Status != StageStauts.Clear) clearAllStage = false;
            }

            if(clearAllStage == true) artData.Item1.HasIt = true;  // HasIt 업데이트

            Rank? rank = await _artDataUpdaterService.UpdateArtData(artData.Item1);
            if (rank == null) return null;

            return rank;
        }

        //PlayerArtworkDTO _artworkDTO;

        void OnClickNextStageBtn()
        {
            SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();

            int row = _artData.Sections.Count;
            int col = _artData.Sections[0].Count;

            if (data.SelectedArtworkSectionIndex.x == row - 1
            && data.SelectedArtworkSectionIndex.y == col - 1)
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

        public enum State
        {
            DelayAfterClear,
            Minimizing,
            CompleteStage,
            Finish,
        }

        State _currentState;
        Timer _stateTimer;
        const float _stateChangeDelay = 1.5f;
        Rank? currentRank;

        public override async void OnStateEnter()
        {
            currentRank = await UpdateArtDataToServer();
            if (currentRank == null) return;

            _currentState = State.DelayAfterClear;
            _stateTimer.Reset();
        }

        void MinimizeAllDots()
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
        }

        void CompleteStage()
        {
            DestroyDots?.Invoke(); // 모든 닷 제거

            // 다음 스테이지로 갈 것인지 판단하는 UI 띄우기
            _collectStageUIPresenter.ActivateGameClearPanel(true);
            _collectStageUIPresenter.ActivateNextStageBtn(true);
            _collectStageUIPresenter.ActivateClearExitBtn(true);

            string completeTitle;
            string completeContent;

            SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();

            int row = _artData.Sections.Count;
            int col = _artData.Sections[0].Count;

            if (data.SelectedArtworkSectionIndex.x == row - 1
                && data.SelectedArtworkSectionIndex.y == col - 1) // 마지막 스테이지의 경우
            {
                ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.GameClear);

                Sprite artworkSprite;
                Sprite rankFrameSprite;
                Sprite rankDecorationIconSprite;

                artworkSprite = _artSpriteAsserts[data.SelectedArtworkKey];
                rankFrameSprite = _artworkFrameAssets[currentRank.Value];
                rankDecorationIconSprite = _rankDecorationIconAssets[currentRank.Value];

                _collectStageUIPresenter.ChangeArtworkPreview(artworkSprite, rankFrameSprite, rankDecorationIconSprite);

                _completeAnimator.SetTrigger("CompleteArtwork");

                completeTitle = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.CollectionCompleteTitle);
                completeContent = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.CollectionCompleteContent);

                _collectStageUIPresenter.ActivateClearExitBtn(false);
            }
            else
            {
                ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.StageClear);

                _completeAnimator.SetTrigger("CompleteSection");

                completeTitle = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.CollectionClearTitle);
                completeContent = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.CollectionClearContent);
            }

            _collectStageUIPresenter.ChangeClearTitleInfo(completeTitle);
            _collectStageUIPresenter.ChangeClearContentInfo(completeContent);
        }

        public override void OnStateUpdate()
        {
            if (_stateTimer.CurrentState == Timer.State.Running) return;

            // 타이머가 완료되었다면 현재 상태에서 다음 상태로 넘어가기
            if (_stateTimer.CurrentState == Timer.State.Finish)
            {
                switch (_currentState)
                {
                    case State.DelayAfterClear:
                        _currentState = State.Minimizing;
                        break;
                    case State.Minimizing:
                        _currentState = State.CompleteStage;
                        break;
                    case State.CompleteStage:
                        _currentState = State.Finish;
                        break;
                }
            }

            // 다음 상태로 넘어가기
            switch (_currentState)
            {
                case State.DelayAfterClear:
                    _stateTimer.Reset();
                    _stateTimer.Start(0.5f);
                    break;
                case State.Minimizing:
                    MinimizeAllDots();

                    _stateTimer.Reset();
                    _stateTimer.Start(_stateChangeDelay);
                    break;
                case State.CompleteStage:
                    // 다음 스테이지로 넘어가기
                    CompleteStage();
                    break;
                case State.Finish:
                    break;
            }
        }

        const int clearPoint = 100;

        public override void OnStateExit()
        {
            _modeData.MyScore += clearPoint;
            _collectStageUIPresenter.ActivateDetailContent(false);
            _collectStageUIPresenter.ActivateGameClearPanel(false);
        }
    }
}