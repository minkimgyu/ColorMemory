using NetworkService.DTO;
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
    Dictionary<NetworkService.DTO.Rank, Sprite> _stageRankIconSprites;

    public StageCreater(
        SpawnableUI prefab, 
        Dictionary<Rank, Sprite> stageRankIconSprites) : base(prefab)
    {
        _stageRankIconSprites = stageRankIconSprites;
    }

    public override SpawnableUI Create()
    {
        SpawnableUI selectStageUI = Object.Instantiate(_prefab);
        selectStageUI.Initialize(_stageRankIconSprites);

        return selectStageUI;
    }
}

public class ArtworkCreater : SpawnableUICreater
{
    Dictionary<int, Sprite> _artSprites;
    Dictionary<NetworkService.DTO.Rank, Sprite> _artworkFrameSprites;
    Dictionary<NetworkService.DTO.Rank, Sprite> _rankIconSprites;

    public ArtworkCreater(
        SpawnableUI prefab,
        Dictionary<int, Sprite> artSprites,
        Dictionary<NetworkService.DTO.Rank, Sprite> artworkFrameSprites,
        Dictionary<NetworkService.DTO.Rank, Sprite> rankIconSprites) : base(prefab)
    {
        _artSprites = artSprites;
        _artworkFrameSprites = artworkFrameSprites;
        _rankIconSprites = rankIconSprites;
    }

    public override SpawnableUI Create(int artworkIndex, NetworkService.DTO.Rank rankType, bool hasIt)
    {
        SpawnableUI artwork = Object.Instantiate(_prefab);
        artwork.Initialize(_artSprites[artworkIndex], _artworkFrameSprites[rankType], _rankIconSprites[rankType], hasIt);
        return artwork;
    }
}

public class FilteredArtworkCreater : SpawnableUICreater
{
    Dictionary<int, Sprite> _artSprites;

    public FilteredArtworkCreater(
        SpawnableUI prefab,
        Dictionary<int, Sprite> artSprites) : base(prefab)
    {
        _artSprites = artSprites;
    }

    public override SpawnableUI Create(int artworkIndex, string title, bool hasIt)
    {
        SpawnableUI artwork = Object.Instantiate(_prefab);
        artwork.Initialize(_artSprites[artworkIndex], title, hasIt);
        return artwork;
    }
}

public class FilterItemCreater : SpawnableUICreater
{
    Dictionary<NetworkService.DTO.Rank, Sprite> _iconSprites;

    public FilterItemCreater(
        SpawnableUI prefab,
        Dictionary<NetworkService.DTO.Rank, Sprite> artSprites) : base(prefab)
    {
        _iconSprites = artSprites;
    }

    public override SpawnableUI Create(NetworkService.DTO.Rank rank, string description)
    {
        SpawnableUI artwork = Object.Instantiate(_prefab);
        artwork.Initialize(_iconSprites[rank], description);
        return artwork;
    }

    public override SpawnableUI Create(string description)
    {
        SpawnableUI artwork = Object.Instantiate(_prefab);
        artwork.Initialize(description);
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



    public virtual SpawnableUI Create(NetworkService.DTO.Rank rank, string description) { return default; }
    public virtual SpawnableUI Create(string description) { return default; }



    public virtual SpawnableUI Create(int artworkIndex, string title, bool hasIt) { return default; }
    public virtual SpawnableUI Create(string name, string description, int reward, int price) { return default; }
    public virtual SpawnableUI Create(int artworkIndex, NetworkService.DTO.Rank frameType, bool hasIt) { return default; }
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
    Dictionary<NetworkService.DTO.Rank, Sprite> _stageRankIconSprites;

    public StageUIFactory(
        SpawnableUI clearPatternUIPrefab, 
        Dictionary<Rank, Sprite> stageRankIconSprites)
    {
        _selectStageCreater = new StageCreater(clearPatternUIPrefab, stageRankIconSprites);
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
        Dictionary<NetworkService.DTO.Rank, Sprite> artworkFrameSprites,
        Dictionary<NetworkService.DTO.Rank, Sprite> rankIconSprites)
    {
        _artworkUICreater = new ArtworkCreater(artworkUIPrefab, artSprites, artworkFrameSprites, rankIconSprites);
    }

    public override SpawnableUI Create(int artworkIndex, NetworkService.DTO.Rank frameType, bool hasIt)
    {
        return _artworkUICreater.Create(artworkIndex, frameType, hasIt);
    }
}

public class FilteredArtworkFactory : BaseFactory
{
    FilteredArtworkCreater _filterItemCreater;

    public FilteredArtworkFactory(
        SpawnableUI prefab,
        Dictionary<int, Sprite> artSprites)
    {
        _filterItemCreater = new FilteredArtworkCreater(prefab, artSprites);
    }

    public override SpawnableUI Create(int artworkIndex, string title, bool hasIt)
    {
        return _filterItemCreater.Create(artworkIndex, title, hasIt);
    }
}

public class FilterItemFactory : BaseFactory
{
    FilterItemCreater _filterItemCreater;

    public FilterItemFactory(
        SpawnableUI prefab,
        Dictionary<NetworkService.DTO.Rank, Sprite> artSprites)
    {
        _filterItemCreater = new FilterItemCreater(prefab, artSprites);
    }

    public override SpawnableUI Create(NetworkService.DTO.Rank rank, string description)
    {
        return _filterItemCreater.Create(rank, description);
    }

    public override SpawnableUI Create(string description)
    {
        return _filterItemCreater.Create(description);
    }
}