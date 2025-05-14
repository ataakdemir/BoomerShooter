using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header(" Audio Source ")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Main Music")]
    public AudioClip mainMusicClip;

    [Header(" SFX Clips ")]

    public AudioClip bowFireSound;
    public AudioClip nailgunFireSound;
    public AudioClip wandFireSound;
    public AudioClip[] pipeHitSounds;
    public AudioClip playerHurtSound;
    public AudioClip healthPickupSound;
    public AudioClip[] enemyDeathSounds;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PlayMainMusic();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void PlayMainMusic()
    {
        musicSource.clip = mainMusicClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
