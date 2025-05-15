using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public interface ILocalization
{
    public enum Language
    {
        Korean,
        English
    }

    // 보스 등장
    // 스테이지 클리어

    public enum Key
    {
        RememberTitle,
        PreviewTitle,
        PreviewContent1,
        PreviewContent2,
        GetCoin,
        StageSelectTitle,

        HintUsage,
        WrongCount,

        Count,
        Counts,

        GameSettingTitle,
        GameExitBtn,
        SFXTitle,
        BGMTitle,
        Mute,
        IncreaseSound,
        DecreaseSound,

        CompleteArtworkResultTitle,
        ProgressArtworkResultTitle,

        GetRankTitle,
        MyCollectionTitle,
        CurrentCollectionComplete,
        TotalCollectionComplete,
        CollectionClearTitle,
        CollectionCompleteTitle,
        CollectionClearContent,
        CollectionCompleteContent,
        RankPageTitle,

        FilterTitle,
        FilterCheerInfo,

        OwnFilterTitle,
        RankFilterTitle,
        DateFilterTitle,

        OldFilter,
        NewFilter,

        NoFilteredArtwork,

        ShopAdBtnInfo,
        ShopBundleBuyInfo,
        ShopBundle1Title,
        ShopBundle1Content,
        ShopBundle2Title,
        ShopBundle2Content,
        ShopBundle3Title,
        ShopBundle3Content,
        Rank,
        Progress
    }

    string GetWord(Key key);
    string GetWord(string key);
}

[Serializable]
public struct Localization
{
    [JsonProperty] Dictionary<ILocalization.Key, Dictionary<ILocalization.Language, string>> _word;
    [JsonIgnore] public Dictionary<ILocalization.Key, Dictionary<ILocalization.Language, string>> Word { get => _word; }

    public Localization(Dictionary<ILocalization.Key, Dictionary<ILocalization.Language, string>> word)
    {
        _word = word;
    }
}

public class NullLocalizationManager : ILocalization
{
    public string GetWord(ILocalization.Key key) { return default; }
    public string GetWord(string key) { return default; }
}

public class LocalizationManager : ILocalization
{
    Localization _localization;

    public LocalizationManager(Localization localization)
    {
        _localization = localization;
    }

    public string GetWord(ILocalization.Key key)
    {
        SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();
        ILocalization.Language language = saveData.Language;

        if (_localization.Word.ContainsKey(key) == false || _localization.Word[key].ContainsKey(language) == false) return string.Empty;
        return _localization.Word[key][language];
    }

    public string GetWord(string key)
    {
        ILocalization.Key nameKey;

        bool canParse = Enum.TryParse<ILocalization.Key>(key, out nameKey);
        if(canParse == false) return string.Empty;

        SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();
        ILocalization.Language language = saveData.Language;

        if (_localization.Word.ContainsKey(nameKey) == false || _localization.Word[nameKey].ContainsKey(language) == false) return string.Empty;
        return _localization.Word[nameKey][language];
    }
}
