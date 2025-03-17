using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using System;

public class AddressableHandler : MonoBehaviour
{
    public enum Label
    {
        Dots,
        Effects,
        ModeTitles,
    }

    HashSet<BaseLoader> _assetLoaders;

    int _successCount;
    int _totalCount;
    Action OnCompleted;
    Action<float> OnProgress;

    public void AddProgressEvent(Action<float> OnProgress)
    {
        this.OnProgress = OnProgress;
    }

    public void Initialize()
    {
        DontDestroyOnLoad(gameObject);

        _successCount = 0;
        _totalCount = 0;
        _assetLoaders = new HashSet<BaseLoader>();
    }

    public Dictionary<Dot.Name, Dot> DotAssets { get; private set; }
    public Dictionary<Effect.Name, Effect> EffectAssets { get; private set; }
    public Dictionary<GameMode.Type, Sprite> ModeTitleIconAssets { get; private set; }

    public void Load(Action OnCompleted)
    {
        _assetLoaders.Add(new DotAssetLoader(Label.Dots, (value, label) => { DotAssets = value; OnSuccess(label); }));
        _assetLoaders.Add(new EffectAssetLoader(Label.Effects, (value, label) => { EffectAssets = value; OnSuccess(label); }));
        _assetLoaders.Add(new ModeTitleIconAssetLoader(Label.ModeTitles, (value, label) => { ModeTitleIconAssets = value; OnSuccess(label); }));

        this.OnCompleted = OnCompleted;
        _totalCount = _assetLoaders.Count;
        foreach (var loader in _assetLoaders)
        {
            loader.Load();
        }
    }

    void OnSuccess(Label label)
    {
        _successCount++;
        Debug.Log(_successCount);
        Debug.Log(label.ToString() + "Success");

        OnProgress?.Invoke((float)_successCount / _totalCount);
        if (_successCount == _totalCount)
        {
            Debug.Log("Complete!");
            OnCompleted?.Invoke();
        }
    }

    public void Release()
    {
        foreach (var loader in _assetLoaders)
        {
            loader.Release();
        }
    }
}
