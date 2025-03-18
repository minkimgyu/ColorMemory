using System.Collections.Generic;
using UnityEngine;

public class ArtworkCreater
{
    Artwork _artworkPrefab;

    public ArtworkCreater(Artwork artwork)
    {
        _artworkPrefab = artwork;
    }

    public Artwork Create(Sprite artSprite, Sprite frameSprite)
    {
        Artwork artwork = Object.Instantiate(_artworkPrefab);
        artwork.Initialize(artSprite, frameSprite);
        return artwork;
    }
}

public class ArtworkFactory : BaseFactory
{
    Dictionary<ArtData.Name, ArtworkCreater> _artCreater;

    Dictionary<ArtData.Name, Sprite> _artSprites;
    Dictionary<Artwork.Type, Sprite> _artworkFrameSprites;

    public ArtworkFactory(
        Artwork artworkPrefab,
        Dictionary<ArtData.Name, Sprite> artSprites,
        Dictionary<Artwork.Type, Sprite> artworkFrameSprites)
    {
        _artSprites = artSprites;
        _artworkFrameSprites = artworkFrameSprites;
        _artCreater = new Dictionary<ArtData.Name, ArtworkCreater>();

        // Enum의 모든 요소를 foreach로 순회
        foreach (ArtData.Name name in System.Enum.GetValues(typeof(ArtData.Name)))
        {
            _artCreater.Add(name, new ArtworkCreater(artworkPrefab));
        }
    }

    public override Artwork Create(ArtData.Name name, Artwork.Type frameType)
    {
        return _artCreater[name].Create(_artSprites[name], _artworkFrameSprites[frameType]);
    }
}
