using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 声音管理器
/// </summary>
public class AudioManager : MonoBehaviour
{
    public AudioSource _EffectAudio;
    public AudioSource _Bgm;
    public AudioSource _SoundAudio;
    /// <summary>
    /// 初始化
    /// </summary>
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// 播放按钮音效
    /// </summary>
    /// <param name="audioEffectType">音效类型</param>
    /// <param name="PlayTime">播放比例</param>
    public void f_PlayAudioEffect()
    {
        _EffectAudio.loop = false;
        _EffectAudio.volume = StaticValue.m_fEffectVolume;
        _EffectAudio.Play();
    }

    /// <summary>
    /// 播放游戏背景音乐
    /// </summary>
    /// <param name="audioMusicType">音乐类型</param>
    /// <param name="isLoop">是否循环播放</param>
    public void f_PlayAudioMusic(bool isLoop = true)
    {
        _Bgm.loop = isLoop;
        _Bgm.volume = StaticValue.m_fBgmVolume;
        _Bgm.Play();
    }

    /// <summary>
    /// 播放特效音效
    /// </summary>
    /// <param name="audioEffectType">音效类型</param>
    /// <param name="PlayTime">播放比例</param>
    public void f_PlayAudioSound()
    {
        _SoundAudio.volume = StaticValue.m_fSoundVolume;
        _SoundAudio.Play();
    }

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void f_StopAudioMusic()
    {
        if (_Bgm == null) { return; }
        _Bgm.Stop();
    }
    public void f_StopAudioButtle()
    {
        _EffectAudio.Stop();
        _SoundAudio.Stop();
    }

    public void f_PlayAudioButtle()
    {
        _EffectAudio.Play();
        _SoundAudio.Play();
    }
    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void f_PauseAudioMusic()
    {
        if (_Bgm == null) { return; }
        _Bgm.Pause();
        _Bgm.enabled = false;
    }
    public void f_PauseAudioButtle()
    {
        _EffectAudio.enabled = false;
        _SoundAudio.enabled = false;
    }

    public void f_UnPauseAudioButtle()
    {
        _EffectAudio.clip = null;
        _SoundAudio.clip = null;
        _EffectAudio.enabled = true;
        _SoundAudio.enabled = true;
    }

    public void f_SetBGMVolume(float value)
    {
        if (_Bgm == null) { return; }
        _Bgm.volume = value;
    }
}