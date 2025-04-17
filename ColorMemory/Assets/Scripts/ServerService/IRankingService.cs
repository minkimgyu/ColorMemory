using NetworkService.DTO;
using NetworkService.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IRankingService
{
    Task<Tuple<List<PlayerRankingDTO>, PlayerRankingDTO>> GetTop10RankingData() { return default; }
    Task<List<PlayerRankingDTO>> GetNearRankingData(int nearRange) { return default; }
}

public class MockRankingService : IRankingService
{
    public async Task<Tuple<List<PlayerRankingDTO>, PlayerRankingDTO>> GetTop10RankingData()
    {
        ScoreManager scoreManager = new ScoreManager();
        List<PlayerRankingDTO> otherRankings;
        PlayerRankingDTO myRanking;

        string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;

        otherRankings = await scoreManager.GetTopWeeklyScoresAsync(10);
        myRanking = await scoreManager.GetPlayerWeeklyScoreAsDTOAsync(userId);

        return new Tuple<List<PlayerRankingDTO>, PlayerRankingDTO>(otherRankings, myRanking);
    }

    public async Task<List<PlayerRankingDTO>> GetNearRankingData(int nearRange)
    {
        ScoreManager scoreManager = new ScoreManager();
        List<PlayerRankingDTO> playerScoreDTOs = new List<PlayerRankingDTO>();

        string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;
        playerScoreDTOs = await scoreManager.GetSurroundingWeeklyRankingByIdAsync(userId, nearRange);

        return playerScoreDTOs;
    }
}

public class Top10RankingService : IRankingService
{
    public async Task<Tuple<List<PlayerRankingDTO>, PlayerRankingDTO>> GetTop10RankingData() 
    {
        ScoreManager scoreManager = new ScoreManager();
        List<PlayerRankingDTO> otherRankings;
        PlayerRankingDTO myRanking;

        try
        {
            string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;

            otherRankings = await scoreManager.GetTopWeeklyScoresAsync(10);
            myRanking = await scoreManager.GetPlayerWeeklyScoreAsDTOAsync(userId);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버로부터 데이터를 받아오지 못함");
            return null;
        }

        return new Tuple<List<PlayerRankingDTO>, PlayerRankingDTO>(otherRankings, myRanking);
    }
}

public class NearRankingService : IRankingService
{
    public async Task<List<PlayerRankingDTO>> GetNearRankingData(int nearRange)
    {
        ScoreManager scoreManager = new ScoreManager();
        List<PlayerRankingDTO> playerScoreDTOs = new List<PlayerRankingDTO>();

        try
        {
            string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;
            playerScoreDTOs = await scoreManager.GetSurroundingWeeklyRankingByIdAsync(userId, nearRange);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버로 데이터를 전송하지 못 함");
            return null;
        }

        return playerScoreDTOs;
    }
}