using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Clips")]
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

    // Separate AudioSources for music and SFX to avoid delay
    private AudioSource musicSource;
    private AudioSource sfxSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // Use existing AudioSource for music
            musicSource = GetComponent<AudioSource>();
            if (musicSource == null)
                musicSource = gameObject.AddComponent<AudioSource>();

            // Create a dedicated AudioSource for sound effects
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;

            // Start background music
            musicSource.clip = BackgroundClip;
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Background Music

    public void PlayBackGroundMusic()
    {
        musicSource.clip = BackgroundClip;
        musicSource.Play();
    }

    public void StopPlayBackGroundMusic()
    {
        musicSource.Stop();
    }

    #endregion

    #region Sound Effects

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void OpenDoor()
    {
        PlaySFX(OpenDoorClip);
    }

    public void IceBreak()
    {
        PlaySFX(IceBreakClip);
    }

    public void IceCrack()
    {
        PlaySFX(IceCrackClip);
    }

    public void SpikesSound()
    {
        PlaySFX(SpikesClip);
    }

    public void uiButtonSound()
    {
        PlaySFX(ButtonClip);
    }

    public void teleportationSound()
    {
        PlaySFX(teleportationClip);
    }

    public void CrystalSound()
    {
        PlaySFX(CrystalClip);
    }

    public void GiveUpSkillsSound()
    {
        PlaySFX(GiveUpSkillsClip);
    }

    public void ChangeScenceSound()
    {
        PlaySFX(ChangeScenceClip);
    }

    public void PullBoxSound()
    {
        PlaySFX(PullBoxClip);
    }

    public void TrapsFilledSound()
    {
        PlaySFX(TrapsFilledClip);
    }

    #endregion
}
