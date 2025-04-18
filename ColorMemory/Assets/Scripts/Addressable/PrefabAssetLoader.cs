using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;

public class EffectAssetLoader : MultiplePrafabAssetLoader<Effect.Name, Effect>
{
    public EffectAssetLoader(AddressableLoader.Label label, Action<Dictionary<Effect.Name, Effect>, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class DotAssetLoader : MultiplePrafabAssetLoader<Dot.Name, Dot>
{
    public DotAssetLoader(AddressableLoader.Label label, Action<Dictionary<Dot.Name, Dot>, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class SpawnableUIAssetLoader : MultiplePrafabAssetLoader<SpawnableUI.Name, SpawnableUI>
{
    public SpawnableUIAssetLoader(AddressableLoader.Label label, Action<Dictionary<SpawnableUI.Name, SpawnableUI>, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

abstract public class MultiplePrafabAssetLoader<Key, Value> : MultipleAssetLoader<Key, Value, GameObject>
{
    protected MultiplePrafabAssetLoader(AddressableLoader.Label label, Action<Dictionary<Key, Value>, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
    {
    }

    protected override void LoadAsset(IResourceLocation location, Dictionary<Key, Value> dictionary, Action OnComplete)
    {
        Addressables.LoadAssetAsync<GameObject>(location).Completed +=
        (handle) =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    Key key = (Key)Enum.Parse(typeof(Key), location.PrimaryKey);
                    Value value = handle.Result.GetComponent<Value>();

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

abstract public class SinglePrafabAssetLoader<Value> : SingleAssetLoader<Value, GameObject>
{
    protected SinglePrafabAssetLoader(AddressableLoader.Label label, Action<Value, AddressableLoader.Label> OnComplete) : base(label, OnComplete)
    {
    }

    protected override void LoadAsset(GameObject value)
    {
        _asset = value.GetComponent<Value>();
    }
}