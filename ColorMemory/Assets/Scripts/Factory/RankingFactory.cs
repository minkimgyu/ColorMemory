using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingCreater
{
    RankingUI _rankingPrefab;

    public RankingCreater(RankingUI rankingPrefab)
    {
        _rankingPrefab = rankingPrefab;
    }

    public RankingUI Create(Sprite profileSprite, string title, int score, int rank)
    {
        RankingUI rankingUI = Object.Instantiate(_rankingPrefab);
        rankingUI.Initialize(profileSprite, title, score, rank);
        return rankingUI;
    }
}

public class RankingFactory : BaseFactory
{
    RankingCreater _rankingCreater;
    Dictionary<RankingIconName, Sprite> _rankingIconSprites;

    public RankingFactory(
        RankingUI rankingPrefab,
        Dictionary<RankingIconName, Sprite> rankingIconSprites)
    {
        _rankingCreater = new RankingCreater(rankingPrefab);
        _rankingIconSprites = rankingIconSprites;
    }

    public override RankingUI Create(RankingData data)
    {
        return _rankingCreater.Create(
            _rankingIconSprites[data.IconName],
            data.Name,
            data.Score,
            data.Rank);
    }
}
