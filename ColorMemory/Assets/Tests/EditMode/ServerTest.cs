using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ServerTest
{
    private ILoginService _loginService;
    private IRankingService _rankingService;

    [SetUp]
    public void Setup()
    {
        _loginService = new MockLoginService();
        _rankingService = new MockRankingService();
    }

    // A Test behaves as an ordinary method
    [Test]
    public async void LoginTest()
    {
        // Arrange
        string userId = "testUser123";
        string userName = "Test User";

        // Act
        bool result = await _loginService.Login(userId, userName);

        // Assert
        Assert.IsTrue(result);
    }

    // A Test behaves as an ordinary method
    [Test]
    public async void RankingTest()
    {

        // Assert
        //Assert.IsTrue(result);

        // 검사 추가 필요함
    }
}
