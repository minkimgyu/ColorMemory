using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour, ISoundPlayable
{
    Dictionary<ISoundPlayable.SoundName, AudioClip> _clipDictionary;

    AudioSource _bgmPlayer;
    AudioSource[] _sfxPlayer;

    float _sfxVolume = 1;

    public void Initialize(
        Dictionary<ISoundPlayable.SoundName, AudioClip> clipDictionary,
        float initBgmVolume,
        float initSfxVolume)
    {
        _clipDictionary = clipDictionary;

        GameObject bgmPlayerObject = new GameObject("bgmPlayer");
        bgmPlayerObject.transform.SetParent(transform);

        _bgmPlayer = bgmPlayerObject.AddComponent<AudioSource>();
        SetBGMVolume(initBgmVolume);
        _bgmPlayer.loop = true;

        SetSFXVolume(initSfxVolume);
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBGM(ISoundPlayable.SoundName name)
    {
        if (_clipDictionary.ContainsKey(name) == false) return;
        _bgmPlayer.clip = _clipDictionary[name];
        _bgmPlayer.Play();
    }

    public void PlaySFX(ISoundPlayable.SoundName name, Vector3 pos)
    {
        if (_nowSFXMute == true) return;
        if (_clipDictionary.ContainsKey(name) == false) return;
        AudioSource.PlayClipAtPoint(_clipDictionary[name], pos, _sfxVolume);
    }

    public void PlaySFX(ISoundPlayable.SoundName name)
    {
        if (_nowSFXMute == true) return;
        if (_clipDictionary.ContainsKey(name) == false) return;
        AudioSource.PlayClipAtPoint(_clipDictionary[name], Vector3.zero, _sfxVolume);
    }

    public void SetBGMVolume(float volume = 1)
    {
        _bgmPlayer.volume = volume * 0.3f;
        ServiceLocater.ReturnSaveManager().ChangeBGMVolume(volume);
    }

    public void SetSFXVolume(float volume = 1)
    {
        _sfxVolume = volume;
        ServiceLocater.ReturnSaveManager().ChangeSFXVolume(volume);
    }

    public void StopBGM()
    {
        _bgmPlayer.Stop();
    }

    public void StopAllSound()
    {
        _bgmPlayer.Stop();
        for (int i = 0; i < _sfxPlayer.Length; i++)
        {
            _sfxPlayer[i].Stop();
        }
    }

    public bool GetBGMMute() { return _bgmPlayer.mute; }
    public bool GetSFXMute() { return _nowSFXMute; }

    bool _nowSFXMute = false;

    public void MuteBGM(bool nowMute)
    {
        _bgmPlayer.mute = nowMute;
    }

    public void MuteSFX(bool nowMute)
    {
        _nowSFXMute = nowMute;

        for (int i = 0; i < _sfxPlayer.Length; i++)
        {
            _sfxPlayer[i].mute = _nowSFXMute;
        }
    }
}
