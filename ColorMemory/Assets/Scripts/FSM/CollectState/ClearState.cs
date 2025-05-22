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

            // ���� �������� �ر����ִ� �ڵ�
            artData.Item1.Stages[data.SelectedArtworkSectionIntIndex].Status = StageStauts.Clear; // ���� �������� Ŭ���� ����

            int lastIndex = artData.Item1.Stages.Count - 1; // �������� ���� - 1 -> 0���� ������
            if(lastIndex >= data.SelectedArtworkSectionIntIndex + 1) // ���� �������� �ε����� lastIndex ���� �۰ų� ���� ��츸 ����
            {
                if(artData.Item1.Stages[data.SelectedArtworkSectionIntIndex + 1].Status == StageStauts.Lock)
                {
                    artData.Item1.Stages[data.SelectedArtworkSectionIntIndex + 1].Status = StageStauts.Open;
                }
            }

            artData.Item1.Stages[data.SelectedArtworkSectionIntIndex].IncorrectCnt = _modeData.WrongCount; // index + 1 �ؼ� ã�� -> 1-indexed��
            artData.Item1.Stages[data.SelectedArtworkSectionIntIndex].HintUsage = _modeData.GoBackCount;

            bool clearAllStage = true;
            foreach (var item in artData.Item1.Stages)
            {
                // �ϳ��� ���������� Ŭ���� ���� ���� ���
                if (item.Value.Status != StageStauts.Clear) clearAllStage = false;
            }

            if(clearAllStage == true) artData.Item1.HasIt = true;  // HasIt ������Ʈ

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
                // ���� ���������� �������� ��� �׸��� ���� ������ ���� ���
                // ���� �÷��� �� ��ũ�� �ٲ� ��� �������� ���ϰԲ� ��������
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
            Rank? currentRank = await UpdateArtDataToServer();
            if (currentRank == null) return;

            DOVirtual.DelayedCall(0.5f, () =>
            {
                Tuple<Dot[,], Dot[], MapData> levelData = GetLevelData();
                Dot[,] dots = levelData.Item1;
                Vector2Int levelSize = new Vector2Int(dots.GetLength(0), dots.GetLength(1));

                for (int i = 0; i < levelSize.x; i++)
                {
                    for (int j = 0; j < levelSize.y; j++)
                    {
                        // �����ϰ� ���̱�
                        dots[i, j].Minimize(1f);
                    }
                }

                DOVirtual.DelayedCall(1.5f, () =>
                {
                    DestroyDots?.Invoke(); // ��� �� ����

                    // ���� ���������� �� ������ �Ǵ��ϴ� UI ����
                    _collectStageUIPresenter.ActivateGameClearPanel(true);
                    _collectStageUIPresenter.ActivateNextStageBtn(true);
                    _collectStageUIPresenter.ActivateClearExitBtn(true);

                    string completeTitle;
                    string completeContent;

                    SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();

                    int row = _artData.Sections.Count;
                    int col = _artData.Sections[0].Count;

                    if (data.SelectedArtworkSectionIndex.x == row - 1
                        && data.SelectedArtworkSectionIndex.y == col - 1) // ������ ���������� ���
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

                        //if (_alreadyHaveArtwork == true) // �̹� ��Ʈ��ũ�� ������ ���
                        //{
                        //    // next ��ư �����ֱ�
                        //    _collectStageUIPresenter.ActivateNextStageBtn(false);
                        //    // exit�� �����ϰ� ������ش�.
                        //}
                        //else // �������� ���� ���
                        //{
                        //    _collectStageUIPresenter.ActivateClearExitBtn(false);
                        //    // next�� �����ϰ� ������ش�.
                        //}
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
                });
            });
        }

        const int clearPoint = 100;

        public override void OnStateExit()
        {
            _modeData.MyScore += clearPoint;
            _collectStageUIPresenter.ActivateDetailContent(false);
            _collectStageUIPresenter.ActivateGameClearPanel(false);

            //_collectStageUIPresenter.ChangeNowScore(data.MyScore);
            //_challengeStageUIPresenter.ChangeBestScore(data.MyScore);
        }
    }
}