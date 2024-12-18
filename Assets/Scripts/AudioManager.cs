using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource, sfxSource;

    public AudioClip background;
    public AudioClip pieceWalk;
    public AudioClip pieceKill;
    public AudioClip king;
    public AudioClip iceSkill;
    public AudioClip timeSkill;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip sfx)
    {
        sfxSource.PlayOneShot(sfx);
    }
}
