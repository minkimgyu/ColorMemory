using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Threading.Tasks;

public class ServerTest
{
    private ILoginService _loginService;
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
        _artDataIndex = 0; // �׽�Ʈ�� ������ �ٲٱ�
        _userId = "serverTestUser12";
        _userName = "serverTestUser12345";

        _loginService = new MockLoginService(new LoginService());
        _rankingService = new MockRankingService(new WeeklyScoreUpdateService(), new Top10RankingService(), new NearRankingService());
        _iconService = new MockProfileService(new ProfileService());
        _assetService = new MockAssetService(new CurrencyService(), new ChallengeModeDataService(), new TransactionService());
        _artDataService = new MockArtDataService(new ArtDataLoaderService(), new ArtDataUpdaterService());
    }

    [Test]
    public async void IntegratedServerTest()
    {
        // Act
        bool result = await _loginService.Login(_userId, _userName);
        // Assert
        Assert.IsTrue(result, "�α��� ����");






        // Act
        int myScore = 1001;
        bool result1 = await _rankingService.UpdatePlayerWeeklyScore(myScore, _userId);

        // Assert
        Assert.IsTrue(result1, "�ְ� ���� ������Ʈ ����");
        // �Ʒ����� �ڱ� �����Ͱ� �ùٸ��� �ݿ��Ǿ����� Ȯ�� �ʿ���

        int topCount = 10;
        Tuple<List<PersonalRankingData>, PersonalRankingData> result2 = await _rankingService.GetTopRankingData(topCount, _userId);

        // Assert
        Assert.NotNull(result2, "��ŷ �ҷ����� ����");
        // Assert
        Assert.AreEqual(result2.Item2.Score, myScore, "��ŷ ������Ʈ ����");


        int nearRange = 2;
        Tuple<List<PersonalRankingData>, int> result3 = await _rankingService.GetNearRankingData(nearRange, _userId);

        // Assert
        Assert.NotNull(result3, "�ֺ� ��ŷ �ҷ����� ����");
        // Assert
        Assert.AreEqual(result3.Item1[result3.Item2].Score, myScore, "��ŷ ������Ʈ ����");







        const int newIconId = 5;
        // Act
        bool result4 = await _iconService.SetPlayerIconId(_userId, _userName, newIconId);
        // Assert
        Assert.IsTrue(result4, "������ ������ ���� ����");

        // Act
        int iconId = await _iconService.GetPlayerIconId(_userId);
        // Assert
        Assert.AreEqual(newIconId, iconId, $"������ ������ ID�� {newIconId}�̾�� �մϴ�.");






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

        int usedMoney = 0;
        int earnMoney = 300;

        bool result5 = await _assetService.ProcessTransaction(_userId, usedMoney, earnMoney);
        // Assert
        Assert.IsTrue(result5, "���� ����");

        int currency = await _assetService.GetCurrency(_userId);
        Assert.AreEqual(currency, earnMoney - usedMoney, "���� �ݾ��� ���� ����");








        Dictionary<int, ArtData> artDatas1 = await _artDataService.GetArtData(_userId);
        // Assert
        Assert.NotNull(artDatas1, "��Ʈ ������ ���� �� �ҷ����� ����");



    }


        // A Test behaves as an ordinary method
   //[Test]
   // public async void LoginTest()
   // {
   //     // Act
   //     bool result = await _loginService.Login(_userId, _userName);
   //     // Assert
   //     Assert.IsTrue(result, "�α��� ����");
   // }

   //[Test]
   // public async void RankingTest()
   // {
   //     // Act
   //     int myScore = UnityEngine.Random.Range(0, 1000);
   //     bool result = await _rankingService.UpdatePlayerWeeklyScore(myScore, _userId);

   //     // Assert
   //     Assert.IsTrue(result, "�ְ� ���� ������Ʈ ����");
   //     // �Ʒ����� �ڱ� �����Ͱ� �ùٸ��� �ݿ��Ǿ����� Ȯ�� �ʿ���

   //     int topCount = 10;
   //     Tuple<List<PersonalRankingData>, PersonalRankingData> result1 = await _rankingService.GetTopRankingData(topCount, _userId);

   //     // Assert
   //     Assert.NotNull(result1, "��ŷ �ҷ����� ����");
   //     // Assert
   //     Assert.AreNotEqual(result1.Item2.Score, myScore, "��ŷ ������Ʈ ����");


   //     int nearRange = 2;
   //     Tuple<List<PersonalRankingData>, int> result2 = await _rankingService.GetNearRankingData(nearRange, _userId);

   //     // Assert
   //     Assert.NotNull(result2, "�ֺ� ��ŷ �ҷ����� ����");
   //     // Assert
   //     Assert.AreNotEqual(result2.Item1[result2.Item2], myScore, "��ŷ ������Ʈ ����");
   // }

   // [Test]
   // public async void ProfileTest()
   // {
   //     int newIconId = 5;
   //     // Act
   //     bool result = await _iconService.SetPlayerIconId(_userId, _userName, newIconId);
   //     // Assert
   //     Assert.IsTrue(result, "������ ������ ���� ����");

   //     // Act
   //     int iconId = await _iconService.GetPlayerIconId(_userId);
   //     Debug.Log(iconId);
   //     // Assert
   //     Assert.AreNotEqual(newIconId, iconId, $"������ ������ ID�� {newIconId}�̾�� �մϴ�.");
   // }

    //[Test]
    //public async void AssetTest()
    //{
    //    float playDuration = 6;
    //    float decreaseDurationOnMiss = 2;
    //    float increaseDurationOnClear = 3;

    //    Challenge.ChallengeMode.ModeData modeData = await _assetService.GetChallengeModeData(
    //        _userId,
    //        playDuration,
    //        decreaseDurationOnMiss,
    //        increaseDurationOnClear);


    //    // Assert
    //    Assert.NotNull(modeData, "��ŷ �ҷ����� ����");
    //    Assert.AreNotEqual(modeData.PlayDuration, playDuration, "�÷��� �ð��� ���� ����");
    //    Assert.AreNotEqual(modeData.DecreaseDurationOnMiss, decreaseDurationOnMiss, "Miss�� �پ��� �ð��� ���� ����");
    //    Assert.AreNotEqual(modeData.IncreaseDurationOnClear, increaseDurationOnClear, "Ŭ���� �� �����ϴ� �ð��� ���� ����");

    //    int usedMoney = 100;
    //    int earnMoney = 300;

    //    bool result = await _assetService.ProcessTransaction(_userId, usedMoney, earnMoney);
    //    // Assert
    //    Assert.IsTrue(result, "���� ����");

    //    int currency = await _assetService.GetCurrency(_userId);
    //    Assert.AreNotEqual(currency, earnMoney - usedMoney, "���� �ݾ��� ���� ����");
    //}

    //[Test]
    //public async void ArtDataTest()
    //{
    //    // ������Ʈ �� ��Ʈ �����Ͱ� �˸°� �ݿ��Ǿ����� Ȯ�� �ʿ���

    //    // Ŭ���� ���� ��
    //    // ���� ���������� �ر��� ��

    //    Dictionary<int, ArtData> artDatas1 = await _artDataService.GetArtData(_userId);
    //    // Assert
    //    Assert.NotNull(artDatas1, "��Ʈ ������ ���� �� �ҷ����� ����");

        //int ownCount = 0;
        //int unownCount = 0;

        //foreach (var data in artDatas1)
        //{
        //    if (data.Value.HasIt == false) unownCount++;
        //    else ownCount++;
        //}



        //Tuple<NetworkService.DTO.PlayerArtworkDTO, int, int> artData = await _artDataService.GetArtData(_userId, _artDataIndex);
        //// Assert
        //Assert.NotNull(artData, "��Ʈ ������ �ϳ� �ҷ����� ����");

        //Assert.AreNotEqual(artData.Item2, ownCount, "���� ���� ��Ʈ ������ ������ ���� ����");
        //Assert.AreNotEqual(artData.Item3, unownCount, "���� ������ ���� ��Ʈ ������ ������ ���� ����");





        //artData.Item1.Stages[0].Status = NetworkService.DTO.StageStauts.Clear;
        //artData.Item1.Stages[1].Status = NetworkService.DTO.StageStauts.Open;
        //// Ŭ���� �������� ����

        //NetworkService.DTO.Rank? rank = await _artDataService.UpdateArtData(artData.Item1);

        //Tuple<NetworkService.DTO.PlayerArtworkDTO, int, int> updatedArtData = await _artDataService.GetArtData(_userId, _artDataIndex);

        //Assert.AreNotEqual(updatedArtData.Item1.Stages[0].Status, NetworkService.DTO.StageStauts.Clear, "������Ʈ�� �� ��");
        //Assert.AreNotEqual(updatedArtData.Item1.Stages[1].Status, NetworkService.DTO.StageStauts.Open, "������Ʈ�� �� ��");






        //updatedArtData.Item1.HasIt = true;
        //NetworkService.DTO.Rank? newRank = await _artDataService.UpdateArtData(updatedArtData.Item1);

        //Tuple<NetworkService.DTO.PlayerArtworkDTO, int, int> clearedArtData = await _artDataService.GetArtData(_userId, _artDataIndex);

        //Assert.AreNotEqual(clearedArtData.Item1.HasIt, true, "���� ������Ʈ�� �� ��");


        // ���Ŀ� ��ũ ������Ʈ�� �ؼ� �����غ��� �͵� ���ƺ��δ�.
    //}
}
