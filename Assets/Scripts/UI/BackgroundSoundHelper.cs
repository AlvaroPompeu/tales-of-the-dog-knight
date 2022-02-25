using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSoundHelper : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClip environment;
    [SerializeField] AudioClip bossFight;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        SwitchAudioClip("Environment");
    }

    public void SwitchAudioClip(string clip)
    {
        if (clip == "Environment")
        {
            audioSource.clip = environment;
            audioSource.Play();
        }
        else
        {
            audioSource.clip = bossFight;
            audioSource.Play();
        }
    }
}
