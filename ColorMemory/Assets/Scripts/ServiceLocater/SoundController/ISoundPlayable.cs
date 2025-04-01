using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundPlayable
{
    public enum SoundName
    {
        
    }

    void Initialize(Dictionary<SoundName, AudioClip> clipDictionary);

    void MuteBGM(bool nowMute);
    void MuteSFX(bool nowMute);


    void SetBGMVolume(float volume = 1);
    void SetSFXVolume(float volume = 1);


    bool GetBGMMute();
    bool GetSFXMute();

    void PlayBGM(SoundName name, float volume = 1);
    void PlaySFX(SoundName name, float volume = 1);
    void PlaySFX(SoundName name, Vector3 pos, float volume = 1);

    void StopBGM();
    void StopAllSound();
}
