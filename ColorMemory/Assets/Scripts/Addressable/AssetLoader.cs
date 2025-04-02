using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;

public class ModeTitleIconAssetLoader : AssetLoader<GameMode.Type, Sprite, Sprite>
{
    public ModeTitleIconAssetLoader(AddressableHandler.Label label, Action<Dictionary<GameMode.Type, Sprite>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class ArtSpriteAssetLoader : IntKeyAssetLoader<int, Sprite, Sprite>
{
    public ArtSpriteAssetLoader(AddressableHandler.Label label, Action<Dictionary<int, Sprite>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class ArtworkFrameAssetLoader : AssetLoader<NetworkService.DTO.Rank, Sprite, Sprite>
{
    public ArtworkFrameAssetLoader(AddressableHandler.Label label, Action<Dictionary<NetworkService.DTO.Rank, Sprite>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class RankingIconAssetLoader : AssetLoader<RankingIconName, Sprite, Sprite>
{
    public RankingIconAssetLoader(AddressableHandler.Label label, Action<Dictionary<RankingIconName, Sprite>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

//public class GoldIconAssetLoader : SingleAssetLoader<Sprite, Sprite>
//{
//    public GoldIconAssetLoader(AddressableHandler.Label label, Action<Sprite, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
//    {
//    }

//    protected override void LoadAsset(Sprite item) => _asset = item;
//}


abstract public class AssetLoader<Key, Value, Type> : MultipleAssetLoader<Key, Value, Type>
{
    protected AssetLoader(AddressableHandler.Label label, Action<Dictionary<Key, Value>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }

    protected override void LoadAsset(IResourceLocation location, Dictionary<Key, Value> dictionary, Action OnComplete)
    {
        Addressables.LoadAssetAsync<Value>(location).Completed +=
        (handle) =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    Key key = (Key)Enum.Parse(typeof(Key), location.PrimaryKey);

                    dictionary.Add(key, handle.Result);
                    OnComplete?.Invoke();
                    break;

                case AsyncOperationStatus.Failed:
                    break;

                default:
                    break;
            }
        };
    }
}

abstract public class IntKeyAssetLoader<Key, Value, Type> : MultipleAssetLoader<Key, Value, Type>
{
    protected IntKeyAssetLoader(AddressableHandler.Label label, Action<Dictionary<Key, Value>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }

    protected override void LoadAsset(IResourceLocation location, Dictionary<Key, Value> dictionary, Action OnComplete)
    {
        Addressables.LoadAssetAsync<Value>(location).Completed +=
        (handle) =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    Key key = (Key)(object)int.Parse(location.PrimaryKey);

                    dictionary.Add(key, handle.Result);
                    OnComplete?.Invoke();
                    break;

                case AsyncOperationStatus.Failed:
                    break;

                default:
                    break;
            }
        };
    }
}
