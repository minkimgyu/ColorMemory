using NetworkService.DTO;
using NetworkService.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public interface IAssetService
{
    Task<Challenge.ChallengeMode.ModeData> GetChallengeModeData(string playerId, 
        float leftDuration, 
        float decreaseDurationOnMiss,
        float increaseDurationOnClear) 
    {
        return default;
    }

    Task<bool> ProcessTransaction(string playerId, int usedMoney, int earnMoney) { return default; }
    Task<int> GetCurrency(string playerId) { return default; }
}

public class MockAssetService : IAssetService
{
    IAssetService _currencyService;
    IAssetService _challengeModeDataService;
    IAssetService _transactionService;

    public MockAssetService(IAssetService currencyService, IAssetService challengeModeDataService, IAssetService transactionService)
    {
        _currencyService = currencyService;
        _challengeModeDataService = challengeModeDataService;
        _transactionService = transactionService;
    }

    public async Task<Challenge.ChallengeMode.ModeData> GetChallengeModeData(string playerId,
         float leftDuration,
         float decreaseDurationOnMiss,
         float increaseDurationOnClear)
    {
        return await _challengeModeDataService.GetChallengeModeData (playerId, leftDuration, decreaseDurationOnMiss, increaseDurationOnClear);
    }

    public async Task<bool> ProcessTransaction(string playerId, int usedMoney, int earnMoney) 
    {
        return await _transactionService.ProcessTransaction(playerId, usedMoney, earnMoney);
    }

    public async Task<int> GetCurrency(string playerId)
    {
        return await _currencyService.GetCurrency(playerId);
    }
}

public class CurrencyService : IAssetService
{
    public async Task<int> GetCurrency(string playerId) 
    {
        MoneyManager moneyManager = new MoneyManager();
        int money = 0;

        try
        {
            money = await moneyManager.GetMoneyAsync(playerId);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버로부터 데이터를 받아오지 못함");
            return -1;
        }

        return money;
    }
}

public class ChallengeModeDataService : IAssetService
{
    public async Task<Challenge.ChallengeMode.ModeData> GetChallengeModeData(string playerId,
       float playDuration,
       float decreaseDurationOnMiss,
       float increaseDurationOnClear)
    {
        MoneyManager moneyManager = new MoneyManager();
        HintManager hintManager = new HintManager();
        ScoreManager scoreManager = new ScoreManager();

        int money, oneColorHintCost, oneZoneHintCost, maxScore;

        try
        {
            money = await moneyManager.GetMoneyAsync(playerId);
            oneColorHintCost = await hintManager.GetHintPriceAsync(NetworkService.DTO.HintType.OneColorHint);
            oneZoneHintCost = await hintManager.GetHintPriceAsync(NetworkService.DTO.HintType.OneZoneHint);
            maxScore = await scoreManager.GetPlayerWeeklyScoreAsIntAsync(playerId);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버에서 데이터를 받아오지 못 함");
            return null;
        }

        return new Challenge.ChallengeMode.ModeData(
            money,
            oneColorHintCost,
            oneZoneHintCost,
            maxScore,
            playDuration,
            decreaseDurationOnMiss,
            increaseDurationOnClear);
    }
}

public class TransactionService : IAssetService
{
    public async Task<bool> ProcessTransaction(string playerId, int currentMoney, int earnMoney) 
    {
        ScoreManager scoreManager = new ScoreManager();
        MoneyManager moneyManager = new MoneyManager();

        try
        {
            int currentMoneyInServer = await moneyManager.GetMoneyAsync(playerId);
            int usedMoney = currentMoneyInServer - currentMoney;

            await moneyManager.PayPlayerMoneyAsync(playerId, usedMoney);
            await moneyManager.EarnPlayerMoneyAsync(playerId, earnMoney);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버로 데이터를 전송하지 못 함");
            return false;
        }

        return true;
    }
}
