using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource bgMusicSource;
    public AudioSource[] sfxSource;

    private float bgMusicVolume = 1.0f;
    private float sfxVolume = 1.0f;
    private float bgMusicTime = 0f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += onSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        LoadVolumeSettings();
    }

    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignAudioSources();

        if(bgMusicSource != null)
        {
            bgMusicSource.time = bgMusicTime;
            if (!bgMusicSource.isPlaying)
            {
                bgMusicSource.Play();
            }

        }
        ApplyVolumeSettings();
    }

    private void AssignAudioSources()
    {
        bgMusicSource = GameObject.Find("bg")?.GetComponent<AudioSource>();

        if(bgMusicSource != null)
        {
            bgMusicSource.loop = true;
        }

        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        sfxSource = System.Array.FindAll(allAudioSources, source => source != bgMusicSource);

    }

    private void LoadVolumeSettings()
    {
        bgMusicVolume = PlayerPrefs.GetFloat("BGMusicVolume", 1f);

        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    private void ApplyVolumeSettings()
    {
        if(bgMusicSource != null)
        {
            bgMusicSource.volume = bgMusicVolume;
        }

        foreach(var sfx in sfxSource)
        {
            if(sfx != null)
            {
                sfx.volume = sfxVolume;
            }
        }
    }

    public void SetBGMusicVolume(float volume)
    {
        bgMusicVolume = volume;
        PlayerPrefs.SetFloat("BGMusicVolume", bgMusicVolume);
        ApplyVolumeSettings();
    }
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        ApplyVolumeSettings();
    }

    private void Update()
    {
        if(bgMusicSource != null && bgMusicSource.isPlaying)
        {
            bgMusicTime = bgMusicSource.time;
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("BGMusicVolume", bgMusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }




}
