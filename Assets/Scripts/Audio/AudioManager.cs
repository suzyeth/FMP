using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance; // 单例实例
    public AudioClip IceBreakClip;
    public AudioClip IceCrackClip;
    public AudioClip OpenDoorClip;
    public AudioClip SpikesClip;
    public AudioClip ButtonClip;
    public AudioClip teleportationClip;
    public AudioClip CrystalClip;
    public AudioClip GiveUpSkillsClip;
    public AudioClip ChangeScenceClip;
    public AudioClip PullBoxClip;
    public AudioClip TrapsFilledClip;
    public AudioClip BackgroundClip;



    // 将你的音频文件拖拽到这里
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = BackgroundClip;
            audioSource.Play();

        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void PlayBackGroundMusic()
    {
       
        audioSource.clip = BackgroundClip;
        audioSource.Play();
        audioSource.Stop();
    }
    public void StopPlayBackGroundMusic()
    {
        audioSource.Stop();
    }

    public void OpenDoor()
    {
        audioSource.PlayOneShot(OpenDoorClip);
    }

    public void IceBreak()
    {
        audioSource.PlayOneShot(IceBreakClip);
    }

    public void IceCrack()
    {
        audioSource.PlayOneShot(IceCrackClip);
    }

    public void SpikesSound()
    {
        audioSource.PlayOneShot(SpikesClip);
    }

    public void uiButtonSound()
    {
        audioSource.PlayOneShot(ButtonClip);
    }

    public void teleportationSound()
    {
        audioSource.PlayOneShot(teleportationClip);
    }

    public void CrystalSound()
    {
        audioSource.PlayOneShot(CrystalClip);
    }

    public void GiveUpSkillsSound()
    {
        audioSource.PlayOneShot(GiveUpSkillsClip);
    }

    public void ChangeScenceSound()
    {
        audioSource.PlayOneShot(ChangeScenceClip);
    }

    public void PullBoxSound()
    {
        audioSource.PlayOneShot(PullBoxClip);
    }

    public void TrapsFilledSound()
    {
        audioSource.PlayOneShot(TrapsFilledClip);
    }

}


