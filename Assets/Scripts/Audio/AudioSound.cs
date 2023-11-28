using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSound : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    //�ı�����
    const float pitchMin = 0.9f;
    const float pitchMax = 1.1f;

    //�����жϾ������
    private bool IsMuteMaster = false;
    private bool IsMuteMusic = false;
    private bool IsMuteSound = false;
    //���ڴ��澲��ǰ������
    private float LastMaster;
    private float LastMusic;
    private float LastSound;

    //��������
    //Slider on click��������������������ȡ������
    public void MasterSldOnClick(GameObject image, Slider slider)
    {
        audioMixer.SetFloat("vMaster", slider.value);
        if (IsMuteMaster == false) return;
        else
        {
            image.SetActive(false);
            IsMuteMaster = false;
        }
    }
    public void MusicSldOnClick(GameObject image, Slider slider)
    {
        audioMixer.SetFloat("vMusic", slider.value);
        if (IsMuteMusic == false) return;
        else
        {
            image.SetActive(false);
            IsMuteMusic = false;
        }
    }
    public void SoundSldOnClick(GameObject image, Slider slider)
    {
        audioMixer.SetFloat("vSound", slider.value);
        if (IsMuteSound == false) return;
        else
        {
            image.SetActive(false);
            IsMuteSound = false;
        };
    }

    //Button on click����������ȡ���������ص���������δ������������������
    public void MasterBtnOnClick(GameObject image, Slider Master)
    {
        if (IsMuteMaster)
        {
            image.SetActive(false);
            IsMuteMaster = false;
            Master.value = LastMaster;
        }

        else
        {
            image.SetActive(true);
            LastMaster = Master.value;
            Master.value = Master.minValue;
            IsMuteMaster = true;
        }
    }
    public void SoundBtnOnClick(GameObject image, Slider Sound)
    {
        if (IsMuteSound)
        {
            image.SetActive(false);
            IsMuteSound = false;
            Sound.value = this.LastSound;
        }

        else
        {
            image.SetActive(true);
            LastSound = Sound.value;
            Sound.value = Sound.minValue;
            IsMuteSound = true;
        }
    }
    public void MusicBtnOnClick(GameObject image, Slider Music)
    {
        if (IsMuteMusic)
        {
            image.SetActive(false);
            IsMuteMusic = false;
            Music.value = LastMusic;
        }

        else
        {
            image.SetActive(true);
            LastMusic = Music.value;
            Music.value = Music.minValue;
            IsMuteMusic = true;
        }
    }


    [SerializeField] AudioSource SoundPlayer;
    //������Ч
    public void PlaySound(AudioClip audioClip)
    {
        SoundPlayer.pitch = 1;
        SoundPlayer.PlayOneShot(audioClip);
    }

    // �ı���������Ҫ�����ظ����ŵ���Ч
    public void PlayRandomSound(AudioClip audioClip)
    {
        SoundPlayer.pitch = Random.Range(pitchMin, pitchMax);
        SoundPlayer.PlayOneShot(audioClip);
    }

    public void PlayRandomSound(AudioClip[] audioClip)
    {
        PlayRandomSound(audioClip[Random.Range(0, audioClip.Length)]);
    }

}
