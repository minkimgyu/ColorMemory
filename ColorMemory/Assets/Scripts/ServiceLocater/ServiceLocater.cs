using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocater
{
    static ICoroutineRunner _coroutineRunner;
    static NullCoroutineRunner _nullCoroutineRunner;

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

    static IAdManager _adManager;
    static NullAdManager _nullAdManager;

    static IIAPManager _iAPManager;
    static NullIAPManager _nullIAPManager;

    static ServiceLocater()
    {
        _nullCoroutineRunner = new NullCoroutineRunner();
        _nullSoundPlayer = new NullSoundPlayer();
        _nullSceneController = new NullSceneController();
        _nullSaveManager = new NullSaveManager();
        _nullTimeController = new NullTimeController();
        _nullGpgsManager = new NullGPGSManager();
        _nullLocalizationManager = new NullLocalizationManager();
        _nullAdManager = new NullAdManager();
        _nullIAPManager = new NullIAPManager();
    }

    public static void Provide(IAdManager aDController)
    {
        _adManager = aDController;
    }

    public static void Provide(ICoroutineRunner coroutineRunner)
    {
        _coroutineRunner = coroutineRunner;
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

    public static void Provide(IIAPManager iAPManager)
    {
        _iAPManager = iAPManager;
    }


    public static IIAPManager ReturnIAPManager()
    {
        if (_iAPManager == null) return _nullIAPManager;
        return _iAPManager;
    }

    public static IAdManager ReturnAdManager()
    {
        if (_adManager == null) return _nullAdManager;
        return _adManager;
    }

    public static ICoroutineRunner ReturnCoroutineRunner()
    {
        if (_coroutineRunner == null) return _nullCoroutineRunner;
        return _coroutineRunner;
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
