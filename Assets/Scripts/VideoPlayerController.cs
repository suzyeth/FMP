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
    public VideoPlayer endingVideo;
    public GameObject start;
    public GameObject end;

    public static VideoPlayerController Instance; // ����ʵ��


    string startingvideo_Path = "";//��Ƶ·��
    string endingVideo_Path = "";
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

        // ȡ��֮ǰ��loopPointReached�¼��Ķ���
        //startingVideo.loopPointReached -= OnStartVideoEnd;
        // ����loopPointReached�¼�
        startingVideo.loopPointReached += OnStartVideoEnd;
        // ������Ƶ
        startingVideo.Play();
    }

    // �¼���������
     void OnStartVideoEnd(VideoPlayer vp)
    {
        // ȡ���¼��Ķ���
        vp.loopPointReached -= OnStartVideoEnd;
        // �����ﴦ����Ƶ������ɺ���߼������������һ������
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


    public void PlayEndingVideo()
    {
        endingVideo_Path = Application.streamingAssetsPath + "/Endin.mp4";
        endingVideo.url = endingVideo_Path;

        end.SetActive(true);
        AudioManager.Instance.StopPlayBackGroundMusic();
        endingVideo.loopPointReached += OnEndVideoEnd;
        endingVideo.Play();
    }
    void OnEndVideoEnd(VideoPlayer vp)
    {
        // ȡ���¼��Ķ���
        vp.loopPointReached -= OnEndVideoEnd;
        // �����ﴦ����Ƶ������ɺ���߼������������һ������
        GameMgr.Instance.levelMgr.ChangLevel(0);

    }
    public void SkipEndVideo()
    {
        endingVideo.Stop();
        end.SetActive(false);
        GameMgr.Instance.levelMgr.ChangLevel(0);
        AudioManager.Instance.PlayBackGroundMusic();
    }

}
