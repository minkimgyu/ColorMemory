using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullSoundPlayer : ISoundPlayable
{
    public bool GetBGMMute() { return default; }
    public bool GetSFXMute() { return default; }

    public void Initialize(Dictionary<ISoundPlayable.SoundName, AudioClip> clipDictionary) { }

    public void MuteBGM(bool nowMute) { }
    public void MuteSFX(bool nowMute) { }

    public void PlayBGM(ISoundPlayable.SoundName name, float volumn = 1) { }

    public void PlaySFX(ISoundPlayable.SoundName name, float volumn = 1) { }
    public void PlaySFX(ISoundPlayable.SoundName name, Vector3 pos, float volumn = 1) { }

    public void StopAllSound() { }
    public void StopBGM() { }
}
