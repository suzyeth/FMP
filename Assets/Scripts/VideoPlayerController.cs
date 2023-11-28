using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    public VideoPlayer startingVideo;
    public VideoPlayer endingVdeo;
    public GameObject start;
    public GameObject end;

    public static VideoPlayerController Instance; // ����ʵ��


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
