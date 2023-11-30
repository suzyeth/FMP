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

    public static VideoPlayerController Instance; // ����ʵ��


    string startingvideo_Path = "";//��Ƶ·��
    string endingVdeo_Path = "";//��Ƶ·��
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            

        }
        else
        {
            Destroy(gameObject);
        }

    }


    public void PlayStartVideo()
    {
        //startingVideo = GetComponent<VideoPlayer>();
        startingvideo_Path = Application.streamingAssetsPath + "/Opennin.mp4";
        startingVideo.url = startingvideo_Path;

        start.SetActive(true);
        AudioManager.Instance.StopPlayBackGroundMusic();
        // ����loopPointReached�¼�
        startingVideo.loopPointReached += OnStartVideoEnd;
        // ������Ƶ
        startingVideo.Play();
    }

    // �¼�������
    void OnStartVideoEnd(VideoPlayer vp)
    {
        // ȡ���¼��Ķ���
        vp.loopPointReached -= OnStartVideoEnd;
        // �����ﴦ����Ƶ������ɺ���߼������������һ������
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
        // ����loopPointReached�¼�
        endingVdeo.loopPointReached += OnEndVideoEnd;
        // ������Ƶ
        endingVdeo.Play();
    }
    void OnEndVideoEnd(VideoPlayer vp)
    {
        // ȡ���¼��Ķ���
        vp.loopPointReached -= OnEndVideoEnd;
        // �����ﴦ����Ƶ������ɺ���߼������������һ������
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

    }

}
