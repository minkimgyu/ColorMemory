using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

public interface ISaveable
{
    bool VerifyJson(string json) { return default; }
    bool HaveSaveFile() { return false; }

    void Save() { }
    void ClearSave() { }

    void Load() { }

    void ChangeGoToCollectPage(bool goToCollectPage) { }

    void ChangeBGMMute(bool nowMute) { }
    void ChangeSFXMute(bool nowMute) { }

    void ChangeBGMVolume(float volume) { }
    void ChangeSFXVolume(float volume) { }
    void ChangeGameModeType(GameMode.Type type) { }
    void SelectArtwork(string selectedArtworkName, Vector2Int selectedArtworkSectionIndex) { }
    void SelectArtwork(Vector2Int selectedArtworkSectionIndex) { }

    void ChangeMoney(int money) { }

    void ChangeHighScore(int highScore) { }
    void ChangeOneColorHintCount(int oneColorHintCount) { }
    void ChangeOneZoneHintCount(int oneZoneHintCount) { }
    void ChangeArtworkData(List<PlayerArtwork> artworks) { }

    SaveData GetSaveData() { return default; }
}

public class NullSaveManager : ISaveable { }

public class PlayerArtwork
{
    string artName;
    bool hasIt;
    Rank rank;
}

public struct SaveData
{
    [JsonProperty] bool _muteBGM;
    [JsonProperty] bool _muteSFX;

    [JsonProperty] float _bgmVolume;
    [JsonProperty] float _sfxVolume;
    [JsonProperty] [JsonConverter(typeof(StringEnumConverter))] GameMode.Type _selectedType;

    [JsonProperty] string _selectedArtworkName;
    [JsonProperty] Vector2Int _selectedArtworkSectionIndex;
    [JsonProperty] bool _goToCollectPage;

    [JsonIgnore] string _name;
    [JsonIgnore] int _money; // ���
    [JsonIgnore] int _highScore;

    [JsonIgnore] int _oneColorHintCount;
    [JsonIgnore] int _oneZoneHintCount;
    [JsonIgnore] List<PlayerArtwork> _artworkDatas;

    public SaveData(string name)
    {
        _muteBGM = false;
        _muteSFX = false;

        _bgmVolume = 1f;
        _sfxVolume = 1f;

        _selectedType = GameMode.Type.Collect;
        _selectedArtworkName = "";
        _selectedArtworkSectionIndex = Vector2Int.zero;

        _name = name;
        _money = 0;
        _highScore = 0;
        _oneColorHintCount = 0;
        _oneZoneHintCount = 0;
        _artworkDatas = new List<PlayerArtwork>();
    }

    public SaveData(
        string name,
        int money,
        int highScore,
        int oneColorHintCount,
        int oneZoneHintCount,
        List<PlayerArtwork> artworkDatas)
    {
        _muteBGM = false;
        _muteSFX = false;

        _bgmVolume = 1f;
        _sfxVolume = 1f;

        _selectedType = GameMode.Type.Collect;
        _selectedArtworkKey = 0;
        _selectedArtworkSectionIndex = Vector2Int.zero;
        _goToCollectPage = false;
    }

    [JsonIgnore] public bool MuteBGM { get => _muteBGM; set => _muteBGM = value; }
    [JsonIgnore] public bool MuteSFX { get => _muteSFX; set => _muteSFX = value; }
    [JsonIgnore] public float BgmVolume { get => _bgmVolume; set => _bgmVolume = value; }
    [JsonIgnore] public float SfxVolume { get => _sfxVolume; set => _sfxVolume = value; }
    [JsonIgnore] public GameMode.Type SelectedType { get => _selectedType; set => _selectedType = value; }
    [JsonIgnore] public string Name { get => _name; set => _name = value; }
    [JsonIgnore] public int Money { get => _money; set => _money = value; }
    [JsonIgnore] public int HighScore { get => _highScore; set => _highScore = value; }
    [JsonIgnore] public int OneColorHintCount { get => _oneColorHintCount; set => _oneColorHintCount = value; }
    [JsonIgnore] public int OneZoneHintCount { get => _oneZoneHintCount; set => _oneZoneHintCount = value; }
    [JsonIgnore] public List<PlayerArtwork> ArtworkDatas { get => _artworkDatas; set => _artworkDatas = value; }
    [JsonIgnore] public string SelectedArtworkName { get => _selectedArtworkName; set => _selectedArtworkName = value; }
    [JsonIgnore] public Vector2Int SelectedArtworkSectionIndex { get => _selectedArtworkSectionIndex; set => _selectedArtworkSectionIndex = value; }
    [JsonIgnore] public int SelectedArtworkSectionIntIndex 
    { 
        get
        {
            return SelectedArtworkSectionIndex.x * ArtworkSize + SelectedArtworkSectionIndex.y;
        }
    }

    [JsonIgnore] public float SelectedArtworkProgress { get => (float)((_selectedArtworkSectionIndex.x * ArtworkSize) + _selectedArtworkSectionIndex.y) / (float)(ArtworkSize * ArtworkSize);  }
    [JsonIgnore] public string UserId { get => _userId; set => _userId = value; }
    [JsonIgnore] public string UserName { get => _userName; set => _userName = value; }
    [JsonIgnore] public bool GoToCollectPage { get => _goToCollectPage; set => _goToCollectPage = value; }

    [JsonIgnore] const int ArtworkSize = 4;
}

public class SaveManager : ISaveable
{
    JsonParser _parser;
    string _filePath;

    SaveData _defaultSaveData;
    SaveData _saveData;

    public SaveManager(SaveData defaultSaveData)
    {
        _parser = new JsonParser();
        _defaultSaveData = defaultSaveData;
        _filePath = Application.persistentDataPath + "/SaveData.txt";
        Debug.Log(_filePath);
        Load();
    }

    public SaveData GetSaveData()
    {
        return _saveData;
    }

    public void ClearSave() 
    {
        if (HaveSaveFile())
        {
            _saveData = _defaultSaveData;
            Save();
            return;
        }
    }

    public string GetSaveJsonData()
    {
        // ������ �������� �ʴ´ٸ�
        if (!HaveSaveFile())
        {
            _saveData = _defaultSaveData;
            Save(); // ���̺� ������ ������ְ� �����Ѵ�.
        }

        // ����� ������ �ҷ��� �����Ѵ�.
        string json = File.ReadAllText(_filePath);
        return json;
    }


    /// <summary>
    /// GPGS�� ������ ����
    /// ���� �������� ���� �����Ͱ� ���峭 ��� ���� �����͸� �������� �ʰ�
    /// �״�� ���
    /// </summary>

    public bool VerifyJson(string json)
    {
        // �ҷ����� �� ������ �ִٸ� �⺻ �����͸� �Ѱ��ش�.
        try
        {
            _saveData = _parser.JsonToObject<SaveData>(json);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            _saveData = _defaultSaveData;
            return false; // ��ȿ���� ����
        }

        return true; // ��ȿ��
    }

    public bool HaveSaveFile()
    {
        return File.Exists(_filePath);
    }

    public void Load()
    {
        // ������ �������� �ʴ´ٸ�
        if (!HaveSaveFile())
        {
            _saveData = _defaultSaveData; // �⺻ ���̺�� ��ü���ش�.
            Save();
            return;
        }

        string json = File.ReadAllText(_filePath);
        bool nowValidate = VerifyJson(json);
        Save();
    }

    public void Save()
    {
        string json = _parser.ObjectToJson(_saveData);
        File.WriteAllText(_filePath, json);
    }

    public void ChangeGoToCollectPage(bool goToCollectPage)
    {
        _saveData.GoToCollectPage = goToCollectPage;
        Save();
    }

    public void ChangeUserData(string id, string name)
    {
        _saveData.UserId = id;
        _saveData.UserName = name;
        Save();
    }

    public void ChangeBGMMute(bool nowMute)
    {
        _saveData.MuteBGM = nowMute;
        Save();
    }

    public void ChangeSFXMute(bool nowMute)
    {
        _saveData.MuteSFX = nowMute;
        Save();
    }

    public void ChangeBGMVolume(float volume) 
    {
        _saveData.BgmVolume = volume;
        Save();
    }

    public void ChangeSFXVolume(float volume)
    {
        _saveData.SfxVolume = volume;
        Save();
    }

    public void ChangeGameModeType(GameMode.Type type) 
    {
        _saveData.SelectedType = type;
        Save();
    }

    public void ChangeMoney(int money) 
    {
        _saveData.Money = money;
        Save();
    }

    public void SelectArtwork(Vector2Int selectedArtworkSectionIndex)
    {
        _saveData.SelectedArtworkSectionIndex = selectedArtworkSectionIndex;
        Save();
    }

    public void SelectArtwork(string selectedArtworkName, Vector2Int selectedArtworkSectionIndex) 
    {
        _saveData.SelectedArtworkName = selectedArtworkName;
        _saveData.SelectedArtworkSectionIndex = selectedArtworkSectionIndex;
        Save();
    }

    public void ChangeHighScore(int highScore) 
    {
        _saveData.HighScore = highScore;
        Save();
    }

    public void ChangeOneColorHintCount(int oneColorHintCount) 
    {
        _saveData.OneColorHintCount = oneColorHintCount;
        Save();
    }

    public void ChangeOneZoneHintCount(int oneZoneHintCount) 
    {
        _saveData.OneZoneHintCount = oneZoneHintCount;
        Save();
    }

    public void ChangeArtworkData(List<PlayerArtwork> data) 
    {
        _saveData.ArtworkDatas = data;
        Save();
    }
}
