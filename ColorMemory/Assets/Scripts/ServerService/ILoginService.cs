using NetworkService.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface ILoginService
{
    Task<bool> Login(string userId, string userName);
}

public class LoginService : ILoginService
{
    public async Task<bool> Login(string userId, string userName)
    {
        PlayerManager playerManager = new PlayerManager();
        bool canLogin = false;

        try
        {
            Debug.Log("_userId          " + userId);
            Debug.Log("_userName            " + userName);
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
}

public class MockLoginService : ILoginService
{
    ILoginService _loginService;

    public MockLoginService(ILoginService loginService)
    {
        _loginService = loginService;
    }

    public async Task<bool> Login(string userId, string userName)
    {
        return await _loginService.Login(userId, userName);
    }
}