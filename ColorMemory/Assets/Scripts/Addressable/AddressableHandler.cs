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
        Dot,
        Effect,
        ModeTitle,

        ArtData,
        ArtSprite,

        Artwork,
        ArtworkFrame,
        ArtworkData,

        Ranking,
        RankingIcon,

        ClearPattern,
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

    public Dictionary<ArtName, CollectiveArtData> CollectiveArtJsonAsserts { get; private set; }

    public ArtworkUI ArtworkAsset { get; private set; }
    public ArtworkDataObject ArtworkJsonAsset { get; private set; }
    public Dictionary<ArtName, Sprite> ArtSpriteAsserts { get; private set; }
    public Dictionary<ArtworkUI.Type, Sprite> ArtworkFrameAsserts { get; private set; }


    public Dictionary<Dot.Name, Dot> DotAssets { get; private set; }
    public Dictionary<Effect.Name, Effect> EffectAssets { get; private set; }
    public Dictionary<GameMode.Type, Sprite> ModeTitleIconAssets { get; private set; }

    public RankingUI RankingAsset { get; private set; }
    public Dictionary<RankingIconName, Sprite> RankingIconAssets { get; private set; }

    public ClearPatternUI ClearPatternAsset { get; private set; }


    public void Load(Action OnCompleted)
    {
        _assetLoaders.Add(new DotAssetLoader(Label.Dot, (value, label) => { DotAssets = value; OnSuccess(label); }));
        _assetLoaders.Add(new EffectAssetLoader(Label.Effect, (value, label) => { EffectAssets = value; OnSuccess(label); }));
        _assetLoaders.Add(new ModeTitleIconAssetLoader(Label.ModeTitle, (value, label) => { ModeTitleIconAssets = value; OnSuccess(label); }));
        
        _assetLoaders.Add(new CollectiveArtJsonAssetLoader(Label.ArtData, (value, label) => { CollectiveArtJsonAsserts = value; OnSuccess(label); }));
        _assetLoaders.Add(new ArtSpriteAssetLoader(Label.ArtSprite, (value, label) => { ArtSpriteAsserts = value; OnSuccess(label); }));

        _assetLoaders.Add(new ArtworkAssetLoader(Label.Artwork, (value, label) => { ArtworkAsset = value; OnSuccess(label); }));
        _assetLoaders.Add(new ArtworkFrameAssetLoader(Label.ArtworkFrame, (value, label) => { ArtworkFrameAsserts = value; OnSuccess(label); }));
        _assetLoaders.Add(new ArtworkJsonAssetLoader(Label.ArtworkData, (value, label) => { ArtworkJsonAsset = value; OnSuccess(label); }));


        _assetLoaders.Add(new RankingAssetLoader(Label.Ranking, (value, label) => { RankingAsset = value; OnSuccess(label); }));
        _assetLoaders.Add(new RankingIconAssetLoader(Label.RankingIcon, (value, label) => { RankingIconAssets = value; OnSuccess(label); }));

        _assetLoaders.Add(new ClearPatternAssetLoader(Label.ClearPattern, (value, label) => { ClearPatternAsset = value; OnSuccess(label); }));

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
