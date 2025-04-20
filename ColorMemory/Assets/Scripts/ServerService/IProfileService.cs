using NetworkService.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IProfileService
{
    Task<int> GetPlayerIconId(string playerId) { return default; }
    Task<bool> SetPlayerIconId(string playerId, string playerName, int profileIndex) { return default; }
}

public class MockProfileService : IProfileService
{
    IProfileService _playerIconService;

    public MockProfileService(IProfileService playerIconService)
    {
        _playerIconService = playerIconService;
    }

    public async Task<int> GetPlayerIconId(string playerId) 
    { 
        return await _playerIconService.GetPlayerIconId(playerId);
    }

    public async Task<bool> SetPlayerIconId(string playerId, string playerName, int profileIndex) 
    {
        return await _playerIconService.SetPlayerIconId(playerId, playerName, profileIndex);
    }
}

public class ProfileService : IProfileService
{
    public async Task<int> GetPlayerIconId(string playerId) 
    {
        PlayerManager playerManager = new PlayerManager();
        int iconIndex = -1;

        try
        {
            iconIndex = await playerManager.GetPlayerIconIdAsync(playerId);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버로 데이터를 전달할 수 없음");
            return -1;
        }

        return iconIndex;
    }

    public async Task<bool> SetPlayerIconId(string playerId, string playerName, int profileIndex) 
    {
        PlayerManager playerManager = new PlayerManager();
        bool isSuccess = false;

        try
        {
            isSuccess = await playerManager.SetPlayerIconIdAsync(playerId, playerName, profileIndex);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버로 데이터를 전달할 수 없음");
            return false;
        }

        return isSuccess;
    }
}