using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinSFX : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;
    void Start()
    {
        source.PlayOneShot(clip);
    }
}
