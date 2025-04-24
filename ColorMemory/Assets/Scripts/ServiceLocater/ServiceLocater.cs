using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocater
{
    static ISoundPlayable _soundPlayer;
    static NullSoundPlayer _nullSoundPlayer;

    static ITimeController _timeController;
    static NullTimeController _nullTimeController;

    static ISceneControllable _sceneController;
    static NullSceneController _nullSceneController;

    static ISaveable _saveManager;
    static NullSaveManager _nullSaveManager;

    static IGPGS _gpgsManager;
    static NullGPGSManager _nullGpgsManager;

    static ILocalization _localizationManager;
    static NullLocalizationManager _nullLocalizationManager;

    static ServiceLocater()
    {
        _nullSoundPlayer = new NullSoundPlayer();
        _nullSceneController = new NullSceneController();
        _nullSaveManager = new NullSaveManager();
        _nullTimeController = new NullTimeController();
        _nullGpgsManager = new NullGPGSManager();
        _nullLocalizationManager = new NullLocalizationManager();
    }

    public static void Provide(ISoundPlayable soundPlayer)
    {
        _soundPlayer = soundPlayer;
    }

    public static void Provide(ITimeController timeController)
    {
        _timeController = timeController;
    }

    public static void Provide(ISceneControllable sceneController)
    {
        _sceneController = sceneController;
    }

    public static void Provide(ISaveable saveable)
    {
        _saveManager = saveable;
    }

    public static void Provide(IGPGS gpgs)
    {
        _gpgsManager = gpgs;
    }

    public static void Provide(ILocalization localization)
    {
        _localizationManager = localization;
    }

    public static ISoundPlayable ReturnSoundPlayer()
    {
        if (_soundPlayer == null) return _nullSoundPlayer;
        return _soundPlayer;
    }

    public static ITimeController ReturnTimeController()
    {
        if (_timeController == null) return _nullTimeController;
        return _timeController;
    }

    public static ISceneControllable ReturnSceneController()
    {
        if (_sceneController == null) return _nullSceneController;
        return _sceneController;
    }

    public static ISaveable ReturnSaveManager()
    {
        if (_saveManager == null) return _nullSaveManager;
        return _saveManager;
    }

    public static IGPGS ReturnGPGSManager()
    {
        if (_gpgsManager == null) return _nullGpgsManager;
        return _gpgsManager;
    }

    public static ILocalization ReturnLocalizationManager()
    {
        if (_localizationManager == null) return _nullLocalizationManager;
        return _localizationManager;
    }
}
