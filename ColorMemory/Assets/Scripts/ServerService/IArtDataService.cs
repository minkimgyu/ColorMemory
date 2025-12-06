using NetworkService.DTO;
using NetworkService.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.SceneManagement;
using UnityEngine;

public interface IArtDataService
{
    Task<Dictionary<int, ArtData>> GetArtData(string userId) { return default; } // 전부 가져오기
    Task<Tuple<PlayerArtworkDTO, int, int>> GetArtData(string userId, int artworkKey) { return default; } // 하나만 가져오기
    Task<Rank?> UpdateArtData(int artworkKey, PlayerArtworkDTO dTO) { return default; }

   // 전체 아트워크 가져오기
   // 아트워크 업데이트 하기
}

public class MockArtDataService : IArtDataService
{
    IArtDataService _artDataLoaderService;
    IArtDataService _artDataUpdaterService;

    public MockArtDataService(IArtDataService artDataLoaderService, IArtDataService artDataUpdaterService)
    {
        _artDataLoaderService = artDataLoaderService;
        _artDataUpdaterService = artDataUpdaterService;
    }

    public async Task<Dictionary<int, ArtData>> GetArtData(string userId) 
    { 
        return await _artDataLoaderService.GetArtData(userId);
    }

    public async Task<Tuple<PlayerArtworkDTO, int, int>> GetArtData(string userId, int artworkKey) 
    {
        return await _artDataLoaderService.GetArtData(userId, artworkKey);
    }

    public async Task<Rank?> UpdateArtData(int artworkKey, PlayerArtworkDTO dTO)
    { 
        return await _artDataUpdaterService.UpdateArtData(artworkKey, dTO);
    }
}


public class ArtDataLoaderService : IArtDataService
{
    public async Task<Tuple<PlayerArtworkDTO, int, int>> GetArtData(string userId, int artworkKey)
    {
        ArtworkManager artworkManager = new ArtworkManager();
        List<PlayerArtworkDTO> ownedArtworkDTOs, unownedArtworkDTOs;

        try
        {
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

        PlayerArtworkDTO artworkDTO = ownedArtworkDTOs.Find(x => x.ArtworkId == artworkKey);
        return new Tuple<PlayerArtworkDTO, int, int>(artworkDTO, ownCount, unownedCount);
    }

    public async Task<Dictionary<int, ArtData>> GetArtData(string userId)
    {
        ArtworkManager artworkManager = new ArtworkManager();
        List<PlayerArtworkDTO> artworkDTOs;

        try
        {
            artworkDTOs = await artworkManager.GetWholePlayerArtworksAsync(userId);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버로부터 데이터를 받아오지 못함");
            return null;
        }

        artworkDTOs.Sort((a, b) => a.ArtworkId.CompareTo(b.ArtworkId));

        Dictionary<int, ArtData> artDatas = new Dictionary<int, ArtData>();

        for (int i = 0; i < artworkDTOs.Count; i++)
        {
            Dictionary<int, StageData> stageDatas = new Dictionary<int, StageData>();

            foreach (var dto in artworkDTOs[i].Stages)
            {
                StageData stageData = new StageData(dto.Value.Rank, dto.Value.HintUsage, dto.Value.IncorrectCnt, dto.Value.Status);
                stageDatas.Add(dto.Key, stageData);
            }

            ArtData artData = new ArtData(
                rank: artworkDTOs[i].Rank,
                hasIt: artworkDTOs[i].HasIt,
                stageDatas: stageDatas,
                totalMistakes: artworkDTOs[i].TotalMistakes,
                totalHints: artworkDTOs[i].TotalHints,
                obtainedDate: artworkDTOs[i].ObtainedDate);

            artDatas.Add(artworkDTOs[i].ArtworkId, artData);
        }

        return artDatas;
    }
}

public class LocalArtDataLoaderService : IArtDataService
{
    public async Task<Tuple<PlayerArtworkDTO, int, int>> GetArtData(string userId, int artworkKey)
    {
        PlayerArtworkDTO artworkDTO = null;
        int unownedCount = 0;
        int ownCount = 0;

        try
        {
            await Task.Delay(10); // 모의 비동기 대기

            Dictionary<int, ArtData> artDatas = ServiceLocater.ReturnSaveManager().GetSaveData().ArtDatas;
            ArtData artDataToFind = artDatas[artworkKey];

            int totalHints = 0;
            int totalMistakes = 0;

            Dictionary<int, StageDTO> stageDTO = new Dictionary<int, StageDTO>();
            foreach (var stageData in artDataToFind.StageDatas)
            {
                totalMistakes += stageData.Value.IncorrectCnt;
                totalHints += stageData.Value.HintUsage;

                StageDTO dto = new StageDTO
                {
                    Rank = stageData.Value.Rank,
                    HintUsage = stageData.Value.HintUsage,
                    IncorrectCnt = stageData.Value.IncorrectCnt,
                    Status = stageData.Value.Stauts
                };
                stageDTO.Add(stageData.Key, dto);
            }

            artworkDTO = new PlayerArtworkDTO
            {
                PlayerId = userId,
                ArtworkId = artworkKey,
                Title = "", // 로컬에서는 제목 정보 없음
                Artist = "", // 로컬에서는 설명 정보 없음
                TotalHints = totalHints, // 안씀
                TotalMistakes = totalMistakes, // 안씀
                Stages = stageDTO,
                Rank = artDataToFind.Rank,
                HasIt = artDataToFind.HasIt,
                ObtainedDate = artDataToFind.ObtainedDate,
            };

            foreach (var data in artDatas)
            {
                if (data.Value.HasIt)
                {
                    ownCount++;
                }
                else
                {
                    unownedCount++;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버로부터 데이터를 받아오지 못함");
            return null;
        }

        return new Tuple<PlayerArtworkDTO, int, int>(artworkDTO, ownCount, unownedCount);
    }

    public async Task<Dictionary<int, ArtData>> GetArtData(string userId)
    {
        Dictionary<int, ArtData> artDatas = new Dictionary<int, ArtData>();

        try
        {
            await Task.Delay(10); // 모의 비동기 대기
            artDatas = ServiceLocater.ReturnSaveManager().GetSaveData().ArtDatas;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버로부터 데이터를 받아오지 못함");
            return null;
        }

        return artDatas;
    }
}


public class ArtDataUpdaterService : IArtDataService
{
    public async Task<Rank?> UpdateArtData(int artworkKey, PlayerArtworkDTO dTO)
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
}

public class LocalArtDataUpdaterService : IArtDataService
{

    Rank GetRank(int totalMistakesAndHints)
    {
        Rank rank = Rank.NONE;

        if (totalMistakesAndHints < 5)
        {
            rank = Rank.GOLD;
        }
        else if (totalMistakesAndHints < 15)
        {
            rank = Rank.SILVER;
        }
        else
        {
            rank = Rank.COPPER;
        }

        return rank;
    }

    public async Task<Rank?> UpdateArtData(int artworkKey, PlayerArtworkDTO dTO)
    {
        Rank totalRank = Rank.NONE;

        try
        {
            await Task.Delay(10); // 모의 비동기 대기

            Dictionary<int, StageData> stageDatas = new Dictionary<int, StageData>();
            int totalMistakes = 0;
            int totalHints = 0;

            bool nowClear = true;

            foreach (var stage in dTO.Stages)
            {
                totalMistakes += stage.Value.IncorrectCnt;
                totalHints += stage.Value.HintUsage;

                int currentTotalMistakesAndHints = stage.Value.HintUsage + stage.Value.IncorrectCnt;

                Rank curStageRank = Rank.NONE;

                // 해금된 스테이지만 변경
                if (stage.Value.Status != StageStauts.Lock)
                {
                    curStageRank = GetRank(currentTotalMistakesAndHints);
                }

                StageData stageData = new StageData(
                   curStageRank,
                   stage.Value.HintUsage,
                   stage.Value.IncorrectCnt,
                   stage.Value.Status
                );
                stageDatas.Add(stage.Key, stageData);

                // 하나라도 잠긴 스테이지가 있으면 클리어 못한 것
                if (stage.Value.Status == StageStauts.Lock)
                {
                    nowClear = false;
                }
            }

            // 클리어한 경우에만 랭크 갱신
            if (nowClear == true)
            {
                totalRank = GetRank(totalMistakes + totalHints);
            }

            Dictionary<int, ArtData> artDatas = ServiceLocater.ReturnSaveManager().GetSaveData().ArtDatas;

            // 획득 날짜가 비어있으면 현재 시간으로 설정
            if (dTO.ObtainedDate == null)
            {
                dTO.ObtainedDate = DateTime.Now;
            }

            ArtData artData = new ArtData(
                rank : totalRank,
                hasIt: nowClear,
                stageDatas: stageDatas,
                totalHints: totalHints,
                totalMistakes: totalMistakes,
                obtainedDate: dTO.ObtainedDate
            );

            artDatas[artworkKey] = artData;
            ServiceLocater.ReturnSaveManager().ChangeArtDatas(artDatas);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버로 데이터를 전송하지 못함");
            return null;
        }

        return totalRank;
    }
}