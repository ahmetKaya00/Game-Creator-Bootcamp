using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public void NewGame()
    {
        float bgMusicVolume = PlayerPrefs.GetFloat("BGMusicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetFloat("BGMusicVolume", bgMusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);

        PlayerPrefs.Save();

        SceneManager.LoadScene("Selector");
    }
}
