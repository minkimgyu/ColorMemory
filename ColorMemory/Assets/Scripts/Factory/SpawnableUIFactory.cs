using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class RankingCreater : SpawnableUICreater
{
    Dictionary<RankingIconName, Sprite> _rankingIconSprites;

    public RankingCreater(SpawnableUI prefab, Dictionary<RankingIconName, Sprite> rankingIconSprites) : base(prefab)
    {
        _rankingIconSprites = rankingIconSprites;
    }

    public override SpawnableUI Create(PersonalRankingData data)
    {
        SpawnableUI rankingUI = Object.Instantiate(_prefab);
        rankingUI.Initialize(_rankingIconSprites[data.IconName], data.Name, data.Score, data.Rank);
        return rankingUI;
    }
}

public class ClearPatternCreater : SpawnableUICreater
{
    public ClearPatternCreater(SpawnableUI prefab) : base(prefab)
    {
    }

    public override SpawnableUI Create(MapData data, Color[] pickColors)
    {
        SpawnableUI clearPatternUI = Object.Instantiate(_prefab);
        clearPatternUI.Initialize(data, pickColors);

        return clearPatternUI;
    }
}

public class StageCreater : SpawnableUICreater
{
    public StageCreater(SpawnableUI prefab) : base(prefab)
    {
    }

    public override SpawnableUI Create(
        List<List<CollectiveArtData.Block>> blocks,
        List<List<CollectiveArtData.Color>> usedColors)
    {
        SpawnableUI selectStageUI = Object.Instantiate(_prefab);
        selectStageUI.Initialize(blocks, usedColors);

        return selectStageUI;
    }
}

public class ArtworkCreater : SpawnableUICreater
{
    Dictionary<ArtName, Sprite> _artSprites;
    Dictionary<ArtworkUI.Type, Sprite> _artworkFrameSprites;

    public ArtworkCreater(
        SpawnableUI prefab,
        Dictionary<ArtName, Sprite> artSprites,
        Dictionary<ArtworkUI.Type, Sprite> artworkFrameSprites) : base(prefab)
    {
        _artSprites = artSprites;
        _artworkFrameSprites = artworkFrameSprites;
    }

    public override SpawnableUI Create(ArtName name, ArtworkUI.Type frameType)
    {
        SpawnableUI artwork = Object.Instantiate(_prefab);
        artwork.Initialize(_artSprites[name], _artworkFrameSprites[frameType]);
        return artwork;
    }
}

abstract public class SpawnableUICreater
{
    protected SpawnableUI _prefab;

    public SpawnableUICreater(SpawnableUI prefab)
    {
        _prefab = prefab;
    }
    

    public virtual SpawnableUI Create(ArtName name, ArtworkUI.Type frameType) { return default; }
    public virtual SpawnableUI Create(MapData data, Color[] pickColors) { return default; }
    public virtual SpawnableUI Create(
        List<List<CollectiveArtData.Block>> blocks,
        List<List<CollectiveArtData.Color>> usedColors) { return default; }
    public virtual SpawnableUI Create(PersonalRankingData data) { return default; }
}




public class ClearPatternUIFactory : BaseFactory
{
    ClearPatternCreater _clearPatternCreater;

    public ClearPatternUIFactory(SpawnableUI clearPatternUIPrefab)
    {
        _clearPatternCreater = new ClearPatternCreater(clearPatternUIPrefab);
    }

    public override SpawnableUI Create(MapData data, Color[] pickColors)
    {
        return _clearPatternCreater.Create(data, pickColors);
    }
}

public class StageUIFactory : BaseFactory
{
    StageCreater _selectStageCreater;

    public StageUIFactory(SpawnableUI clearPatternUIPrefab)
    {
        _selectStageCreater = new StageCreater(clearPatternUIPrefab);
    }

    public override SpawnableUI Create(
        List<List<CollectiveArtData.Block>> blocks,
        List<List<CollectiveArtData.Color>> usedColors)
    {
        return _selectStageCreater.Create(blocks, usedColors);
    }
}

public class RankingUIFactory : BaseFactory
{
    RankingCreater _rankingUICreater;

    public RankingUIFactory(
        SpawnableUI rankingUIPrefab,
        Dictionary<RankingIconName, Sprite> rankingIconSprites)
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
        Dictionary<ArtName, Sprite> artSprites,
        Dictionary<ArtworkUI.Type, Sprite> artworkFrameSprites)
    {
        _artworkUICreater = new ArtworkCreater(artworkUIPrefab, artSprites, artworkFrameSprites);
    }

    public override SpawnableUI Create(ArtName artworkName, ArtworkUI.Type frameType)
    {
        return _artworkUICreater.Create(artworkName, frameType);
    }
}
