using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour, ISoundPlayable
{
    Dictionary<ISoundPlayable.SoundName, AudioClip> _clipDictionary;

    AudioSource _bgmPlayer;
    AudioSource[] _sfxPlayer;

    [SerializeField] GameObject _bgmPlayerObject;
    [SerializeField] GameObject _sfxPlayerObject;

    public void Initialize(Dictionary<ISoundPlayable.SoundName, AudioClip> clipDictionary)
    {
        _clipDictionary = clipDictionary;
        _bgmPlayer = _bgmPlayerObject.GetComponent<AudioSource>();
        _bgmPlayer.loop = true;

        _sfxPlayer = _sfxPlayerObject.GetComponents<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBGM(ISoundPlayable.SoundName name, float volumn = 1)
    {
        if (_clipDictionary.ContainsKey(name) == false) return;
        _bgmPlayer.clip = _clipDictionary[name];


        _bgmPlayer.volume = volumn;
        _bgmPlayer.Play();
    }

    public void PlaySFX(ISoundPlayable.SoundName name, Vector3 pos, float volumn = 1)
    {
        if (_nowSFXMute == true) return;
        if (_clipDictionary.ContainsKey(name) == false) return;
        AudioSource.PlayClipAtPoint(_clipDictionary[name], pos, volumn);

    }

    public void PlaySFX(ISoundPlayable.SoundName name, float volumn = 1)
    {
        if (_clipDictionary.ContainsKey(name) == false) return;

        for (int i = 0; i < _sfxPlayer.Length; i++)
        {
            if (_sfxPlayer[i].isPlaying == true) continue;
            _sfxPlayer[i].clip = _clipDictionary[name];

            _sfxPlayer[i].volume = volumn;
            _sfxPlayer[i].Play();
            break;
        }
    }

    public void SetBGMVolume(float volume = 1)
    {
        _bgmPlayer.volume = volume;
        ServiceLocater.ReturnSaveManager().ChangeBGMVolume(volume);
    }

    public void SetSFXVolume(float volume = 1)
    {
        for (int i = 0; i < _sfxPlayer.Length; i++)
        {
            _sfxPlayer[i].volume = volume;
        }

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
