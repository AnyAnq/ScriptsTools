using System;
using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.UI;

public class VideoSliderControl : MonoBehaviour
{
    [Header("视频播放控制器")] public MediaPlayer _mediaPlayer;
    [Header("视频控制进度条")] public Slider _videoSeekSlider;
    [Header("音量")] public Slider _volumeSlider;
    [Header("视频进度时间控件")] public Text time;

    //视频进度值
    private float _setVideoSeekSliderValue;

    //视频播放状态
    private bool _wasPlayingOnScrub;


    private void Start()
    {
        _videoSeekSlider.onValueChanged.AddListener(value => { OnVideoSeekSlider(); });
        _volumeSlider.onValueChanged.AddListener(value => { OnVideoVolumeSlider(); });
    }

    private void Update()
    {
        //判断控制video

        var _all = (int) float.Parse((_mediaPlayer.Info.GetDurationMs() * 0.001f).ToString("F1"));
        var allTime = new TimeSpan(0, 0, _all);

        var _current = (int) float.Parse((_mediaPlayer.Control.GetCurrentTimeMs() * 0.001f).ToString("F1"));
        var currentTime = new TimeSpan(0, 0, _current);

        time.text = currentTime.Hours + ":" + currentTime.Minutes + ":" + currentTime.Seconds + "/" + allTime.Hours +
                    ":" + allTime.Minutes + ":" + allTime.Seconds;
        if (_mediaPlayer && _mediaPlayer.Info != null && _mediaPlayer.Info.GetDurationMs() > 0f)
        {
            var timer = _mediaPlayer.Control.GetCurrentTimeMs();
            var d = timer / _mediaPlayer.Info.GetDurationMs();
            _setVideoSeekSliderValue = d;
            _videoSeekSlider.value = d;
        }

        _volumeSlider.value = _mediaPlayer.Control.GetVolume();
    }


    /// <summary>
    /// on value changed事件
    /// </summary>
    void OnVideoSeekSlider()
    {
        if (_mediaPlayer && _videoSeekSlider && _videoSeekSlider.value != _setVideoSeekSliderValue)
            _mediaPlayer.Control.Seek(_videoSeekSlider.value * _mediaPlayer.Info.GetDurationMs());
    }

    void OnVideoVolumeSlider()
    {
        if (_mediaPlayer)
        {
            _mediaPlayer.Control.SetVolume(_volumeSlider.value);
        }
    }
}