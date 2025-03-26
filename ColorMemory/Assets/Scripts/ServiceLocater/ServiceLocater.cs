using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocater
{
    static ISoundPlayable _soundPlayer;
    static NullSoundPlayer _nullSoundPlayer;

    static ISceneControllable _sceneController;
    static NullSceneController _nullSceneController;

    static ISaveable _saveManager;
    static NullSaveManager _nullSaveManager;

    static ServiceLocater()
    {
        _nullSoundPlayer = new NullSoundPlayer();
        _nullSceneController = new NullSceneController();
        _nullSaveManager = new NullSaveManager();
    }

    public static void Provide(ISoundPlayable soundPlayer)
    {
        _soundPlayer = soundPlayer;
    }

    public static void Provide(ISceneControllable sceneController)
    {
        _sceneController = sceneController;
    }

    public static void Provide(ISaveable saveable)
    {
        _saveManager = saveable;
    }

    public static ISoundPlayable ReturnSoundPlayer()
    {
        if (_soundPlayer == null) return _nullSoundPlayer;
        return _soundPlayer;
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
}
