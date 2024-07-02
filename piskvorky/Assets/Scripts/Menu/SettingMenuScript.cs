using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenuScript : MonoBehaviour
{
    [SerializeField]
    private MenuHistoryManager menuController;

    [SerializeField]
    private Slider volumeSlider;
    [SerializeField]
    private AudioSource cameraAudio;
    void Start()
    {
        AudioScript.INSTANCE.gameObject.GetComponent<AudioSource>().volume = volumeSlider.value;
    }

    public void OnValueChange()
    {
        float volume = volumeSlider.value;
        AudioScript.INSTANCE.gameObject.GetComponent<AudioSource>().volume = volume;
    }

    public void BackMenu()
    {
        menuController.CloseCurrentMenu();
    }
}
