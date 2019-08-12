using System;
using GameFramework.Sound;

public class SoundAgentHelper : ISoundAgentHelper
{
    /// <summary>
    /// 获取当前是否正在播放。
    /// </summary>
    public bool IsPlaying
    {
        get;
    }

    /// <summary>
    /// 获取声音长度。
    /// </summary>
    public float Length
    {
        get;
    }

    /// <summary>
    /// 获取或设置播放位置。
    /// </summary>
    public float Time
    {
        get;
        set;
    }

    /// <summary>
    /// 获取或设置是否静音。
    /// </summary>
    public bool Mute
    {
        get;
        set;
    }

    /// <summary>
    /// 获取或设置是否循环播放。
    /// </summary>
    public bool Loop
    {
        get;
        set;
    }

    /// <summary>
    /// 获取或设置声音优先级。
    /// </summary>
    public int Priority
    {
        get;
        set;
    }

    /// <summary>
    /// 获取或设置音量大小。
    /// </summary>
    public float Volume
    {
        get;
        set;
    }

    /// <summary>
    /// 获取或设置声音音调。
    /// </summary>
    public float Pitch
    {
        get;
        set;
    }

    /// <summary>
    /// 获取或设置声音立体声声相。
    /// </summary>
    public float PanStereo
    {
        get;
        set;
    }

    /// <summary>
    /// 获取或设置声音空间混合量。
    /// </summary>
    public float SpatialBlend
    {
        get;
        set;
    }

    /// <summary>
    /// 获取或设置声音最大距离。
    /// </summary>
    public float MaxDistance
    {
        get;
        set;
    }

    /// <summary>
    /// 获取或设置声音多普勒等级。
    /// </summary>
    public float DopplerLevel
    {
        get;
        set;
    }

    /// <summary>
    /// 重置声音代理事件。
    /// </summary>
    public event EventHandler<ResetSoundAgentEventArgs> ResetSoundAgent
    {
        add {}
        remove {}
    }

    /// <summary>
    /// 播放声音。
    /// </summary>
    /// <param name="fadeInSeconds">声音淡入时间，以秒为单位。</param>
    public void Play(float fadeInSeconds)
    {

    }

    /// <summary>
    /// 停止播放声音。
    /// </summary>
    /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
    public void Stop(float fadeOutSeconds)
    {

    }

    /// <summary>
    /// 暂停播放声音。
    /// </summary>
    /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
    public void Pause(float fadeOutSeconds)
    {

    }

    /// <summary>
    /// 恢复播放声音。
    /// </summary>
    /// <param name="fadeInSeconds">声音淡入时间，以秒为单位。</param>
    public void Resume(float fadeInSeconds)
    {

    }

    /// <summary>
    /// 重置声音代理辅助器。
    /// </summary>
    public void Reset()
    {
    }

    /// <summary>
    /// 设置声音资源。
    /// </summary>
    /// <param name="soundAsset">声音资源。</param>
    /// <returns>是否设置声音资源成功。</returns>
    public bool SetSoundAsset(object soundAsset)
    {
        return true;
    }
}
