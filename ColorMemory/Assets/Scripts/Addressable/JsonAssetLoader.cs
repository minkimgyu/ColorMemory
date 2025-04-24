using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;

public class SingleJsonAssetLoader<Value> : SingleAssetLoader<Value, TextAsset>
{
    JsonParser _parser;
    public SingleJsonAssetLoader(AddressableLoader.Label label, Action<Value, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
    {
        _parser = new JsonParser();
    }

    protected override void LoadAsset(TextAsset value)
    {
        _asset = _parser.JsonToObject<Value>(value.text);
    }
}

public class MultipleJsonAssetLoader<Key, Value> : MultipleAssetLoader<Key, Value, TextAsset>
{
    JsonParser _parser;
    public MultipleJsonAssetLoader(AddressableLoader.Label label, Action<Dictionary<Key, Value>, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
    {
        _parser = new JsonParser();
    }

    protected override void LoadAsset(IResourceLocation location, Dictionary<Key, Value> dictionary, Action OnComplete)
    {
        Addressables.LoadAssetAsync<TextAsset>(location).Completed +=
        (handle) =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    Key key = (Key)Enum.Parse(typeof(Key), location.PrimaryKey);
                    Value value = _parser.JsonToObject<Value>(handle.Result);

                    dictionary.Add(key, value);
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

public class IntMultipleJsonAssetLoader<Key, Value> : MultipleAssetLoader<Key, Value, TextAsset>
{
    JsonParser _parser;
    public IntMultipleJsonAssetLoader(AddressableLoader.Label label, Action<Dictionary<Key, Value>, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
    {
        _parser = new JsonParser();
    }

    protected override void LoadAsset(IResourceLocation location, Dictionary<Key, Value> dictionary, Action OnComplete)
    {
        Addressables.LoadAssetAsync<TextAsset>(location).Completed +=
        (handle) =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    Key key = (Key)(object)int.Parse(location.PrimaryKey);
                    Value value = _parser.JsonToObject<Value>(handle.Result);

                    dictionary.Add(key, value);
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

public class CollectiveArtJsonAssetLoader : IntMultipleJsonAssetLoader<int, CollectArtData>
{
    public CollectiveArtJsonAssetLoader(AddressableLoader.Label label, Action<Dictionary<int, CollectArtData>, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class ArtworkDataJsonAssetLoader : MultipleJsonAssetLoader<ILocalization.Language, ArtworkDateWrapper>
{
    public ArtworkDataJsonAssetLoader(AddressableLoader.Label label, Action<Dictionary<ILocalization.Language, ArtworkDateWrapper>, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class ChallengeModeStageDataJsonAssetLoader : SingleJsonAssetLoader<LevelDataWrapper>
{
    public ChallengeModeStageDataJsonAssetLoader(AddressableLoader.Label label, Action<LevelDataWrapper, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class LocalizationDataJsonAssetLoader : SingleJsonAssetLoader<Localization>
{
    public LocalizationDataJsonAssetLoader(AddressableLoader.Label label, Action<Localization, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
    {
    }
}