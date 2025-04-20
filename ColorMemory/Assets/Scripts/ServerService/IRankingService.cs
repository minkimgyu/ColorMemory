using NetworkService.DTO;
using NetworkService.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IRankingService
{
    Task<bool> UpdatePlayerWeeklyScore(int myScore, string userId) { return default; }
    Task<Tuple<List<PersonalRankingData>, PersonalRankingData>> GetTopRankingData(int topCount, string userId) { return default; }
    Task<Tuple<List<PersonalRankingData>, int>> GetNearRankingData(int nearRange, string userId) { return default; }
}

public class MockRankingService : IRankingService
{
    IRankingService _weeklyScoreUpdateService;
    IRankingService _top10RankingService;
    IRankingService _nearRankingService;

    public MockRankingService(IRankingService weeklyScoreUpdateService, IRankingService top10RankingService, IRankingService nearRankingService)
    {
        _weeklyScoreUpdateService = weeklyScoreUpdateService;
        _top10RankingService = top10RankingService;
        _nearRankingService = nearRankingService;
    }

    public async Task<bool> UpdatePlayerWeeklyScore(int myScore, string userId)
    {
        return await _weeklyScoreUpdateService.UpdatePlayerWeeklyScore(myScore, userId);
    }

    public async Task<Tuple<List<PersonalRankingData>, PersonalRankingData>> GetTopRankingData(int topCount, string userId)
    {
        return await _top10RankingService.GetTopRankingData(topCount, userId);
    }

    public async Task<Tuple<List<PersonalRankingData>, int>> GetNearRankingData(int nearRange, string userId)
    {
        return await _nearRankingService.GetNearRankingData(nearRange, userId);
    }
}

public class WeeklyScoreUpdateService : IRankingService
{
    public async Task<bool> UpdatePlayerWeeklyScore(int myScore, string userId)
    {
        ScoreManager scoreManager = new ScoreManager();
        bool canUpdate = false; 

        try
        {
            canUpdate = await scoreManager.UpdatePlayerWeeklyScoreAsync(userId, myScore);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버에 데이터를 전송하지 못함");
            return false;
        }

        return canUpdate;
    }
}

public class Top10RankingService : IRankingService
{
    public async Task<Tuple<List<PersonalRankingData>, PersonalRankingData>> GetTopRankingData(int topCount, string userId) 
    {
        ScoreManager scoreManager = new ScoreManager();
        List<PlayerRankingDTO> otherRankings;
        PlayerRankingDTO myRanking;

        try
        {
            otherRankings = await scoreManager.GetTopWeeklyScoresAsync(topCount);
            myRanking = await scoreManager.GetPlayerWeeklyScoreAsDTOAsync(userId);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버로부터 데이터를 받아오지 못함");
            return null;
        }

        List<PersonalRankingData> topRankingDatas = new List<PersonalRankingData>();

        for (int i = 0; i < otherRankings.Count; i++)
        {
            topRankingDatas.Add(new PersonalRankingData(otherRankings[i].IconId, otherRankings[i].Name, otherRankings[i].Score, otherRankings[i].Ranking));
        }

        PersonalRankingData myRankingData = new PersonalRankingData(myRanking.IconId, myRanking.Name, myRanking.Score, myRanking.Ranking);

        return new Tuple<List<PersonalRankingData>, PersonalRankingData>(topRankingDatas, myRankingData);
    }
}

public class NearRankingService : IRankingService
{
    public async Task<Tuple<List<PersonalRankingData>, int>> GetNearRankingData(int nearRange, string userId)
    {
        ScoreManager scoreManager = new ScoreManager();
        List<PlayerRankingDTO> playerScoreDTOs = new List<PlayerRankingDTO>();

        try
        {
            playerScoreDTOs = await scoreManager.GetSurroundingWeeklyRankingByIdAsync(userId, nearRange);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버로 데이터를 전송하지 못 함");
            return null;
        }

        int myRankingIndex = -1;
        List<PersonalRankingData> rankingDatas = new List<PersonalRankingData>();
        for (int i = 0; i < playerScoreDTOs.Count; i++)
        {
            if (playerScoreDTOs[i].PlayerId == userId) myRankingIndex = i;
            rankingDatas.Add(new PersonalRankingData(playerScoreDTOs[i].IconId, playerScoreDTOs[i].Name, playerScoreDTOs[i].Score, i + 1));
        }

        return new Tuple<List<PersonalRankingData>, int>(rankingDatas, myRankingIndex);
    }
}