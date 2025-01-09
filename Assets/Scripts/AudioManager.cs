using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public bool isSelectScene = false;

    [SerializeField] private AudioSource musicSource, sfxSource;

    [Header("Sound Effect")]
    public AudioClip background;
    public AudioClip pieceWalk;
    public AudioClip pieceKill;
    public AudioClip king;
    public AudioClip iceSkill;
    public AudioClip timeSkill;

    public AudioClip ui_select;
    public AudioClip ui_error;
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
        if (isSelectScene) return;

        musicSource.clip = background;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip sfx)
    {
        sfxSource.PlayOneShot(sfx);
    }
}
