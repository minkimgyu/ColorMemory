using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

public interface ISaveable
{
    bool VerifyJson(string json) { return default; }
    bool HaveSaveFile() { return false; }

    void Save() { }
    void ClearSave() { }

    void Load() { }

    void ChangeLanguage(ILocalization.Language language) { }
    void ChangeGoToCollectPage(bool goToCollectPage) { }

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

public struct SaveData
{
    [JsonProperty] string _userId;
    [JsonProperty] string _userName;
    [JsonProperty] bool _muteBGM;
    [JsonProperty] bool _muteSFX;

    [JsonProperty] float _bgmVolume;
    [JsonProperty] float _sfxVolume;
    [JsonProperty] [JsonConverter(typeof(StringEnumConverter))] GameMode.Type _selectedType;

    [JsonProperty] int _selectedArtworkKey;
    [JsonProperty] Vector2Int _selectedArtworkSectionIndex;
    [JsonProperty] bool _goToCollectPage;

    [JsonProperty] [JsonConverter(typeof(StringEnumConverter))] ILocalization.Language _language;

    public SaveData(string id, string name)
    {
        _userId = id;
        _userName = name;

        _muteBGM = false;
        _muteSFX = false;

        _bgmVolume = 0.5f;
        _sfxVolume = 0.5f;

        _selectedType = GameMode.Type.Collect;
        _selectedArtworkKey = 0;
        _selectedArtworkSectionIndex = Vector2Int.zero;
        _goToCollectPage = false;
        _language = ILocalization.Language.Korean;
    }

    [JsonIgnore] public bool MuteBGM { get => _muteBGM; set => _muteBGM = value; }
    [JsonIgnore] public bool MuteSFX { get => _muteSFX; set => _muteSFX = value; }
    [JsonIgnore] public float BgmVolume { get => _bgmVolume; set => _bgmVolume = value; }
    [JsonIgnore] public float SfxVolume { get => _sfxVolume; set => _sfxVolume = value; }
    [JsonIgnore] public GameMode.Type SelectedType { get => _selectedType; set => _selectedType = value; }
    [JsonIgnore] public int SelectedArtworkKey { get => _selectedArtworkKey; set => _selectedArtworkKey = value; }
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
    [JsonIgnore] public ILocalization.Language Language { get => _language; set => _language = value; }

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
        // 파일이 존재하지 않는다면
        if (!HaveSaveFile())
        {
            _saveData = _defaultSaveData;
            Save(); // 세이브 파일을 만들어주고 저장한다.
        }

        // 저장된 파일을 불러서 리턴한다.
        string json = File.ReadAllText(_filePath);
        return json;
    }


    /// <summary>
    /// GPGS용 데이터 검증
    /// 만약 서버에서 받은 데이터가 고장난 경우 기존 데이터를 삭제하지 않고
    /// 그대로 사용
    /// </summary>

    public bool VerifyJson(string json)
    {
        // 불러오는 중 오류가 있다면 기본 데이터를 넘겨준다.
        try
        {
            _saveData = _parser.JsonToObject<SaveData>(json);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            _saveData = _defaultSaveData;
            return false; // 유효하지 않음
        }

        return true; // 유효함
    }

    public bool HaveSaveFile()
    {
        return File.Exists(_filePath);
    }

    public void Load()
    {
        // 파일이 존재하지 않는다면
        if (!HaveSaveFile())
        {
            _saveData = _defaultSaveData; // 기본 세이브로 대체해준다.
            Save();
            return;
        }

        string json = File.ReadAllText(_filePath);
        bool nowValidate = VerifyJson(json);
        Save();
    }

    void Save()
    {
        string json = _parser.ObjectToJson(_saveData);
        File.WriteAllText(_filePath, json);
    }

    public void ChangeLanguage(ILocalization.Language language) 
    { 
        _saveData.Language = language;
        Save();
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
