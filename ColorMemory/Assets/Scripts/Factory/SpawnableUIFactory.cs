using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class RankingCreater : SpawnableUICreater
{
    Dictionary<int, Sprite> _rankingIconSprites;

    public RankingCreater(SpawnableUI prefab, Dictionary<int, Sprite> rankingIconSprites) : base(prefab)
    {
        _rankingIconSprites = rankingIconSprites;
    }

    public override SpawnableUI Create(PersonalRankingData data)
    {
        SpawnableUI rankingUI = Object.Instantiate(_prefab);
        rankingUI.Initialize(_rankingIconSprites[data.ProfileIconIndex], data.Name, data.Score, data.Rank);
        return rankingUI;
    }
}

public class ClearPatternCreater : SpawnableUICreater
{
    public ClearPatternCreater(SpawnableUI prefab) : base(prefab)
    {
    }

    public override SpawnableUI Create(int currentStageCount, int totalStageCount, MapData data, Color[] pickColors)
    {
        SpawnableUI clearPatternUI = Object.Instantiate(_prefab);
        clearPatternUI.Initialize(currentStageCount, totalStageCount, data, pickColors);

        return clearPatternUI;
    }
}

public class StageCreater : SpawnableUICreater
{
    public StageCreater(SpawnableUI prefab) : base(prefab)
    {
    }

    public override SpawnableUI Create()
    {
        SpawnableUI selectStageUI = Object.Instantiate(_prefab);
        selectStageUI.Initialize();

        return selectStageUI;
    }
}

public class ArtworkCreater : SpawnableUICreater
{
    Dictionary<int, Sprite> _artSprites;
    Dictionary<NetworkService.DTO.Rank, Sprite> _artworkFrameSprites;

    public ArtworkCreater(
        SpawnableUI prefab,
        Dictionary<int, Sprite> artSprites,
        Dictionary<NetworkService.DTO.Rank, Sprite> artworkFrameSprites) : base(prefab)
    {
        _artSprites = artSprites;
        _artworkFrameSprites = artworkFrameSprites;
    }

    public override SpawnableUI Create(int artworkIndex, NetworkService.DTO.Rank frameType)
    {
        SpawnableUI artwork = Object.Instantiate(_prefab);
        artwork.Initialize(_artSprites[artworkIndex], _artworkFrameSprites[frameType]);
        return artwork;
    }
}

public class ShopBundleCreater : SpawnableUICreater
{
    public ShopBundleCreater(SpawnableUI prefab) : base(prefab)
    {
    }

    public override SpawnableUI Create(string name, string description, int reward, int price)
    {
        SpawnableUI selectStageUI = Object.Instantiate(_prefab);
        selectStageUI.Initialize(name, description, reward, price);

        return selectStageUI;
    }
}

abstract public class SpawnableUICreater
{
    protected SpawnableUI _prefab;

    public SpawnableUICreater(SpawnableUI prefab)
    {
        _prefab = prefab;
    }

    public virtual SpawnableUI Create(string name, string description, int reward, int price) { return default; }
    public virtual SpawnableUI Create(int artworkIndex, NetworkService.DTO.Rank frameType) { return default; }
    public virtual SpawnableUI Create(int currentStageCount, int totalStageCount, MapData data, Color[] pickColors) { return default; }
    public virtual SpawnableUI Create() { return default; }
    public virtual SpawnableUI Create(PersonalRankingData data) { return default; }
}

public class ShopBundleUIFactory : BaseFactory
{
    ShopBundleCreater _shopBundleCreater;

    public ShopBundleUIFactory(SpawnableUI shopBundlePrefab)
    {
        _shopBundleCreater = new ShopBundleCreater(shopBundlePrefab);
    }

    public override SpawnableUI Create(string name, string description, int reward, int price)
    {
        return _shopBundleCreater.Create(name, description, reward, price);
    }
}


public class ClearPatternUIFactory : BaseFactory
{
    ClearPatternCreater _clearPatternCreater;

    public ClearPatternUIFactory(SpawnableUI clearPatternUIPrefab)
    {
        _clearPatternCreater = new ClearPatternCreater(clearPatternUIPrefab);
    }

    public override SpawnableUI Create(int currentStageCount, int totalStageCount, MapData data, Color[] pickColors)
    {
        return _clearPatternCreater.Create(currentStageCount, totalStageCount, data, pickColors);
    }
}

public class StageUIFactory : BaseFactory
{
    StageCreater _selectStageCreater;

    public StageUIFactory(SpawnableUI clearPatternUIPrefab)
    {
        _selectStageCreater = new StageCreater(clearPatternUIPrefab);
    }

    public override SpawnableUI Create()
    {
        return _selectStageCreater.Create();
    }
}

public class RankingUIFactory : BaseFactory
{
    RankingCreater _rankingUICreater;

    public RankingUIFactory(
        SpawnableUI rankingUIPrefab,
        Dictionary<int, Sprite> rankingIconSprites)
    {
        _rankingUICreater = new RankingCreater(rankingUIPrefab, rankingIconSprites);
    }

    public override SpawnableUI Create(PersonalRankingData data)
    {
        return _rankingUICreater.Create(data);
    }
}


public class ArtworkUIFactory : BaseFactory
{
    ArtworkCreater _artworkUICreater;

    public ArtworkUIFactory(
        SpawnableUI artworkUIPrefab,
        Dictionary<int, Sprite> artSprites,
        Dictionary<NetworkService.DTO.Rank, Sprite> artworkFrameSprites)
    {
        _artworkUICreater = new ArtworkCreater(artworkUIPrefab, artSprites, artworkFrameSprites);
    }

    public override SpawnableUI Create(int artworkIndex, NetworkService.DTO.Rank frameType)
    {
        return _artworkUICreater.Create(artworkIndex, frameType);
    }
}
