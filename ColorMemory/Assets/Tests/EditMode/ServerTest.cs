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
        _artDataIndex = 0; // 테스트할 때마다 바꾸기
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
        Assert.IsTrue(result, "로그인 실패");






        // Act
        int myScore = 1001;
        bool result1 = await _rankingService.UpdatePlayerWeeklyScore(myScore, _userId);

        // Assert
        Assert.IsTrue(result1, "주간 점수 업데이트 실패");
        // 아래에서 자기 데이터가 올바르게 반영되었는지 확인 필요함

        int topCount = 10;
        Tuple<List<PersonalRankingData>, PersonalRankingData> result2 = await _rankingService.GetTopRankingData(topCount, _userId);

        // Assert
        Assert.NotNull(result2, "랭킹 불러오기 실패");
        // Assert
        Assert.AreEqual(result2.Item2.Score, myScore, "랭킹 업데이트 실패");


        int nearRange = 2;
        Tuple<List<PersonalRankingData>, int> result3 = await _rankingService.GetNearRankingData(nearRange, _userId);

        // Assert
        Assert.NotNull(result3, "주변 랭킹 불러오기 실패");
        // Assert
        Assert.AreEqual(result3.Item1[result3.Item2].Score, myScore, "랭킹 업데이트 실패");







        const int newIconId = 5;
        // Act
        bool result4 = await _iconService.SetPlayerIconId(_userId, _userName, newIconId);
        // Assert
        Assert.IsTrue(result4, "프로필 아이콘 설정 실패");

        // Act
        int iconId = await _iconService.GetPlayerIconId(_userId);
        // Assert
        Assert.AreEqual(newIconId, iconId, $"프로필 아이콘 ID는 {newIconId}이어야 합니다.");






        float playDuration = 6;
        float decreaseDurationOnMiss = 2;
        float increaseDurationOnClear = 3;

        Challenge.ChallengeMode.ModeData modeData = await _assetService.GetChallengeModeData(
            _userId,
            playDuration,
            decreaseDurationOnMiss,
            increaseDurationOnClear);


        // Assert
        Assert.NotNull(modeData, "랭킹 불러오기 실패");
        Assert.AreEqual(modeData.PlayDuration, playDuration, "플레이 시간이 맞지 않음");
        Assert.AreEqual(modeData.DecreaseDurationOnMiss, decreaseDurationOnMiss, "Miss시 줄어드는 시간이 맞지 않음");
        Assert.AreEqual(modeData.IncreaseDurationOnClear, increaseDurationOnClear, "클리어 시 증가하는 시간이 맞지 않음");

        int usedMoney = 0;
        int earnMoney = 300;

        bool result5 = await _assetService.ProcessTransaction(_userId, usedMoney, earnMoney);
        // Assert
        Assert.IsTrue(result5, "결제 실패");

        int currency = await _assetService.GetCurrency(_userId);
        Assert.AreEqual(currency, earnMoney - usedMoney, "결제 금액이 맞지 않음");








        Dictionary<int, ArtData> artDatas1 = await _artDataService.GetArtData(_userId);
        // Assert
        Assert.NotNull(artDatas1, "아트 데이터 여러 개 불러오기 실패");



    }


        // A Test behaves as an ordinary method
   //[Test]
   // public async void LoginTest()
   // {
   //     // Act
   //     bool result = await _loginService.Login(_userId, _userName);
   //     // Assert
   //     Assert.IsTrue(result, "로그인 실패");
   // }

   //[Test]
   // public async void RankingTest()
   // {
   //     // Act
   //     int myScore = UnityEngine.Random.Range(0, 1000);
   //     bool result = await _rankingService.UpdatePlayerWeeklyScore(myScore, _userId);

   //     // Assert
   //     Assert.IsTrue(result, "주간 점수 업데이트 실패");
   //     // 아래에서 자기 데이터가 올바르게 반영되었는지 확인 필요함

   //     int topCount = 10;
   //     Tuple<List<PersonalRankingData>, PersonalRankingData> result1 = await _rankingService.GetTopRankingData(topCount, _userId);

   //     // Assert
   //     Assert.NotNull(result1, "랭킹 불러오기 실패");
   //     // Assert
   //     Assert.AreNotEqual(result1.Item2.Score, myScore, "랭킹 업데이트 실패");


   //     int nearRange = 2;
   //     Tuple<List<PersonalRankingData>, int> result2 = await _rankingService.GetNearRankingData(nearRange, _userId);

   //     // Assert
   //     Assert.NotNull(result2, "주변 랭킹 불러오기 실패");
   //     // Assert
   //     Assert.AreNotEqual(result2.Item1[result2.Item2], myScore, "랭킹 업데이트 실패");
   // }

   // [Test]
   // public async void ProfileTest()
   // {
   //     int newIconId = 5;
   //     // Act
   //     bool result = await _iconService.SetPlayerIconId(_userId, _userName, newIconId);
   //     // Assert
   //     Assert.IsTrue(result, "프로필 아이콘 설정 실패");

   //     // Act
   //     int iconId = await _iconService.GetPlayerIconId(_userId);
   //     Debug.Log(iconId);
   //     // Assert
   //     Assert.AreNotEqual(newIconId, iconId, $"프로필 아이콘 ID는 {newIconId}이어야 합니다.");
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
    //    Assert.NotNull(modeData, "랭킹 불러오기 실패");
    //    Assert.AreNotEqual(modeData.PlayDuration, playDuration, "플레이 시간이 맞지 않음");
    //    Assert.AreNotEqual(modeData.DecreaseDurationOnMiss, decreaseDurationOnMiss, "Miss시 줄어드는 시간이 맞지 않음");
    //    Assert.AreNotEqual(modeData.IncreaseDurationOnClear, increaseDurationOnClear, "클리어 시 증가하는 시간이 맞지 않음");

    //    int usedMoney = 100;
    //    int earnMoney = 300;

    //    bool result = await _assetService.ProcessTransaction(_userId, usedMoney, earnMoney);
    //    // Assert
    //    Assert.IsTrue(result, "결제 실패");

    //    int currency = await _assetService.GetCurrency(_userId);
    //    Assert.AreNotEqual(currency, earnMoney - usedMoney, "결제 금액이 맞지 않음");
    //}

    //[Test]
    //public async void ArtDataTest()
    //{
    //    // 업데이트 한 아트 데이터가 알맞게 반영되었는지 확인 필요함

    //    // 클리어 했을 시
    //    // 다음 스테이지를 해금할 시

    //    Dictionary<int, ArtData> artDatas1 = await _artDataService.GetArtData(_userId);
    //    // Assert
    //    Assert.NotNull(artDatas1, "아트 데이터 여러 개 불러오기 실패");

        //int ownCount = 0;
        //int unownCount = 0;

        //foreach (var data in artDatas1)
        //{
        //    if (data.Value.HasIt == false) unownCount++;
        //    else ownCount++;
        //}



        //Tuple<NetworkService.DTO.PlayerArtworkDTO, int, int> artData = await _artDataService.GetArtData(_userId, _artDataIndex);
        //// Assert
        //Assert.NotNull(artData, "아트 데이터 하나 불러오기 실패");

        //Assert.AreNotEqual(artData.Item2, ownCount, "보유 중인 아트 데이터 개수가 맞지 않음");
        //Assert.AreNotEqual(artData.Item3, unownCount, "보유 중이지 않은 아트 데이터 개수가 맞지 않음");





        //artData.Item1.Stages[0].Status = NetworkService.DTO.StageStauts.Clear;
        //artData.Item1.Stages[1].Status = NetworkService.DTO.StageStauts.Open;
        //// 클리어 오픈으로 변경

        //NetworkService.DTO.Rank? rank = await _artDataService.UpdateArtData(artData.Item1);

        //Tuple<NetworkService.DTO.PlayerArtworkDTO, int, int> updatedArtData = await _artDataService.GetArtData(_userId, _artDataIndex);

        //Assert.AreNotEqual(updatedArtData.Item1.Stages[0].Status, NetworkService.DTO.StageStauts.Clear, "업데이트가 안 됨");
        //Assert.AreNotEqual(updatedArtData.Item1.Stages[1].Status, NetworkService.DTO.StageStauts.Open, "업데이트가 안 됨");






        //updatedArtData.Item1.HasIt = true;
        //NetworkService.DTO.Rank? newRank = await _artDataService.UpdateArtData(updatedArtData.Item1);

        //Tuple<NetworkService.DTO.PlayerArtworkDTO, int, int> clearedArtData = await _artDataService.GetArtData(_userId, _artDataIndex);

        //Assert.AreNotEqual(clearedArtData.Item1.HasIt, true, "보유 업데이트가 안 됨");


        // 추후에 랭크 업데이트도 해서 진행해보는 것도 좋아보인다.
    //}
}
