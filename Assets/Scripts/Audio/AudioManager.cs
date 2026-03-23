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

    [Header("SFX Trim - skip silent/low-freq beginning (seconds)")]
    [Tooltip("Set per-clip trim offset in Inspector to skip slow attack")]
    public float trimIceBreak = 0f;
    public float trimIceCrack = 0f;
    public float trimOpenDoor = 0f;
    public float trimSpikes = 0f;
    public float trimButton = 0f;
    public float trimTeleportation = 0f;
    public float trimCrystal = 0f;
    public float trimGiveUpSkills = 0f;
    public float trimChangeScence = 0f;
    public float trimPullBox = 0f;
    public float trimTrapsFilled = 0f;

    // Separate AudioSources for music and SFX to avoid delay
    private AudioSource musicSource;
    private AudioSource sfxSource;

    // Cached trimmed clips
    private Dictionary<AudioClip, AudioClip> trimmedClipCache = new Dictionary<AudioClip, AudioClip>();

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

    void Start()
    {
        // Pre-trim all clips at startup so playback is instant
        PreTrimClip(ref IceBreakClip, trimIceBreak);
        PreTrimClip(ref IceCrackClip, trimIceCrack);
        PreTrimClip(ref OpenDoorClip, trimOpenDoor);
        PreTrimClip(ref SpikesClip, trimSpikes);
        PreTrimClip(ref ButtonClip, trimButton);
        PreTrimClip(ref teleportationClip, trimTeleportation);
        PreTrimClip(ref CrystalClip, trimCrystal);
        PreTrimClip(ref GiveUpSkillsClip, trimGiveUpSkills);
        PreTrimClip(ref ChangeScenceClip, trimChangeScence);
        PreTrimClip(ref PullBoxClip, trimPullBox);
        PreTrimClip(ref TrapsFilledClip, trimTrapsFilled);
    }

    /// <summary>
    /// Trims the beginning of an AudioClip by the specified seconds.
    /// Creates a new clip from the trimmed data and replaces the reference.
    /// </summary>
    private void PreTrimClip(ref AudioClip clip, float trimSeconds)
    {
        if (clip == null || trimSeconds <= 0f) return;

        int trimSamples = Mathf.FloorToInt(trimSeconds * clip.frequency);
        int totalSamples = clip.samples;
        int remainingSamples = totalSamples - trimSamples;

        if (remainingSamples <= 0) return;

        float[] fullData = new float[totalSamples * clip.channels];
        clip.GetData(fullData, 0);

        float[] trimmedData = new float[remainingSamples * clip.channels];
        System.Array.Copy(fullData, trimSamples * clip.channels, trimmedData, 0, trimmedData.Length);

        AudioClip trimmedClip = AudioClip.Create(
            clip.name + "_trimmed",
            remainingSamples,
            clip.channels,
            clip.frequency,
            false
        );
        trimmedClip.SetData(trimmedData, 0);
        clip = trimmedClip;
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
