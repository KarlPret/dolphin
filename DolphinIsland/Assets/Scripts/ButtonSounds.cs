using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;

    public AudioClip audioClip2;
    public AudioClip audioClip3;
    public AudioClip audioClip4;
    public AudioClip audioClip5;
    public AudioClip audioClip6;
    public AudioClip audioClip7;

    public void PlayButtonSound()
    {
        audioSource.Stop();
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void SettingsButtonSound()
    {
        audioSource.Stop();
        audioSource.clip = audioClip2;
        audioSource.Play();
    }

    public void QuitButtonSound()
    {
        audioSource.Stop();
        audioSource.clip = audioClip3;
        audioSource.Play();
    }

    public void MenuButtonSound()
    {
        audioSource.Stop();
        audioSource.clip = audioClip4;
        audioSource.Play();
    }

    public void CloseButtonSound()
    {
        audioSource.Stop();
        audioSource.clip = audioClip5;
        audioSource.Play();
    }

    public void NextButtonSound()
    {
        audioSource.Stop();
        audioSource.clip = audioClip6;
        audioSource.Play();
    }

    public void RetryButtonSound()
    {
        audioSource.Stop();
        audioSource.clip = audioClip7;
        audioSource.Play();
    }
}
