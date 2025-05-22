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
        // 테스트 계정 로그인
        // 테스트 계정 삭제

        _artDataIndex = 0; // 테스트할 때마다 바꾸기
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
        Assert.IsTrue(loginResult, "로그인 실패");
    }

    async Task TestAccountService_DeleteAccount()
    {
        // Act
        bool deleteResult = await _accountService.DeleteAccount(_userId);
        // Assert
        Assert.IsTrue(deleteResult, "계정 삭제 실패");
    }


    async Task TestRankingService()
    {
        // Act
        int myScore = 1001;
        bool updateResult = await _rankingService.UpdatePlayerWeeklyScore(myScore, _userId);

        // Assert
        Assert.IsTrue(updateResult, "주간 점수 업데이트 실패");
        // 아래에서 자기 데이터가 올바르게 반영되었는지 확인 필요함

        int topCount = 10;
        Tuple<List<PersonalRankingData>, PersonalRankingData> getTopRankingResult = await _rankingService.GetTopRankingData(topCount, _userId);

        // Assert
        Assert.NotNull(getTopRankingResult, "랭킹 불러오기 실패");
        // Assert
        Assert.AreEqual(getTopRankingResult.Item2.Score, myScore, "랭킹 업데이트 실패");


        int nearRange = 2;
        Tuple<List<PersonalRankingData>, int> getNearRankingResult = await _rankingService.GetNearRankingData(nearRange, _userId);

        // Assert
        Assert.NotNull(getNearRankingResult, "주변 랭킹 불러오기 실패");
        // Assert
        Assert.AreEqual(getNearRankingResult.Item1[getNearRankingResult.Item2].Score, myScore, "랭킹 업데이트 실패");
    }

    async Task TestProfileService()
    {
        const int newIconId = 5;
        // Act
        bool result4 = await _iconService.SetPlayerIconId(_userId, _userName, newIconId);
        // Assert
        Assert.IsTrue(result4, "프로필 아이콘 설정 실패");

        // Act
        int iconId = await _iconService.GetPlayerIconId(_userId);
        // Assert
        Assert.AreEqual(newIconId, iconId, $"프로필 아이콘 ID는 {newIconId}이어야 합니다.");
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
        Assert.NotNull(modeData, "랭킹 불러오기 실패");
        Assert.AreEqual(modeData.PlayDuration, playDuration, "플레이 시간이 맞지 않음");
        Assert.AreEqual(modeData.DecreaseDurationOnMiss, decreaseDurationOnMiss, "Miss시 줄어드는 시간이 맞지 않음");
        Assert.AreEqual(modeData.IncreaseDurationOnClear, increaseDurationOnClear, "클리어 시 증가하는 시간이 맞지 않음");


        int startCurrency = await _assetService.GetCurrency(_userId);
        bool transactionResult = await _assetService.PayPlayerMoneyAsync(_userId, startCurrency);
        Assert.IsTrue(transactionResult, "결제 실패");

        // 재화 0으로 만들기

        int earnMoney = 300;

        bool transactionResult1 = await _assetService.EarnPlayerMoneyAsync(_userId, earnMoney);
        // Assert
        Assert.IsTrue(transactionResult1, "재화 획득 실패");

        int currency1 = await _assetService.GetCurrency(_userId);
        Assert.AreEqual(currency1, earnMoney, "재화가 맞지 않음");


        int payedMoney = 100;

        bool transactionResult2 = await _assetService.PayPlayerMoneyAsync(_userId, payedMoney);
        // Assert
        Assert.IsTrue(transactionResult2, "결제 실패");

        int currency2 = await _assetService.GetCurrency(_userId);
        Assert.AreEqual(currency2, earnMoney - payedMoney, "결제 금액이 맞지 않음");
    }

    async Task TestArtDataService()
    {
        Dictionary<int, ArtData> artDatas1 = await _artDataService.GetArtData(_userId);
        // Assert
        Assert.NotNull(artDatas1, "아트 데이터 여러 개 불러오기 실패");

        int ownCount = 0;
        int unownCount = 0;

        foreach (var data in artDatas1)
        {
            if (data.Value.HasIt == false) unownCount++;
            else ownCount++;
        }

        Tuple<NetworkService.DTO.PlayerArtworkDTO, int, int> artData = await _artDataService.GetArtData(_userId, _artDataIndex);
        // Assert
        Assert.NotNull(artData, "아트 데이터 하나 불러오기 실패");

        Assert.AreEqual(artData.Item2, ownCount, "보유 중인 아트 데이터 개수가 맞지 않음");
        Assert.AreEqual(artData.Item3, unownCount, "보유 중이지 않은 아트 데이터 개수가 맞지 않음");


        artData.Item1.Stages[0].Status = NetworkService.DTO.StageStauts.Clear;
        artData.Item1.Stages[1].Status = NetworkService.DTO.StageStauts.Open;
        // 클리어 오픈으로 변경

        NetworkService.DTO.Rank? rank = await _artDataService.UpdateArtData(artData.Item1);

        Tuple<NetworkService.DTO.PlayerArtworkDTO, int, int> updatedArtData = await _artDataService.GetArtData(_userId, _artDataIndex);

        Assert.AreEqual(updatedArtData.Item1.Stages[0].Status, NetworkService.DTO.StageStauts.Clear, "업데이트가 안 됨");
        Assert.AreEqual(updatedArtData.Item1.Stages[1].Status, NetworkService.DTO.StageStauts.Open, "업데이트가 안 됨");


        updatedArtData.Item1.HasIt = true;
        NetworkService.DTO.Rank? newRank = await _artDataService.UpdateArtData(updatedArtData.Item1);

        Tuple<NetworkService.DTO.PlayerArtworkDTO, int, int> clearedArtData = await _artDataService.GetArtData(_userId, _artDataIndex);

        Assert.AreEqual(clearedArtData.Item1.HasIt, true, "보유 업데이트가 안 됨");
    }

    [UnityTest, Timeout(30000)]
    public IEnumerator ServerTest()
    {
        Task task = TestAccountService_Start();

        // Task가 완료될 때까지 대기
        yield return new WaitUntil(() => task.IsCompleted);

        UnityEngine.Debug.Log("TestAccountService_Start 성공");


        Task task1 = TestAccountService_Login();

        // Task가 완료될 때까지 대기
        yield return new WaitUntil(() => task1.IsCompleted);

        UnityEngine.Debug.Log("TestAccountService_Login 성공");



        Task task2 = TestRankingService();

        // Task가 완료될 때까지 대기
        yield return new WaitUntil(() => task2.IsCompleted);

        UnityEngine.Debug.Log("TestRankingService 성공");





        Task task3 = TestProfileService();

        // Task가 완료될 때까지 대기
        yield return new WaitUntil(() => task3.IsCompleted);

        UnityEngine.Debug.Log("TestProfileService 성공");





        Task task4 = TestAssetService();

        // Task가 완료될 때까지 대기
        yield return new WaitUntil(() => task4.IsCompleted);

        UnityEngine.Debug.Log("TestAssetService 성공");





        Task task5 = TestArtDataService();

        // Task가 완료될 때까지 대기
        yield return new WaitUntil(() => task5.IsCompleted);

        UnityEngine.Debug.Log("TestArtDataService 성공");




        Task task6 = TestAccountService_DeleteAccount();

        // Task가 완료될 때까지 대기
        yield return new WaitUntil(() => task6.IsCompleted);

        UnityEngine.Debug.Log("TestAccountService_DeleteAccount 성공");
    }
}
