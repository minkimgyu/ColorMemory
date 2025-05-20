using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class LevelTest
{
    AddressableLoader _addressableLoader;
    ILevelGenerator _mockLevelGenerator;
    bool _isComplete = false;

    AddressableLoader CreateAddressableLoader()
    {
        GameObject addressableObject = new GameObject("AddressableLoader");
        AddressableLoader addressableHandler = addressableObject.AddComponent<AddressableLoader>();
        addressableHandler.Initialize(false);

        return addressableHandler;
    }

    [SetUp]
    public void Setup()
    {
        _isComplete = false;
    }

    [UnityTest]
    public IEnumerator LevelGenerateTest()
    {
        _addressableLoader = CreateAddressableLoader();
        _addressableLoader.Load(() =>
        {
            _isComplete = true;
        });

        // 콜백 완료까지 대기
        yield return new WaitUntil(() => _isComplete);

        _mockLevelGenerator = new MockLevelGenerator(
           new RandomLevelGenerator(_addressableLoader.ChallengeStageJsonDataAsset.StageDatas),
           new CustomLevelGenerator(_addressableLoader.CollectiveArtJsonAssets[0].Sections[0][0])
       );

        bool canGenerate = _mockLevelGenerator.CanGenerateLevelData();
        Assert.AreEqual(canGenerate, true, "레벨 데이터를 생성할 수 없음");

        // 각 레벨에 대한 색 개수, 맵 개수 평가

        try
        {
            MapData challengeMapData1 = _mockLevelGenerator.GenerateLevelData(0);

            Assert.AreEqual(challengeMapData1.PickColors.Count, 1, "레벨 데이터가 잘못됨");
            Assert.AreEqual(challengeMapData1.DotColor.GetLength(0), 1, "레벨 데이터가 잘못됨");
            Assert.AreEqual(challengeMapData1.DotColor.GetLength(1), 1, "레벨 데이터가 잘못됨");

            MapData challengeMapData2 = _mockLevelGenerator.GenerateLevelData(5);

            Assert.AreEqual(challengeMapData2.PickColors.Count, 3, "레벨 데이터가 잘못됨");
            Assert.AreEqual(challengeMapData2.DotColor.GetLength(0), 3, "레벨 데이터가 잘못됨");
            Assert.AreEqual(challengeMapData2.DotColor.GetLength(1), 3, "레벨 데이터가 잘못됨");

            MapData challengeMapData3 = _mockLevelGenerator.GenerateLevelData(10);

            Assert.AreEqual(challengeMapData3.PickColors.Count, 3, "레벨 데이터가 잘못됨");
            Assert.AreEqual(challengeMapData3.DotColor.GetLength(0), 4, "레벨 데이터가 잘못됨");
            Assert.AreEqual(challengeMapData3.DotColor.GetLength(1), 4, "레벨 데이터가 잘못됨");

            MapData challengeMapData4 = _mockLevelGenerator.GenerateLevelData(15);

            Assert.AreEqual(challengeMapData4.PickColors.Count, 4, "레벨 데이터가 잘못됨");
            Assert.AreEqual(challengeMapData4.DotColor.GetLength(0), 5, "레벨 데이터가 잘못됨");
            Assert.AreEqual(challengeMapData4.DotColor.GetLength(1), 5, "레벨 데이터가 잘못됨");

            MapData challengeMapData6 = _mockLevelGenerator.GenerateLevelData(24);

            Assert.AreEqual(challengeMapData6.PickColors.Count, 5, "레벨 데이터가 잘못됨");
            Assert.AreEqual(challengeMapData6.DotColor.GetLength(0), 5, "레벨 데이터가 잘못됨");
            Assert.AreEqual(challengeMapData6.DotColor.GetLength(1), 5, "레벨 데이터가 잘못됨");
        }
        catch (AssertionException e)
        {

            Debug.LogError("Assert 실패: 랜덤 레벨 데이터가 잘못됨 " + e);
            throw; // 테스트는 여전히 실패 처리됨
        }

        Debug.Log("RandomLevelGenerator 성공");

        try
        {
            MapData customMapData = _mockLevelGenerator.GenerateLevelData();

            Assert.AreEqual(customMapData.PickColors.Count, 3, "레벨 데이터가 잘못됨");
            Assert.AreEqual(customMapData.DotColor.GetLength(0), 5, "레벨 데이터가 잘못됨");
            Assert.AreEqual(customMapData.DotColor.GetLength(1), 5, "레벨 데이터가 잘못됨");
        }
        catch (AssertionException e)
        {
            Debug.LogError("Assert 실패: 커스텀 레벨 데이터가 잘못됨 " + e);
            throw; // 테스트는 여전히 실패 처리됨
        }

        Debug.Log("CustomLevelGenerator 성공");
        // 위 2개 항목 평가
    }
}
