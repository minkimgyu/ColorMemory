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

        // �ݹ� �Ϸ���� ���
        yield return new WaitUntil(() => _isComplete);

        _mockLevelGenerator = new MockLevelGenerator(
           new RandomLevelGenerator(_addressableLoader.ChallengeStageJsonDataAsset.StageDatas),
           new CustomLevelGenerator(_addressableLoader.CollectiveArtJsonAssets[0].Sections[0][0])
       );

        bool canGenerate = _mockLevelGenerator.CanGenerateLevelData();
        Assert.AreEqual(canGenerate, true, "���� �����͸� ������ �� ����");

        // �� ������ ���� �� ����, �� ���� ��

        try
        {
            MapData challengeMapData1 = _mockLevelGenerator.GenerateLevelData(0);

            Assert.AreEqual(challengeMapData1.PickColors.Count, 1, "���� �����Ͱ� �߸���");
            Assert.AreEqual(challengeMapData1.DotColor.GetLength(0), 1, "���� �����Ͱ� �߸���");
            Assert.AreEqual(challengeMapData1.DotColor.GetLength(1), 1, "���� �����Ͱ� �߸���");

            MapData challengeMapData2 = _mockLevelGenerator.GenerateLevelData(5);

            Assert.AreEqual(challengeMapData2.PickColors.Count, 3, "���� �����Ͱ� �߸���");
            Assert.AreEqual(challengeMapData2.DotColor.GetLength(0), 3, "���� �����Ͱ� �߸���");
            Assert.AreEqual(challengeMapData2.DotColor.GetLength(1), 3, "���� �����Ͱ� �߸���");

            MapData challengeMapData3 = _mockLevelGenerator.GenerateLevelData(10);

            Assert.AreEqual(challengeMapData3.PickColors.Count, 3, "���� �����Ͱ� �߸���");
            Assert.AreEqual(challengeMapData3.DotColor.GetLength(0), 4, "���� �����Ͱ� �߸���");
            Assert.AreEqual(challengeMapData3.DotColor.GetLength(1), 4, "���� �����Ͱ� �߸���");

            MapData challengeMapData4 = _mockLevelGenerator.GenerateLevelData(15);

            Assert.AreEqual(challengeMapData4.PickColors.Count, 4, "���� �����Ͱ� �߸���");
            Assert.AreEqual(challengeMapData4.DotColor.GetLength(0), 5, "���� �����Ͱ� �߸���");
            Assert.AreEqual(challengeMapData4.DotColor.GetLength(1), 5, "���� �����Ͱ� �߸���");

            MapData challengeMapData6 = _mockLevelGenerator.GenerateLevelData(24);

            Assert.AreEqual(challengeMapData6.PickColors.Count, 5, "���� �����Ͱ� �߸���");
            Assert.AreEqual(challengeMapData6.DotColor.GetLength(0), 5, "���� �����Ͱ� �߸���");
            Assert.AreEqual(challengeMapData6.DotColor.GetLength(1), 5, "���� �����Ͱ� �߸���");
        }
        catch (AssertionException e)
        {

            Debug.LogError("Assert ����: ���� ���� �����Ͱ� �߸��� " + e);
            throw; // �׽�Ʈ�� ������ ���� ó����
        }

        Debug.Log("RandomLevelGenerator ����");

        try
        {
            MapData customMapData = _mockLevelGenerator.GenerateLevelData();

            Assert.AreEqual(customMapData.PickColors.Count, 3, "���� �����Ͱ� �߸���");
            Assert.AreEqual(customMapData.DotColor.GetLength(0), 5, "���� �����Ͱ� �߸���");
            Assert.AreEqual(customMapData.DotColor.GetLength(1), 5, "���� �����Ͱ� �߸���");
        }
        catch (AssertionException e)
        {
            Debug.LogError("Assert ����: Ŀ���� ���� �����Ͱ� �߸��� " + e);
            throw; // �׽�Ʈ�� ������ ���� ó����
        }

        Debug.Log("CustomLevelGenerator ����");
        // �� 2�� �׸� ��
    }
}
