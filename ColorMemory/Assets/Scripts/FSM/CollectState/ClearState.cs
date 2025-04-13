using UnityEngine;
using System;
using DG.Tweening;

namespace Collect
{
    public class ClearState : BaseState<CollectMode.State>
    {
        Action DestroyDots;
        CollectMode.Data _modeData;
        CollectiveArtData _artData;

        Func<Tuple<Dot[,], Dot[], MapData>> GetLevelData;
        CollectStageUIPresenter _collectStageUIPresenter;

        public ClearState(
            FSM<CollectMode.State> fsm,
            CollectMode.Data modeData,
            CollectiveArtData artData,
            CollectStageUIPresenter collectStageUIPresenter,

            Func<Tuple<Dot[,], Dot[], MapData>> GetLevelData,
            Action DestroyDots) : base(fsm)
        {
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
                Debug.Log("�����κ��� �����͸� �޾ƿ��� ����");
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
                Debug.Log("������ �����͸� ������Ʈ���� �� ��");
                return null;
            }

            return rank;
        }

        async Task<Rank?> UpdateArtDataToServer()
        {
            List<PlayerArtworkDTO> artDatas = await GetArtDataFromServer();
            if (artDatas == null) return null;

            SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();
            _artworkDTO = artDatas.Find(x => x.ArtworkId == data.SelectedArtworkKey);

            // ������ ������Ʈ

            // ���� �������� �ر����ִ� �ڵ�
            _artworkDTO.Stages[data.SelectedArtworkSectionIntIndex].Status = StageStauts.Clear; // ���� �������� Ŭ���� ����

            int lastIndex = _artworkDTO.Stages.Count - 1; // �������� ���� - 1 -> 0���� ������
            if(lastIndex >= data.SelectedArtworkSectionIntIndex + 1) // ���� �������� �ε����� lastIndex ���� �۰ų� ���� ��츸 ����
            {
                if(_artworkDTO.Stages[data.SelectedArtworkSectionIntIndex + 1].Status == StageStauts.Lock)
                {
                    _artworkDTO.Stages[data.SelectedArtworkSectionIntIndex + 1].Status = StageStauts.Open;
                }
            }

            _artworkDTO.Stages[data.SelectedArtworkSectionIntIndex].IncorrectCnt = _modeData.WrongCount; // index + 1 �ؼ� ã�� -> 1-indexed��
            _artworkDTO.Stages[data.SelectedArtworkSectionIntIndex].HintUsage = _modeData.GoBackCount;

            Rank? rank = await UpdateArtDataToServer(_artworkDTO);
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
            && data.SelectedArtworkSectionIndex.y == col - 1)
            {
                _fsm.SetState(CollectMode.State.Result); // ���� ���������� �������� ���
                return;
            }

            Vector2Int changedIndex;
            int nextIndex = data.SelectedArtworkSectionIndex.y + 1;
            if (nextIndex >= col) changedIndex = new Vector2Int(data.SelectedArtworkSectionIndex.x + 1, 0);
            else changedIndex = new Vector2Int(data.SelectedArtworkSectionIndex.x, data.SelectedArtworkSectionIndex.y + 1);
            ServiceLocater.ReturnSaveManager().SelectArtwork(changedIndex);

            _fsm.SetState(CollectMode.State.Initialize, changedIndex);
        }

        public override void OnStateEnter()
        {
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Tuple<Dot[,], Dot[], MapData> levelData = GetLevelData();
                Dot[,] dots = levelData.Item1;
                Vector2Int levelSize = new Vector2Int(dots.GetLength(0), dots.GetLength(1));

                for (int i = 0; i < levelSize.x; i++)
                {
                    for (int j = 0; j < levelSize.y; j++)
                    {
                        // �����ϰ� Ű���
                        dots[i, j].Minimize(1f);
                    }
                }

                DOVirtual.DelayedCall(1.5f, () =>
                {
                    DestroyDots?.Invoke();

                    // ���� ���������� �� ������ �Ǵ��ϴ� UI ����
                    _collectStageUIPresenter.ActivateGameClearPanel(true);

                    //_fsm.SetState(CollectMode.State.Initialize);
                });
            });
        }

        const int clearPoint = 100;

        public override void OnStateExit()
        {
            _modeData.MyScore += clearPoint;
            _collectStageUIPresenter.ActivateGameClearPanel(false);
            //_collectStageUIPresenter.ChangeNowScore(data.MyScore);
            //_challengeStageUIPresenter.ChangeBestScore(data.MyScore);
        }
    }
}