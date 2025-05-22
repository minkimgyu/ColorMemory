using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using System;
using System.Threading.Tasks;

public class WebServerTest
{
    private IAccountService _accountService;
    private IRankingService _rankingService;
    private IProfileService _iconService;
    private IAssetService _assetService;
    private IArtDataService _artDataService;

    // Arrange
    string _userId;
    string _userName;

    int _artDataIndex;

    [SetUp]
    public void Setup()
    {
        //  _userId = "serverTest"
        // �׽�Ʈ ���� �α���
        // �׽�Ʈ ���� ����

        _artDataIndex = 0; // �׽�Ʈ�� ������ �ٲٱ�
        _userId = "serverTestID";
        _userName = "serverTestName";

        _accountService = new MockAccountService(new AccountService());
        _rankingService = new MockRankingService(new WeeklyScoreUpdateService(), new Top10RankingService(), new NearRankingService());
        _iconService = new MockProfileService(new ProfileService());
        _assetService = new MockAssetService(new CurrencyService(), new ChallengeModeDataService(), new TransactionService());
        _artDataService = new MockArtDataService(new ArtDataLoaderService(), new ArtDataUpdaterService());
    }

    async Task TestAccountService_Start()
    {
        // Act
        bool deleteResult = await _accountService.DeleteAccount(_userId);
    }

    async Task TestAccountService_Login()
    {
        // Act
        bool loginResult = await _accountService.Login(_userId, _userName);
        // Assert
        Assert.IsTrue(loginResult, "�α��� ����");
    }

    async Task TestAccountService_DeleteAccount()
    {
        // Act
        bool deleteResult = await _accountService.DeleteAccount(_userId);
        // Assert
        Assert.IsTrue(deleteResult, "���� ���� ����");
    }


    async Task TestRankingService()
    {
        // Act
        int myScore = 1001;
        bool updateResult = await _rankingService.UpdatePlayerWeeklyScore(myScore, _userId);

        // Assert
        Assert.IsTrue(updateResult, "�ְ� ���� ������Ʈ ����");
        // �Ʒ����� �ڱ� �����Ͱ� �ùٸ��� �ݿ��Ǿ����� Ȯ�� �ʿ���

        int topCount = 10;
        Tuple<List<PersonalRankingData>, PersonalRankingData> getTopRankingResult = await _rankingService.GetTopRankingData(topCount, _userId);

        // Assert
        Assert.NotNull(getTopRankingResult, "��ŷ �ҷ����� ����");
        // Assert
        Assert.AreEqual(getTopRankingResult.Item2.Score, myScore, "��ŷ ������Ʈ ����");


        int nearRange = 2;
        Tuple<List<PersonalRankingData>, int> getNearRankingResult = await _rankingService.GetNearRankingData(nearRange, _userId);

        // Assert
        Assert.NotNull(getNearRankingResult, "�ֺ� ��ŷ �ҷ����� ����");
        // Assert
        Assert.AreEqual(getNearRankingResult.Item1[getNearRankingResult.Item2].Score, myScore, "��ŷ ������Ʈ ����");
    }

    async Task TestProfileService()
    {
        const int newIconId = 5;
        // Act
        bool result4 = await _iconService.SetPlayerIconId(_userId, _userName, newIconId);
        // Assert
        Assert.IsTrue(result4, "������ ������ ���� ����");

        // Act
        int iconId = await _iconService.GetPlayerIconId(_userId);
        // Assert
        Assert.AreEqual(newIconId, iconId, $"������ ������ ID�� {newIconId}�̾�� �մϴ�.");
    }

    async Task TestAssetService()
    {
        float playDuration = 6;
        float decreaseDurationOnMiss = 2;
        float increaseDurationOnClear = 3;

        Challenge.ChallengeMode.ModeData modeData = await _assetService.GetChallengeModeData(
            _userId,
            playDuration,
            decreaseDurationOnMiss,
            increaseDurationOnClear);

        // Assert
        Assert.NotNull(modeData, "��ŷ �ҷ����� ����");
        Assert.AreEqual(modeData.PlayDuration, playDuration, "�÷��� �ð��� ���� ����");
        Assert.AreEqual(modeData.DecreaseDurationOnMiss, decreaseDurationOnMiss, "Miss�� �پ��� �ð��� ���� ����");
        Assert.AreEqual(modeData.IncreaseDurationOnClear, increaseDurationOnClear, "Ŭ���� �� �����ϴ� �ð��� ���� ����");


        int startCurrency = await _assetService.GetCurrency(_userId);
        bool transactionResult = await _assetService.PayPlayerMoneyAsync(_userId, startCurrency);
        Assert.IsTrue(transactionResult, "���� ����");

        // ��ȭ 0���� �����

        int earnMoney = 300;

        bool transactionResult1 = await _assetService.EarnPlayerMoneyAsync(_userId, earnMoney);
        // Assert
        Assert.IsTrue(transactionResult1, "��ȭ ȹ�� ����");

        int currency1 = await _assetService.GetCurrency(_userId);
        Assert.AreEqual(currency1, earnMoney, "��ȭ�� ���� ����");


        int payedMoney = 100;

        bool transactionResult2 = await _assetService.PayPlayerMoneyAsync(_userId, payedMoney);
        // Assert
        Assert.IsTrue(transactionResult2, "���� ����");

        int currency2 = await _assetService.GetCurrency(_userId);
        Assert.AreEqual(currency2, earnMoney - payedMoney, "���� �ݾ��� ���� ����");
    }

    async Task TestArtDataService()
    {
        Dictionary<int, ArtData> artDatas1 = await _artDataService.GetArtData(_userId);
        // Assert
        Assert.NotNull(artDatas1, "��Ʈ ������ ���� �� �ҷ����� ����");

        int ownCount = 0;
        int unownCount = 0;

        foreach (var data in artDatas1)
        {
            if (data.Value.HasIt == false) unownCount++;
            else ownCount++;
        }

        Tuple<NetworkService.DTO.PlayerArtworkDTO, int, int> artData = await _artDataService.GetArtData(_userId, _artDataIndex);
        // Assert
        Assert.NotNull(artData, "��Ʈ ������ �ϳ� �ҷ����� ����");

        Assert.AreEqual(artData.Item2, ownCount, "���� ���� ��Ʈ ������ ������ ���� ����");
        Assert.AreEqual(artData.Item3, unownCount, "���� ������ ���� ��Ʈ ������ ������ ���� ����");


        artData.Item1.Stages[0].Status = NetworkService.DTO.StageStauts.Clear;
        artData.Item1.Stages[1].Status = NetworkService.DTO.StageStauts.Open;
        // Ŭ���� �������� ����

        NetworkService.DTO.Rank? rank = await _artDataService.UpdateArtData(artData.Item1);

        Tuple<NetworkService.DTO.PlayerArtworkDTO, int, int> updatedArtData = await _artDataService.GetArtData(_userId, _artDataIndex);

        Assert.AreEqual(updatedArtData.Item1.Stages[0].Status, NetworkService.DTO.StageStauts.Clear, "������Ʈ�� �� ��");
        Assert.AreEqual(updatedArtData.Item1.Stages[1].Status, NetworkService.DTO.StageStauts.Open, "������Ʈ�� �� ��");


        updatedArtData.Item1.HasIt = true;
        NetworkService.DTO.Rank? newRank = await _artDataService.UpdateArtData(updatedArtData.Item1);

        Tuple<NetworkService.DTO.PlayerArtworkDTO, int, int> clearedArtData = await _artDataService.GetArtData(_userId, _artDataIndex);

        Assert.AreEqual(clearedArtData.Item1.HasIt, true, "���� ������Ʈ�� �� ��");
    }

    [UnityTest, Timeout(30000)]
    public IEnumerator ServerTest()
    {
        Task task = TestAccountService_Start();

        // Task�� �Ϸ�� ������ ���
        yield return new WaitUntil(() => task.IsCompleted);

        UnityEngine.Debug.Log("TestAccountService_Start ����");


        Task task1 = TestAccountService_Login();

        // Task�� �Ϸ�� ������ ���
        yield return new WaitUntil(() => task1.IsCompleted);

        UnityEngine.Debug.Log("TestAccountService_Login ����");



        Task task2 = TestRankingService();

        // Task�� �Ϸ�� ������ ���
        yield return new WaitUntil(() => task2.IsCompleted);

        UnityEngine.Debug.Log("TestRankingService ����");





        Task task3 = TestProfileService();

        // Task�� �Ϸ�� ������ ���
        yield return new WaitUntil(() => task3.IsCompleted);

        UnityEngine.Debug.Log("TestProfileService ����");





        Task task4 = TestAssetService();

        // Task�� �Ϸ�� ������ ���
        yield return new WaitUntil(() => task4.IsCompleted);

        UnityEngine.Debug.Log("TestAssetService ����");





        Task task5 = TestArtDataService();

        // Task�� �Ϸ�� ������ ���
        yield return new WaitUntil(() => task5.IsCompleted);

        UnityEngine.Debug.Log("TestArtDataService ����");




        Task task6 = TestAccountService_DeleteAccount();

        // Task�� �Ϸ�� ������ ���
        yield return new WaitUntil(() => task6.IsCompleted);

        UnityEngine.Debug.Log("TestAccountService_DeleteAccount ����");
    }
}
