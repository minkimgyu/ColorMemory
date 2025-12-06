using NetworkService.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IAccountService
{
    Task<bool> Login(string userId, string userName) { return default; }
    Task<bool> DeleteAccount(string userId) { return default; }
}

public class AccountService : IAccountService
{
    public async Task<bool> Login(string userId, string userName)
    {
        PlayerManager playerManager = new PlayerManager();
        bool canLogin = false;

        try
        {
            canLogin = await playerManager.AddPlayerAsync(userId, userName);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버에 데이터를 보낼 수 없음");
            return false;
        }

        return canLogin;
    }

    public async Task<bool> DeleteAccount(string userId) 
    {
        PlayerManager playerManager = new PlayerManager();
        bool canDelete = false;

        try
        {
            canDelete = await playerManager.DeletePlayerAync(userId);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버에 데이터를 보낼 수 없음");
            return false;
        }

        return canDelete;
    }
}

public class LocalAccountService : IAccountService
{
    public async Task<bool> Login(string userId, string userName)
    {
        bool canLogin = false;

        try
        {
            await Task.Delay(10); // 더미 비동기 대기
            canLogin = true;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버에 데이터를 보낼 수 없음");
            return false;
        }

        return canLogin;
    }

    public async Task<bool> DeleteAccount(string userId)
    {
        bool canDelete = false;

        try
        {
            await Task.Delay(10); // 더미 비동기 대기
            canDelete = true;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버에 데이터를 보낼 수 없음");
            return false;
        }

        return canDelete;
    }
}

public class MockAccountService : IAccountService
{
    IAccountService _loginService;

    public MockAccountService(IAccountService loginService)
    {
        _loginService = loginService;
    }

    public async Task<bool> Login(string userId, string userName)
    {
        return await _loginService.Login(userId, userName);
    }

    public async Task<bool> DeleteAccount(string userId) 
    {
        return await _loginService.DeleteAccount(userId);
    }
}