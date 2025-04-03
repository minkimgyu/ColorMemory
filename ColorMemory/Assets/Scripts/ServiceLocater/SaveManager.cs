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

    void ChangeBGMMute(bool nowMute) { }
    void ChangeSFXMute(bool nowMute) { }

    void ChangeBGMVolume(float volume) { }
    void ChangeSFXVolume(float volume) { }
    void ChangeGameModeType(GameMode.Type type) { }

    void SelectArtwork(int selectedArtworkIndex) { }
    void SelectArtworkSection(int selectedArtworkSectionIndex) { }
    void SelectArtworkSection(Vector2Int index) { }

    SaveData GetSaveData() { return default; }
}

public class NullSaveManager : ISaveable { }

public class PlayerArtwork
{
    string artName;
    bool hasIt;
    NetworkService.DTO.Rank rank;
}

public struct SaveData
{
    [JsonProperty] bool _muteBGM;
    [JsonProperty] bool _muteSFX;

    [JsonProperty] float _bgmVolume;
    [JsonProperty] float _sfxVolume;
    [JsonProperty] [JsonConverter(typeof(StringEnumConverter))] GameMode.Type _selectedType;

    [JsonProperty] int _selectedArtworkKey;
    [JsonProperty] Vector2Int _selectedArtworkSectionIndex;

    public SaveData(string name)
    {
        _muteBGM = false;
        _muteSFX = false;

        _bgmVolume = 0.5f;
        _sfxVolume = 0.5f;

        _selectedType = GameMode.Type.Collect;
        _selectedArtworkKey = 1;
        _selectedArtworkSectionIndex = Vector2Int.zero;
    }

    [JsonIgnore] public bool MuteBGM { get => _muteBGM; set => _muteBGM = value; }
    [JsonIgnore] public bool MuteSFX { get => _muteSFX; set => _muteSFX = value; }
    [JsonIgnore] public float BgmVolume { get => _bgmVolume; set => _bgmVolume = value; }
    [JsonIgnore] public float SfxVolume { get => _sfxVolume; set => _sfxVolume = value; }
    [JsonIgnore] public GameMode.Type SelectedType { get => _selectedType; set => _selectedType = value; }
    [JsonIgnore] public int SelectedArtworkKey { get => _selectedArtworkKey; set => _selectedArtworkKey = value; }
    [JsonIgnore] public Vector2Int SelectedArtworkSectionIndex { get => _selectedArtworkSectionIndex; set => _selectedArtworkSectionIndex = value; }

    [JsonIgnore] public float SelectedArtworkProgress { get => ((_selectedArtworkSectionIndex.x * ArtworkSize) + _selectedArtworkSectionIndex.y) / (ArtworkSize * ArtworkSize);  }

    [JsonIgnore] const float ArtworkSize = 4;

    //[JsonIgnore] public Vector2Int SelectedArtworkSectionIndex 
    //{
    //    get
    //    {
    //        int mod = 4;
    //        int x = SelectedArtworkSectionIndex % mod;
    //        int y = SelectedArtworkSectionIndex / mod;
    //        return new Vector2Int(y, x);
    //    }
    //    set
    //    {
    //        _selectedArtworkSectionIndex = value.y * 4 + value.x;
    //    }
    //}
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
    }

    public void ChangeSFXVolume(float volume)
    {
        _saveData.SfxVolume = volume;
    }

    public void ChangeGameModeType(GameMode.Type type) 
    {
        _saveData.SelectedType = type;
        Save();
    }

    public void SelectArtwork(int selectedArtworkIndex) 
    {
        _saveData.SelectedArtworkKey = selectedArtworkIndex;
        Save();
    }

    public void SelectArtworkSection(Vector2Int index) 
    {
        _saveData.SelectedArtworkSectionIndex = index;
        Save();
    }

    public void SelectArtworkSection(int selectedArtworkSectionIndex) 
    {
        _saveData.SelectedArtworkSectionIndex = new Vector2Int(selectedArtworkSectionIndex / 4, selectedArtworkSectionIndex % 4);
        Save();
    }
}
