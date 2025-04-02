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
    public SingleJsonAssetLoader(AddressableHandler.Label label, Action<Value, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
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
    public MultipleJsonAssetLoader(AddressableHandler.Label label, Action<Dictionary<Key, Value>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
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
    public IntMultipleJsonAssetLoader(AddressableHandler.Label label, Action<Dictionary<Key, Value>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
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

public class CollectiveArtJsonAssetLoader : IntMultipleJsonAssetLoader<int, CollectiveArtData>
{
    public CollectiveArtJsonAssetLoader(AddressableHandler.Label label, Action<Dictionary<int, CollectiveArtData>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class ArtworkJsonAssetLoader : SingleJsonAssetLoader<ArtworkDateWrapper>
{
    public ArtworkJsonAssetLoader(AddressableHandler.Label label, Action<ArtworkDateWrapper, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class ChallengeModeStageDataJsonAssetLoader : SingleJsonAssetLoader<Challenge.ChallengeMode.StageDataWrapper>
{
    public ChallengeModeStageDataJsonAssetLoader(AddressableHandler.Label label, Action<Challenge.ChallengeMode.StageDataWrapper, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}