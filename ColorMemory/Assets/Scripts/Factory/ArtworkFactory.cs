using System.Collections.Generic;
using UnityEngine;

public class ArtworkCreater
{
    ArtworkUI _artworkPrefab;

    public ArtworkCreater(ArtworkUI artwork)
    {
        _artworkPrefab = artwork;
    }

    public ArtworkUI Create(Sprite artSprite, Sprite frameSprite)
    {
        ArtworkUI artwork = Object.Instantiate(_artworkPrefab);
        artwork.Initialize(artSprite, frameSprite);
        return artwork;
    }
}

public class ArtworkFactory : BaseFactory
{
    Dictionary<ArtName, ArtworkCreater> _artCreater;

    Dictionary<ArtName, Sprite> _artSprites;
    Dictionary<ArtworkUI.Type, Sprite> _artworkFrameSprites;

    public ArtworkFactory(
        ArtworkUI artworkPrefab,
        Dictionary<ArtName, Sprite> artSprites,
        Dictionary<ArtworkUI.Type, Sprite> artworkFrameSprites)
    {
        _artSprites = artSprites;
        _artworkFrameSprites = artworkFrameSprites;
        _artCreater = new Dictionary<ArtName, ArtworkCreater>();

        // Enum의 모든 요소를 foreach로 순회
        foreach (ArtName name in System.Enum.GetValues(typeof(ArtName)))
        {
            _artCreater.Add(name, new ArtworkCreater(artworkPrefab));
        }
    }

    public override ArtworkUI Create(ArtName name, ArtworkUI.Type frameType)
    {
        return _artCreater[name].Create(_artSprites[name], _artworkFrameSprites[frameType]);
    }
}
