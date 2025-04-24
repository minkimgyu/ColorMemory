using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using System;

public class AddressableLoader : MonoBehaviour
{
    public enum Label
    {
        Dot,
        Effect,
        ModeTitle,

        ArtData,
        ArtSprite,

        ArtworkFrame,

        RectProfileIcon,
        CircleProfileIcon,

        RankBadgeIcon,
        RankDecorationIcon,
        StageRankIcon,

        SpawnableUI,

        ArtworkData,
        LocalizationData,
        ChallengeModeStageData,
    }

    HashSet<BaseLoader> _assetLoaders;

    int _successCount;
    int _totalCount;
    Action OnCompleted;
    Action<float> OnProgress;

    public void Initialize()
    {
        DontDestroyOnLoad(gameObject);

        _successCount = 0;
        _totalCount = 0;
        _assetLoaders = new HashSet<BaseLoader>();
    }

    public void AddProgressEvent(Action<float> OnProgress)
    {
        this.OnProgress = OnProgress;
    }

    public Dictionary<int, CollectArtData> CollectiveArtJsonAsserts { get; private set; }
    public Dictionary<int, Sprite> ArtSpriteAsserts { get; private set; }

    public Dictionary<NetworkService.DTO.Rank, Sprite> ArtworkFrameAssets { get; private set; }
    public Dictionary<NetworkService.DTO.Rank, Sprite> RankDecorationIconAssets { get; private set; }
    public Dictionary<NetworkService.DTO.Rank, Sprite> RankBadgeIconAssets { get; private set; }
    public Dictionary<NetworkService.DTO.Rank, Sprite> StageRankIconAssets { get; private set; }


    public Dictionary<Dot.Name, Dot> DotAssets { get; private set; }
    public Dictionary<Effect.Name, Effect> EffectAssets { get; private set; }
    public Dictionary<GameMode.Type, Sprite> ModeTitleIconAssets { get; private set; }

    public Dictionary<int, Sprite> CircleProfileIconAssets { get; private set; }
    public Dictionary<int, Sprite> RectProfileIconAssets { get; private set; }

    public Dictionary<SpawnableUI.Name, SpawnableUI> SpawnableUIAssets { get; private set; }

    public LevelDataWrapper ChallengeStageJsonDataAsset { get; private set; }
    public Dictionary<ILocalization.Language, ArtworkDateWrapper> ArtworkJsonDataAssets { get; private set; }
    public Localization LocalizationJsonDataAsset { get; private set; }


    public void Load(Action OnCompleted)
    {
        _assetLoaders.Add(new DotAssetLoader(Label.Dot, (value, label) => { DotAssets = value; OnSuccess(label); }));
        _assetLoaders.Add(new EffectAssetLoader(Label.Effect, (value, label) => { EffectAssets = value; OnSuccess(label); }));
        _assetLoaders.Add(new ModeTitleIconAssetLoader(Label.ModeTitle, (value, label) => { ModeTitleIconAssets = value; OnSuccess(label); }));
        
        _assetLoaders.Add(new CollectiveArtJsonAssetLoader(Label.ArtData, (value, label) => { CollectiveArtJsonAsserts = value; OnSuccess(label); }));
        _assetLoaders.Add(new ArtSpriteAssetLoader(Label.ArtSprite, (value, label) => { ArtSpriteAsserts = value; OnSuccess(label); }));

        _assetLoaders.Add(new ArtworkFrameAssetLoader(Label.ArtworkFrame, (value, label) => { ArtworkFrameAssets = value; OnSuccess(label); }));

        _assetLoaders.Add(new ProfileIconAssetLoader(Label.CircleProfileIcon, (value, label) => { CircleProfileIconAssets = value; OnSuccess(label); }));
        _assetLoaders.Add(new ProfileIconAssetLoader(Label.RectProfileIcon, (value, label) => { RectProfileIconAssets = value; OnSuccess(label); }));

        _assetLoaders.Add(new RankIconAssetLoader(Label.RankBadgeIcon, (value, label) => { RankBadgeIconAssets = value; OnSuccess(label); }));
        _assetLoaders.Add(new RankIconAssetLoader(Label.RankDecorationIcon, (value, label) => { RankDecorationIconAssets = value; OnSuccess(label); }));
        _assetLoaders.Add(new RankIconAssetLoader(Label.StageRankIcon, (value, label) => { StageRankIconAssets = value; OnSuccess(label); }));

        _assetLoaders.Add(new SpawnableUIAssetLoader(Label.SpawnableUI, (value, label) => { SpawnableUIAssets = value; OnSuccess(label); }));

        _assetLoaders.Add(new ArtworkDataJsonAssetLoader(Label.ArtworkData, (value, label) => { ArtworkJsonDataAssets = value; OnSuccess(label); }));
        _assetLoaders.Add(new ChallengeModeStageDataJsonAssetLoader(Label.ChallengeModeStageData, (value, label) => { ChallengeStageJsonDataAsset = value; OnSuccess(label); }));
        _assetLoaders.Add(new LocalizationDataJsonAssetLoader(Label.LocalizationData, (value, label) => { LocalizationJsonDataAsset = value; OnSuccess(label); }));

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
