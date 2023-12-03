using DG.Tweening.Plugins.Core.PathCore;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.IO;
using Path = System.IO.Path;

public class VideoPlayerController : MonoBehaviour
{
    public VideoPlayer startingVideo;
    public VideoPlayer endingVdeo;
    public GameObject start;
    public GameObject end;

    public static VideoPlayerController Instance; // 单例实例


    string startingvideo_Path = "";//视频路径
    string endingVdeo_Path = "";//视频路径
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            startingVideo.frame = 0;

        }
        else
        {
            Destroy(gameObject);
        }

    }

   

    public void PlayStartVideo()
    {
        
        startingvideo_Path = Application.streamingAssetsPath + "/Opennin.mp4";
        startingVideo.url = startingvideo_Path;
        startingVideo.frame = 0;
        start.SetActive(true);
        AudioManager.Instance.StopPlayBackGroundMusic();

        // 取消之前的loopPointReached事件的订阅
        //startingVideo.loopPointReached -= OnStartVideoEnd;
        // 订阅loopPointReached事件
        startingVideo.loopPointReached += OnStartVideoEnd;
        // 播放视频
        startingVideo.Play();
    }

    // 事件处理方法
     void OnStartVideoEnd(VideoPlayer vp)
    {
        // 取消事件的订阅
        vp.loopPointReached -= OnStartVideoEnd;
        // 在这里处理视频播放完成后的逻辑，例如加载下一个场景
        start.SetActive(false);
        GameMgr.Instance.levelMgr.ChangeMap();
        AudioManager.Instance.PlayBackGroundMusic();
    }

    public void SkipStartVideo()
    {
        startingVideo.Stop();
        start.SetActive(false);
        GameMgr.Instance.levelMgr.ChangeMap();
        AudioManager.Instance.PlayBackGroundMusic();
    }


    public void PlayendingVdeo()
    {
        //endingVdeo = GetComponent<VideoPlayer>();
        endingVdeo_Path = Application.streamingAssetsPath + "/Endin.mp4";
        endingVdeo.url = endingVdeo_Path;

        end.SetActive(true);
        AudioManager.Instance.StopPlayBackGroundMusic();
        // 取消之前的loopPointReached事件的订阅
       // endingVdeo.loopPointReached -= OnEndVideoEnd;
        // 订阅loopPointReached事件
        endingVdeo.loopPointReached += OnEndVideoEnd;
        // 播放视频
        endingVdeo.Play();
    }
    void OnEndVideoEnd(VideoPlayer vp)
    {
        // 取消事件的订阅
        vp.loopPointReached -= OnEndVideoEnd;
        // 在这里处理视频播放完成后的逻辑，例如加载下一个场景
        GameMgr.Instance.levelMgr.ChangLevel(0);

    }
    public void SkipEndVideo()
    {
        endingVdeo.Stop();
        start.SetActive(false);
        GameMgr.Instance.levelMgr.ChangLevel(0);
        AudioManager.Instance.PlayBackGroundMusic();
    }

}
