using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundPlayable
{
    public enum SoundName
    {
        LobbyBGM,
        ChallengeBGM,
        CollectBGM,

        ChallengeResultBGM,
        CollectResultBGM,

        BtnClick,
        DotClick,
        GameOver,
        GameClear,
        StageClear,

        HintClick
    }

    void Initialize(Dictionary<SoundName, AudioClip> clipDictionary,
        float initBgmVolume,
        float initSfxVolume);

    void MuteBGM(bool nowMute);
    void MuteSFX(bool nowMute);


    void SetBGMVolume(float volume = 1);
    void SetSFXVolume(float volume = 1);


    bool GetBGMMute();
    bool GetSFXMute();

    void PlayBGM(SoundName name);
    void PlaySFX(SoundName name);
    void PlaySFX(SoundName name, Vector3 pos);

    void StopBGM();
    void StopAllSound();
}
