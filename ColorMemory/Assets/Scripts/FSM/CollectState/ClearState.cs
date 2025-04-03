using UnityEngine;
using System;
using DG.Tweening;
using NetworkService.DTO;
using NetworkService.Manager;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using static Challenge.ChallengeMode;

namespace Collect
{
    public class ClearState : BaseState<CollectMode.State>
    {
        Action DestroyDots;
        CollectMode.Data _modeData;
        CollectArtData _artData;

        Func<Tuple<Dot[,], Dot[], MapData>> GetLevelData;
        CollectStageUIPresenter _collectStageUIPresenter;

        public ClearState(
            FSM<CollectMode.State> fsm,
            CollectMode.Data modeData,
            CollectArtData artData,
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
            ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.HomeScene);
        }

        async Task<List<PlayerArtworkDTO>> GetArtDataFromServer()
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
                Debug.Log("�����κ��� �����͸� �޾ƿ��� ����");
                return null;
            }

            ownedArtworkDTOs.AddRange(unownedArtworkDTOs); // list1�� list2 ��� �߰�
            return ownedArtworkDTOs;
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


        public override async void OnClickNextStageBtn()
        {
            List<PlayerArtworkDTO> artDatas = await GetArtDataFromServer();
            if (artDatas == null) return;

            SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();
            PlayerArtworkDTO artworkDTO = artDatas.Find(x => x.ArtworkId == data.SelectedArtworkKey);

            // ������ ������Ʈ
            artworkDTO.Stages[data.SelectedArtworkSectionIntIndex + 1].IncorrectCnt = _modeData.WrongCount;
            artworkDTO.Stages[data.SelectedArtworkSectionIntIndex + 1].HintUsage = _modeData.GoBackCount;

            Rank? rank = await UpdateArtDataToServer(artworkDTO);
            if (rank == null) return;

            Debug.Log(rank.ToString());


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
            if (nextIndex >= col) changedIndex = new Vector2Int(data.SelectedArtworkSectionIndex.x + 1, data.SelectedArtworkSectionIndex.y);
            else changedIndex = new Vector2Int(data.SelectedArtworkSectionIndex.x, data.SelectedArtworkSectionIndex.y + 1);

            ServiceLocater.ReturnSaveManager().SelectArtworkSection(changedIndex);



            SaveData newData = ServiceLocater.ReturnSaveManager().GetSaveData();
            _fsm.SetState(CollectMode.State.Initialize);
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
                        // �����ϰ� ���̱�
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