using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource audioSource;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            audioSource.Play();
        } 
        if (Input.GetKeyDown(KeyCode.S))
        {
            audioSource.Pause();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            audioSource.Stop();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}
