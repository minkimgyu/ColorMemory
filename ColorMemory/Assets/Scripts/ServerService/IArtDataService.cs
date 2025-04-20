using NetworkService.DTO;
using NetworkService.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IArtDataService
{
    Task<Dictionary<int, ArtData>> GetArtData(string userId) { return default; } // 전부 가져오기
    Task<Tuple<PlayerArtworkDTO, int, int>> GetArtData(string userId, int artworkKey) { return default; } // 하나만 가져오기
    Task<Rank?> UpdateArtData(PlayerArtworkDTO dTO) { return default; }

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

    public async Task<Rank?> UpdateArtData(PlayerArtworkDTO dTO)
    { 
        return await _artDataUpdaterService.UpdateArtData(dTO);
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
                artworkDTOs[i].Rank,
                artworkDTOs[i].HasIt,
                stageDatas,
                artworkDTOs[i].TotalMistakesAndHints,
                artworkDTOs[i].ObtainedDate);

            artDatas.Add(artworkDTOs[i].ArtworkId, artData);
        }

        return artDatas;
    }
}

public class ArtDataUpdaterService : IArtDataService
{
    public async Task<Rank?> UpdateArtData(PlayerArtworkDTO dTO)
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