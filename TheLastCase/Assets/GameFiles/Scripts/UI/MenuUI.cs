using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{

    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private AudioMixer audioMixer;

    public void Start()
    {
        VolumeChange(.5f);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResolutionChange()
    {
        if (dropdown.value == 0)
        {
            Screen.SetResolution(1920, 1080, true);
        }
        else if (dropdown.value == 1)
        {
            Screen.SetResolution(1366, 768, true);
        }
        else if (dropdown.value == 2)
        {
            Screen.SetResolution(1280, 1024, true);
        }
    }

    public void VolumeChange(float sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20f);
    }
}
