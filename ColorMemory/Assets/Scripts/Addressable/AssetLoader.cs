using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;

public class ModeTitleIconAssetLoader : AssetLoader<GameMode.Type, Sprite, Sprite>
{
    public ModeTitleIconAssetLoader(AddressableLoader.Label label, Action<Dictionary<GameMode.Type, Sprite>, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class ArtSpriteAssetLoader : IntKeyAssetLoader<Sprite, Sprite>
{
    public ArtSpriteAssetLoader(AddressableLoader.Label label, Action<Dictionary<int, Sprite>, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class ArtworkFrameAssetLoader : AssetLoader<NetworkService.DTO.Rank, Sprite, Sprite>
{
    public ArtworkFrameAssetLoader(AddressableLoader.Label label, Action<Dictionary<NetworkService.DTO.Rank, Sprite>, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class ProfileIconAssetLoader : IntKeyAssetLoader<Sprite, Sprite>
{
    public ProfileIconAssetLoader(AddressableLoader.Label label, Action<Dictionary<int, Sprite>, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class RankIconAssetLoader : AssetLoader<NetworkService.DTO.Rank, Sprite, Sprite>
{
    public RankIconAssetLoader(AddressableLoader.Label label, Action<Dictionary<NetworkService.DTO.Rank, Sprite>, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class SoundAssetLoader : AssetLoader<ISoundPlayable.SoundName, AudioClip, AudioClip>
{
    public SoundAssetLoader(AddressableLoader.Label label, Action<Dictionary<ISoundPlayable.SoundName, AudioClip>, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
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
    protected AssetLoader(AddressableLoader.Label label, Action<Dictionary<Key, Value>, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
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

abstract public class IntKeyAssetLoader<Value, Type> : MultipleAssetLoader<int, Value, Type>
{
    protected IntKeyAssetLoader(AddressableLoader.Label label, Action<Dictionary<int, Value>, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
    {
    }

    protected override void LoadAsset(IResourceLocation location, Dictionary<int, Value> dictionary, Action OnComplete)
    {
        Addressables.LoadAssetAsync<Value>(location).Completed +=
        (handle) =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    int key = int.Parse(location.PrimaryKey);

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

public class GoldIconAssetLoader : SingleAssetLoader<Sprite, Sprite>
{
    public GoldIconAssetLoader(AddressableLoader.Label label, Action<Sprite, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
    {
    }

    protected override void LoadAsset(Sprite item) => _asset = item;
}